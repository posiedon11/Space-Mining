using Assets.Scripts.Interfaces;
using Assets.Scripts.Misc;
using Assets.Scripts.Objects.Turrets.Projectiles;
using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts.Managers
{
    public abstract class BasePoolManager<TBase, TManager> : MonoBehaviour 
        where TBase : MonoBehaviour, IPoolable 
        where TManager : BasePoolManager<TBase, TManager>

    {
        public static TManager Instance { get; private set; }

        [SerializeField] protected HashSet<GameObject> objectPrefabs = new HashSet<GameObject>();

        protected Dictionary<GameObject, ObjectPool<TBase>> objectPools = new Dictionary<GameObject, ObjectPool<TBase>>();

        protected Dictionary<TBase, GameObject> activePoolable = new Dictionary<TBase, GameObject>();

        [SerializeField] protected int defaultCapacity = 10;
        [SerializeField] protected int maxCapacity = 50;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = (TManager)this;
                //DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
            objectPools = new Dictionary<GameObject, ObjectPool<TBase>>();

            InitializePools();
        }


        private void InitializePools()
        {
            foreach (var prefab in objectPrefabs)
            {
                AddToPool(prefab);
            }
        }
        protected void AddToPool(GameObject prefab)
        {
            if (objectPools.ContainsKey(prefab))
            {
                DebugLogger.LogWarning(DebugData.DebugType.Pools, $"Prefab {prefab.name} already in pool");
                return;
            }

            if (!prefab.TryGetComponent(out TBase component))
            {
                DebugLogger.LogError(DebugData.DebugType.Pools, $"Could not add Prefab {prefab.name}, as it is not or does not have a {typeof(TBase).Name} component.");
                return;
            }

            if (!objectPools.ContainsKey(prefab))
            {
                objectPools[prefab] = new ObjectPool<TBase>(() =>
                    CreatePoolable(prefab),
                    OnGetFromPool,
                    OnReleaseToPool,
                    OnDestroyPoolable,
                    true,
                    defaultCapacity,
                    maxCapacity
                );
            }
        }

        protected TBase CreatePoolable(GameObject prefab)
        {
            TBase poolable = Instantiate(prefab, transform).GetComponent<TBase>();

            if (poolable == null)
            {
                DebugLogger.LogError(DebugData.DebugType.Pools, $"Failed to instantiate poolable from prefab '{prefab.name}'. Ensure it has a component implementing '{typeof(TBase).Name}'.");
                return null;
            }
            DebugLogger.Log(DebugData.DebugType.Pools, $"Instantiated {poolable.poolTag}: {poolable.name}");
            poolable.pickupPrefab = prefab;

            return poolable;
        }
        protected void OnGetFromPool(TBase poolable)
        {
            if (poolable.pickupPrefab == null)
            {
                DebugLogger.LogWarning(DebugData.DebugType.Pools, $"Failed to find prefab for {typeof(TBase).Name}:  {poolable.name}");
                return;
            }
            poolable.gameObject.SetActive(true);

            activePoolable[poolable] = poolable.pickupPrefab;
        }
        protected void OnReleaseToPool(TBase poolable)
        {
            if (!activePoolable.ContainsKey(poolable))
            {
                DebugLogger.LogWarning(DebugData.DebugType.Pools, $"Attempted to release a {poolable.poolTag} not tracked: {poolable.name}");
                return;
            }
            activePoolable.Remove(poolable);
            poolable.gameObject.SetActive(false);
            poolable.transform.SetParent(transform);
        }

        protected void OnDestroyPoolable(TBase projectile)
        {
            Destroy(projectile.gameObject);
        }

        public TBase GetPoolable(GameObject prefab)
        {
            if (objectPools.TryGetValue(prefab, out ObjectPool<TBase> pool))
            {
                TBase poolable = pool.Get();
                if (poolable.pickupPrefab != prefab)
                {
                    DebugLogger.LogError(DebugData.DebugType.Pools, $"{poolable.poolTag} {prefab.name} does not match {poolable.pickupPrefab.name}");

                }
                return poolable;
                //return pool.Get();
            }
            DebugLogger.LogError(DebugData.DebugType.Pools, $"{typeof(TBase).Name}:  {prefab.name} not found in pool");
            return null;
        }
        protected void ReleasePoolable(TBase poolable)
        {
            if (activePoolable.TryGetValue(poolable, out GameObject prefab))
            {
                if (objectPools.TryGetValue(prefab, out ObjectPool<TBase> pool))
                {
                    pool.Release(poolable);
                    return;
                }
            }
            DebugLogger.LogWarning(DebugData.DebugType.Pools, $"{poolable.poolTag} {poolable.name} not found in pool");

            //OnDestroyProjectile(projectile);
        }
    }
}

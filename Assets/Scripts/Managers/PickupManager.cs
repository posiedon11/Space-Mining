using Assets.Scripts.Misc;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Objects.Pickups;
using Assets.Scripts.Objects.Turrets.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Scripts.Managers
{

    public class PickupManager : BasePoolManager<BasePickup, PickupManager>
    {
        public BasePickup GetPickup(GameObject prefab)
        {
            return GetPoolable(prefab);
        }
        public void ReleasePickup(BasePickup pickup)
        {
            ReleasePoolable(pickup);
        }
        public void AddPickupToPool(GameObject prefab)
        {
            AddToPool(prefab);
        }


        public BasePickup<TItem> GetPickup<TItem>(GameObject prefab) where TItem : BaseItem
        {
            BasePickup pickup = GetPoolable(prefab);

            if (pickup is BasePickup<TItem> typedPickup)
            {
                return typedPickup;
            }

            Debug.LogError($"Prefab {prefab.name} is not a BasePickup<{typeof(TItem).Name}>.");
            return null;
        }

        // Generic method to release a specific type of pickup
        public void ReleasePickup<TItem>(BasePickup<TItem> pickup) where TItem : BaseItem
        {
            //ReleasePoolable(pickup);
        }

    }


    //public class PickupManager : MonoBehaviour
    //{
    //    public static PickupManager Instance { get; private set; }

    //    private Dictionary<int, ObjectPool<BasePickup>> pickupPools = new Dictionary<int, ObjectPool<BasePickup>>();

    //    private int defaultCapacity = 10, maxCapacity = 50;

    //    private void Awake()
    //    {
    //        if (Instance == null)
    //        {
    //            Instance = this;
    //        }
    //        else
    //        {
    //            Destroy(this);
    //        }
    //    }

    //    public void AddPickupToPool(GameObject prefab)
    //    {
    //        if (!prefab.TryGetComponent(out BasePickup pickup))
    //        {
    //            Debug.LogError($"Could not add prefab {prefab.name} to pool, as it does not have a BasePickup component.");
    //            return;
    //        }
    //        Int32 itemId = pickup.itemId;
    //        if (!pickupPools.ContainsKey(itemId))
    //        {
    //            pickupPools[itemId] = new ObjectPool<BasePickup>(
    //                () => CreatePickup(prefab),
    //                OnGetFromPool,
    //                OnReleaseToPool,
    //                OnDestroyPickup,
    //                true,
    //                defaultCapacity,
    //                maxCapacity
    //            );
    //        }
    //    }

    //    private BasePickup CreatePickup(GameObject prefab)
    //    {
    //        GameObject obj = Instantiate(prefab);
    //        BasePickup pickup = obj.GetComponent<BasePickup>();
    //        return pickup;
    //    }

    //    private void OnGetFromPool(BasePickup pickup)
    //    {
    //        pickup.gameObject.SetActive(true);
    //    }

    //    private void OnReleaseToPool(BasePickup pickup)
    //    {
    //        pickup.gameObject.SetActive(false);
    //        pickup.transform.SetParent(transform);
    //    }

    //    private void OnDestroyPickup(BasePickup pickup)
    //    {
    //        Destroy(pickup.gameObject);
    //    }

    //    public BasePickup GetPickup(int itemId)
    //    {
    //        if (pickupPools.TryGetValue(itemId, out ObjectPool<BasePickup> pool))
    //        {
    //            return pool.Get();
    //        }
    //        Debug.LogError($"Pickup with ID {itemId} not found in pool.");
    //        return null;
    //    }

    //    public void ReleasePickup(BasePickup pickup)
    //    {
    //        if (pickupPools.TryGetValue(pickup.itemId, out ObjectPool<BasePickup> pool))
    //        {
    //            pool.Release(pickup);
    //        }
    //    }
    //}

}

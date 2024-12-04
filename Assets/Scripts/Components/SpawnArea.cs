using Assets.Scripts.Interfaces;
using Assets.Scripts.Misc;
using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class SpawnArea:BaseArea
    {
        [SerializeField] private List<ISpawnable> spawnables = new List<ISpawnable>();
        [SerializeField] private List<SpawnableData> spawnableDataList = new List<SpawnableData>();
        [SerializeField] private int spawnCount = 10;

        public void Spawn()
        {
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnableData spawnableData = GetRandomSpawnable();
                if (spawnableData == null) return;

                Vector3 spawnPosition = GetRandomPositionInArea();
                Quaternion rotation = Quaternion.identity;

                TrySpawn(spawnableData, spawnPosition, rotation);
            }
        }
        private Vector3 GetRandomPositionInArea()
        {
            switch (shape)
            {
                case AreaShape.Box:
                    {
                        float x = UnityEngine.Random.Range(-halfSize.x, halfSize.x);
                        float y = UnityEngine.Random.Range(-halfSize.y, halfSize.y);
                        return transform.position + new Vector3(x, y, 0f);
                    }
                case AreaShape.Circle:
                    {
                        float angle = UnityEngine.Random.Range(0f, 2 * MathF.PI);
                        float distance = UnityEngine.Random.Range(0f, radius);
                        float x = distance * math.cos(angle);
                        float y = distance * math.sin(angle);
                        return transform.position + new Vector3(x, y, 0f);
                    }
                default:
                    return transform.position;
            }
        }

        private SpawnableData GetRandomSpawnable()
        {
            float totalWeight = 0f;
            foreach (var data in spawnableDataList)
            {
                totalWeight += data.weight;
            }

            float randomValue = UnityEngine.Random.Range(0, totalWeight);
            float cumulativeWeight = 0f;

            foreach (var data in spawnableDataList)
            {
                cumulativeWeight += data.weight;
                if (randomValue <= cumulativeWeight)
                {
                    return data;
                }
            }

            return null; // Should not happen if weights are set correctly
        }

        private void TrySpawn(SpawnableData spawnableData, Vector3 position, Quaternion rotation)
        {
            if (!spawnableData.IsSpawnable()) return;

            GameObject instance = Instantiate(spawnableData.prefab, position, rotation, transform);
            if (instance.TryGetComponent(out ISpawnable spawnable))
            {
                spawnable.Initialize();
            }
            else
            {
                DebugLogger.LogWarning(DebugData.DebugType.Other, $"Prefab {spawnableData.prefab.name} does not implement ISpawnable.");
                //Debug.LogWarning($"Prefab {spawnableData.prefab.name} does not implement ISpawnable.");

                Destroy(instance);
            }

        }
    }
}

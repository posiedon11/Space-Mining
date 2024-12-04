using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "New Spawnable", menuName = "Spawn System/Spawnable")]
    public class SpawnableData : ScriptableObject
    {
        public GameObject prefab; // Reference to the prefab
        public float weight = 1f; // Weight for randomized spawning

        public bool IsSpawnable()
        {
            return prefab != null && prefab.GetComponent<ISpawnable>() != null;
        }
    }

}

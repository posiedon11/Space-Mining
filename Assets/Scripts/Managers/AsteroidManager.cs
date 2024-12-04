using Assets.Scripts.Managers;
using Assets.Scripts.Misc;
using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Objects.Rocks
{
    public class AsteroidManager : MonoBehaviour
    {

        [SerializeField] private SpawnArea spawnArea;

        private void Awake()
        {
            spawnArea = GetComponent<SpawnArea>();
        }
        private void Start()
        {
            SpawnRocks();

        }
        public void SpawnRocks()
        {
            DebugLogger.Log(DebugData.DebugType.Gameplay, "Spawning rocks...");
            spawnArea.Spawn();

        }
    }
    
}

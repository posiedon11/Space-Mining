using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IPoolable
    {
        public GameObject pickupPrefab { get; set; }
        public string poolTag { get; set; }
    }
}

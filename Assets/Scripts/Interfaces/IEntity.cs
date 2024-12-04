using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IEntity
    {
        public Rigidbody2D rb { get; set; }
        public HealthComponent health { get; set; }

        public bool enabled { get; set; }
    }
}

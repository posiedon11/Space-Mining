using Assets.Scripts.Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{ 
    public interface IDamagable
    {
        bool isKillable { get; set; }
        float deathDelay { get; set; }
        void TakeDamage(DamageInfo damage);
        void Die(DamageReport damage);

    }
}

using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Scriptable_Objects;

namespace Assets.Scripts.Misc
{


    public class DamageReport
    {
        public IEntity Reciever;
        public IEntity Attacker;

        public DamageType DamageType;
        public ElementalType? ElementalType;

        public DamageInfo damageInfo;
        public float finalDamage;

        public DamageReport( IEntity _reciever, DamageInfo _damageInfo)
        {
            damageInfo = _damageInfo;

            Reciever = _reciever;
            Attacker = _damageInfo.attacker;
            
            //ApplyDamage();
        }
        public void ApplyDamage()
        {
            if (Reciever is IDamagable)
            {
                (Reciever as IDamagable).TakeDamage(damageInfo);
                PrintDamageInstance();
            }
            else
            {
                DebugLogger.Log(DebugData.DebugType.Damage, $"{Reciever.GetType().Name} is not Damagable");
            }
        }
        public void PrintDamageInstance()
        {
            string elementalInfo = ElementalType.HasValue ? $"({ElementalType.Value})" : "";
            DebugLogger.Log(DebugData.DebugType.Damage, $"Damage Instance: {Attacker.GetType().Name} dealt {finalDamage} {DamageType} damage {elementalInfo} to {Reciever.GetType().Name}");
            //Debug.Log($"Damage Instance: {Attacker.GetType().Name} dealt {DamageAmount} {DamageType} damage {elementalInfo} to {Reciever.GetType().Name}");
        }

        public void PrintDamageReport()
        {
            string damageReportString = $"Damage Report: {Attacker.GetType().Name} dealt {finalDamage} {DamageType} damage to {Reciever.GetType().Name}";
            DebugLogger.Log(DebugData.DebugType.Damage, damageReportString);

        }
    }
}

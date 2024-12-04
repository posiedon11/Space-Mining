using Assets.Scripts.Interfaces;
using Assets.Scripts.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Scriptable_Objects;

namespace Assets.Scripts.Managers

{
    public class HurtBox: MonoBehaviour
    {
        [SerializeField]private Collider2D hurtBoxCollider;
        [SerializeField] private HealthComponent healthComponent;

        [SerializeField] private Color gizmoColor;
        public delegate void HurtEvent(DamageReport damageReport);
        public event HurtEvent OnHurt;

        private IEntity owner;


        private void Awake()
        {
            if (!hurtBoxCollider) hurtBoxCollider = GetComponent<Collider2D>();
            hurtBoxCollider.isTrigger = true; // Ensure it's a trigger

            owner = Tools.FindTopLevelEntity<IEntity>(transform);
            if (owner == null)
            {
                DebugLogger.LogError(DebugData.DebugType.HitBox, $"Hurtbox on {name} has no associated IEntity in its hierarchy.");
            }
            healthComponent = GetComponentInParent<HealthComponent>();
            if(healthComponent == null)
            {
                DebugLogger.LogError(DebugData.DebugType.HitBox, $"Hurtbox on {name} has no associated HealthComponent in its hierarchy.");
            }

        }
        public void TakeHit(DamageInfo damageInfo)
        {

            DamageReport report = new DamageReport(owner, damageInfo);
            if(healthComponent != null)
            {
                healthComponent.TakeDamage(damageInfo);
            }
            //healthComponent?.TakeDamage(damageInfo);
            OnHurt?.Invoke(report);
        }

        private void OnDrawGizmos()
        {
            if (hurtBoxCollider != null)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawWireCube(hurtBoxCollider.bounds.center, hurtBoxCollider.bounds.size);
            }
        }
    }
}

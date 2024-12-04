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
    public class HitBox : MonoBehaviour
    {
        [SerializeField] private Collider2D hitBoxCollider;
        [SerializeField] private LayerMask targetLayers;

        [SerializeField] private Color gizmoColor;
        public bool continuousHit = false;
        private bool isEntered = false;

        public delegate void HitEvent(Collider2D other, DamageInfo damageInfo);
        public event HitEvent OnHit;

        private Collider2D other;

        [SerializeField] private IEntity owner;
        private DamageInfo damageInfo;

        private void Awake()
        {
            if (!hitBoxCollider) hitBoxCollider = GetComponent<Collider2D>();

            hitBoxCollider.isTrigger = true;
            owner = Tools.FindTopLevelEntity<IEntity>(transform);

            if (owner == null)
            {
                DebugLogger.LogWarning(DebugData.DebugType.HitBox, $"Hitbox on {name} has no associated IEntity in its hierarchy. Set it via SetOwner");
            }

        }
        public void FixedUpdate()
        {
            if (isEntered && continuousHit)
            {
                TriggerCheck(other);
            }
        }

        public void SetDamageInfo(DamageInfo damageInfo)
        {
            this.damageInfo = damageInfo;
        }

        public void UpdateBoxCollider(Vector2 size, Vector2 offset)
        {
            if (hitBoxCollider is BoxCollider2D boxCollider)
            {
                boxCollider.offset = offset;
                boxCollider.size = size;
            }
        }
        public void SetTargetMask(LayerMask targetMask)
        {
            this.targetLayers = targetMask;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            isEntered = true;
            this.other = other;
            TriggerCheck(other);
        }

        private void TriggerCheck(Collider2D other)
        {

            if ((targetLayers & (1 << other.gameObject.layer)) != 0)
            {
                DebugLogger.Log(DebugData.DebugType.HitBox, $"Hitbox {name} hit {other.name}");
                OnHit?.Invoke(other, damageInfo);

                //if (other.TryGetComponent<HurtBox>(out HurtBox hurtBox))
                //{
                //    if (damageInfo != null)
                //    {
                //        hurtBox.TakeHit(damageInfo);
                //    }
                //}

            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            isEntered = false;
            this.other = null;

        }


        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!continuousHit) return;

            TriggerCheck(collision);
        }

        public void EnableHitbox(bool enable)
        {
            hitBoxCollider.enabled = enable;
        }
        public void SetOwner(IEntity owner)
        {
            if (owner == null) return;

            this.owner = owner;
            return;
        }

        private void OnDrawGizmos()
        {
            if (hitBoxCollider != null)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawWireCube(hitBoxCollider.bounds.center, hitBoxCollider.bounds.size);
            }
        }
    }
}

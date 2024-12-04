using Assets.Scripts.Interfaces;
using Assets.Scripts.Managers;
using Assets.Scripts.Misc;
using Assets.Scripts.Scriptable_Objects;
using UnityEngine;

namespace Assets.Scripts.Objects.Turrets.Projectiles
{
    public class BaseBeam : MonoBehaviour
    {
        private HitBox hitBox;
        protected IEntity owner;


        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] public float damage = 50f;
        [SerializeField] private LayerMask targetMask;

        [SerializeField] private float lineWidth = 0.1f;
        [SerializeField] private float range = 5f;

        [SerializeField] private DamageInfo damageInfo;
        [SerializeField] protected DamageType damageType = DamageType.Physical;
        [SerializeField] protected Material beamMat;

        [SerializeField] protected Gradient beamGradient;


        // Start is called once before the first execution of Update after the MonoBehaviour is created

        private void Awake()
        {
            hitBox = GetComponent<HitBox>();
            if (hitBox == null)
            {
                DebugLogger.LogWarning(DebugData.DebugType.Turrets, "HitBox not found on BeamController, adding it");
                gameObject.AddComponent<HitBox>();
            }
            lineRenderer = GetComponentInParent<LineRenderer>();
            if (lineRenderer == null)
            {
                DebugLogger.LogWarning(DebugData.DebugType.Turrets, "LineRenderer not found on BeamController, adding it");
                //lineRenderer = gameObject.AddComponent<LineRenderer>();
            }
            else
            {
                lineRenderer.colorGradient = beamGradient;
                lineRenderer.material = beamMat;
            }
            hitBox.OnHit += HandleHit;
        }

        public void HandleHit(Collider2D other, DamageInfo damage)
        {
            DebugLogger.Log(DebugData.DebugType.Turrets, "Beam hit");
            if (other.TryGetComponent(out IEntity entity))
            {
                if (entity == owner) return;
            }

            if (other.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(damage);
            }
        }
        public void Initialize(IEntity owner, float lineWidth, float range, LayerMask targetMask)
        {
            this.lineWidth = lineWidth;
            this.range = range;
            this.owner = owner;

            // Update BoxCollider2D for hitbox
            Vector2 colliderSize = new Vector2(lineWidth, range);
            hitBox.UpdateBoxCollider(colliderSize, new Vector2(0, range / 2f));
            hitBox.SetTargetMask(targetMask);
            hitBox.SetTargetMask(targetMask);

            damageInfo = new DamageInfo(owner, damage, DamageType.Energy);
        }
        
        
        public void SetDamageInfo(DamageInfo damageInfo)
        {
            this.damageInfo = damageInfo;
            hitBox.SetDamageInfo(damageInfo);
        }
        public void UpdateBeam(Vector3 startPoint, Vector3 direction)
        {
            // Update LineRenderer
            Vector3 endPoint = startPoint + direction.normalized * range;

            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);
        }
        public void EnableBeam(bool enable)
        {
            lineRenderer.enabled = enable;
            hitBox.EnableHitbox(enable);
            enabled = enable;
        }
    }
}

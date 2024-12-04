
using Assets.Scripts.Interfaces;
using Assets.Scripts.Misc;
using Assets.Scripts.Objects.Turrets.Projectiles;
using Assets.Scripts.Scriptable_Objects;
using System;
using UnityEngine;
namespace Assets.Scripts.Objects.Turrets
{

    [Serializable]
    public abstract class BaseTurret : MonoBehaviour
    {
        public enum ControlMode { Manual, Auto }
        public enum MountSize { Small, Medium, Large }

        [SerializeField] protected ControlMode controlMode = ControlMode.Manual;
        [SerializeField] public MountSize mountSize;


        [SerializeField] protected float rotationSpeed = 5f;
        [SerializeField] protected float range = 20f;


        [SerializeField] protected Transform turretPivot;
        [SerializeField] protected Transform firePoint; // Point where the projectile spawn

        //LineStuff
        [SerializeField] protected LineRenderer lineRenderer;
        [SerializeField] protected float lineWidth = 0.1f;

        [SerializeField] protected GameObject firePrefab;
        [SerializeField] protected TurretMount turretMount;

        protected Transform target;
        protected Transform focalPoint;
        protected Vector3 aimDirection;

        protected IEntity owner;

        [SerializeField] protected LayerMask aimMask;
        [SerializeField] protected bool useBeam = false;
        [SerializeField] protected bool aimToFocal = false;
        [SerializeField] protected float fireRate = 1f;
        protected float lastFireTime;


        public virtual void Awake()
        {
            lastFireTime = Time.deltaTime;
            owner = Tools.FindTopLevelEntity<IEntity>(transform);
            if (owner == null)
            {
                DebugLogger.LogError(DebugData.DebugType.Turrets, "Turret owner not found");

            }
            if (lineRenderer == null)
                lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null)
            {
                lineRenderer = gameObject.AddComponent<LineRenderer>();
                lineRenderer.widthMultiplier = lineWidth;

                lineRenderer.enabled = useBeam;
            }
            turretMount = GetComponentInParent<TurretMount>();

            if (turretMount == null)
            {
                DebugLogger.LogError(DebugData.DebugType.Turrets, "TurretMount not found");
            }

        }
        public virtual void Start()
        {

        }
        public virtual void Initialize(Transform focalPoint, Transform turretPivot)
        {
            this.focalPoint = focalPoint;
            this.turretPivot = turretPivot;
            this.firePoint = turretPivot;

            if (aimToFocal)
            {
                target = focalPoint;
            }

        }

        protected virtual void FixedUpdate()
        {
            //lastFireTime += Time.deltaTime;
            if (controlMode == ControlMode.Manual)
            {
                ManualControl();
            }
            else if (controlMode == ControlMode.Auto)
            {
                AutoControl();
            }
            UpdateLineRenderer();
        }


        protected virtual void ManualControl()
        {
            if (aimToFocal)
            {
                target = focalPoint;
                aimDirection = target.position;
            }
            else
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                aimDirection = mousePos;
            }
            RotateTurret(aimDirection);

        }

        protected virtual void AutoControl()
        {
            if (target != null)
            {
                RotateTurret(target.position);
            }

            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.up, range, aimMask);

            if (hit.collider != null && CanFire() && IsValidTarget(hit.collider))
            {
                target = hit.collider.transform;
                Fire();
            }

        }
        protected virtual bool IsValidTarget(Collider2D other)
        {
            if (other.TryGetComponent(out IEntity entity))
            {
                return entity != owner;
            }
            return false;
        }


        protected virtual void RotateTurret(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - turretPivot.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // Adjust for Unity's "up" direction
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            turretPivot.rotation = Quaternion.RotateTowards(turretPivot.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }

        public virtual bool CanFire()
        {
            return Time.time >= lastFireTime + (1f / fireRate);
        }
        public virtual bool Fire()
        {
            if (!CanFire()) return false;

            lastFireTime = Time.time;
            return true;
        }

        public virtual void StopFiring()
        {
            if (!useBeam)
            {
                lineRenderer.enabled = false;
            }
        }
        protected virtual void UpdateLineRenderer()
        {
            if (lineRenderer == null) return;
            lineRenderer.enabled = useBeam;

            if (!useBeam)
            {
                return;
            }

            lineRenderer.SetPosition(0, turretPivot.position);
            lineRenderer.SetPosition(1, turretPivot.position + turretPivot.up * range); // Default beam range
        }

    }
}

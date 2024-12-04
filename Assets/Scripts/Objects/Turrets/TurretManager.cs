using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Objects.Turrets
{
    public class TurretManager: MonoBehaviour
    {
        
        [SerializeField] private Transform focalPoint; // Focal point for all turrets
        [SerializeField] private List<TurretMount> turretMounts = new List<TurretMount>(); // Turrets attached to this manager

        // Gizmo color for focal point visualization
        [SerializeField] private Color gizmoColor = Color.red;
        [SerializeField] private float gizmoSize = 0.5f;

        public Transform FocalPoint => focalPoint;
        private void Awake()
        {
            turretMounts.AddRange(GetComponentsInChildren<TurretMount>());
            foreach (TurretMount mount in turretMounts)
            {
                if(mount !=null && mount.CurrentTurret != null)
                {
                    mount.CurrentTurret.Initialize(focalPoint, mount.transform);
                }
            }
        }

        public void AddMount(TurretMount mount)
        {
            if (!turretMounts.Contains(mount))
            {
                turretMounts.Add(mount);
            }
        }

        public void RemoveMount(TurretMount mount)
        {
            if (turretMounts.Contains(mount))
            {
                turretMounts.Remove(mount);
            }
        }

        public void FireTurrets(System.Predicate<BaseTurret> condition)
        {
            foreach(var mount in turretMounts)
            {
                if (mount.CurrentTurret !=null && condition(mount.CurrentTurret))
                {
                    mount.TryFire();
                }
            }
        }

        public void StopFiringTurrets(System.Predicate<BaseTurret> condition)
        {
            foreach (var mount in turretMounts)
            {
                if (mount.CurrentTurret != null && condition(mount.CurrentTurret))
                {
                    mount.TryStopFire();
                }
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(focalPoint.position, gizmoSize);
        }

    }
}

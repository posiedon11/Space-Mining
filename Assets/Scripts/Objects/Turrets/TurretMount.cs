using Assets.Scripts.Misc;
using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Objects.Turrets
{
    public class TurretMount : MonoBehaviour
    {
        public enum TurretType
        {
            Mining,
            Combat,
        }
        [SerializeField] private BaseTurret turretPrefab; // Assign the turret prefab

        [SerializeField] private BaseTurret.MountSize mountSize; //what size of turret can be mounted
        private BaseTurret currentTurret;

        public BaseTurret CurrentTurret => currentTurret;
        private TurretManager turretManager;

        private void Awake()
        {
            turretManager = GetComponentInParent<TurretManager>();
            currentTurret = GetComponentInChildren<BaseTurret>();
            if (turretPrefab != null)
            {
                AttachTurret(turretPrefab);
            }
        }

        public void AttachTurret(BaseTurret turret)
        {
            if (currentTurret == null)
            {
                currentTurret = Instantiate(turret, transform.position, transform.rotation, transform);
            }

            if (currentTurret.mountSize != mountSize)
            {
                DebugLogger.Log(DebugData.DebugType.Turrets, "Turret size does not match mount size");
                //Debug.Log("Turret size does not match mount size");
                return;
            }
            if (currentTurret != null)
            {
                DebugLogger.Log(DebugData.DebugType.Turrets, "Turret Attached");

                currentTurret.Initialize(turretManager.FocalPoint, transform);
            }
        }

        public void DetachTurret()
        {
            if (currentTurret != null)
            {
                DebugLogger.Log(DebugData.DebugType.Turrets, "Turret detached");
                Destroy(currentTurret.gameObject);
                currentTurret = null;
            }
        }

        public void TryFire()
        {
            if (currentTurret != null)
            {
                currentTurret.Fire();
            }
        }
        public void TryStopFire()
        {
            if(currentTurret!= null)
            {
                currentTurret.StopFiring();
            }
        }
    }
}

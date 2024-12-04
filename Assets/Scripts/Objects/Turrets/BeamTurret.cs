using Assets.Scripts.Misc;
using Assets.Scripts.Objects.Rocks;
using Assets.Scripts.Scriptable_Objects;
using Assets.Scripts.Objects.Turrets.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Objects.Turrets
{
    [Serializable]
    public class BeamTurret: BaseTurret
    {
        [SerializeField] private float miningRate = 10f;
        [SerializeField] private BaseBeam beamController;


        public override void Awake()
        {
            base.Awake();
            beamController = GetComponent<BaseBeam>();
            if (beamController == null)
            {
                GameObject beamInstance = Instantiate(firePrefab, transform.position, Quaternion.identity, transform);
                beamController = beamInstance.GetComponent<BaseBeam>();
            }
            if (beamController == null)
            {
                DebugLogger.LogError(DebugData.DebugType.Turrets, "BeamController not found on MiningTurret");
            }


        }
        public override void Initialize(Transform focalPoint, Transform turretPivot)
        {
            base.Initialize(focalPoint, turretPivot);
            beamController.Initialize(owner, lineWidth, range, aimMask);
            //target = focalPoint;
            //Vector2 distanceToFoci = focalPoint.position - turretPivot.position;
            //range = distanceToFoci.magnitude;

        }
        public override bool Fire()
        {
            if (!base.Fire() || beamController == null) return false;

            if(beamController != null)
            {
                //beamController.Initialize(beamWidth, range, turretPivot.position, turretPivot.up, owner);
                beamController.EnableBeam(true);
                beamController.UpdateBeam(firePoint.position, firePoint.up);

                var damageInfo = new DamageInfo(owner, miningRate*Time.deltaTime, DamageType.Energy);
                beamController.SetDamageInfo(damageInfo);
            }

            return true;
        }
        public override void StopFiring()
        {
            base.StopFiring();
            if (beamController != null)
            {
                beamController.EnableBeam(false);
            }
        }
            }

}

using Assets.Scripts.Misc;
using Assets.Scripts.Objects.Turrets.Projectiles;
using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Objects.Turrets
{
    public class ProjectileTurret : BaseTurret
    {

        public override void Awake()
        {
            base.Awake();
        }
        public override void Start()
        {
            base.Start();
            if (firePrefab != null)
                ProjectileManager.Instance.AddProjectileToPool(firePrefab);

        }
        public override bool Fire()
        {
            if (!base.Fire()) return false;


            SpawnProjectile();

            return true;
        }

        protected virtual void SpawnProjectile()
        {
            if (firePrefab == null)
            {
                DebugLogger.LogError(DebugData.DebugType.Turrets, "Projectile prefab not set");
                return;
            }
            BaseProjectile projectile = ProjectileManager.Instance.GetProjectile(firePrefab);

            if (projectile != null)
            {
                projectile.Initialize(owner, owner.rb.linearVelocity.magnitude, firePoint.up, firePoint);
            }
        }
    }
}

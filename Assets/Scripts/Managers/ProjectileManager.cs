using Assets.Scripts.Managers;
using Assets.Scripts.Objects.Turrets.Projectiles;
using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts.Misc
{
    public class ProjectileManager : BasePoolManager<BaseProjectile, ProjectileManager>
    {

        public BaseProjectile GetProjectile(GameObject prefab)
        {
            return GetPoolable(prefab);
        }
        public void ReleaseProjectile(BaseProjectile projectile)
        {
            ReleasePoolable(projectile);
        }
        public void AddProjectileToPool(GameObject prefab)
        {
            AddToPool(prefab);
        }
    }
}

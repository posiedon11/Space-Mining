using Assets.Scripts.Managers;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Objects.Rocks
{
    // BaseRock is a base class for all rocks in the game
    [Serializable]
    public class BaseRock: BaseEntity, ISpawnable
    {

        public SpriteRenderer spriteRenderer { get; set; }

        public override void Awake()
        {
            base.Awake();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public virtual ISpawnable TrySpawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject temp = Instantiate(prefab, position, rotation);
            if (temp != null && temp.TryGetComponent(out ISpawnable spawnable))
            {
                return spawnable;
            }
            return null;

        }
        public virtual void Initialize()
        {
            health = GetComponent<HealthComponent>();
        }

    }
}

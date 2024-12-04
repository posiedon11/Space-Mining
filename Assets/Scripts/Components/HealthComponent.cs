using Assets.Scripts.Interfaces;
using Assets.Scripts.Misc;
using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Managers
{
    [Serializable]
    public abstract class BaseComponent
    {
        protected float current;

        [SerializeField] protected float max = 100;
        [SerializeField] protected float cap = 100;


        [SerializeField] protected float regenRate = 5f;
        [SerializeField] protected float regenDelay = 5f;
        [SerializeField] protected bool isGated = false; // used to see if damage can carry over to a component further down.
        protected float lastDamageTime = 0;

        [SerializeField] public float Current { get => current; }
        [SerializeField] public float Max { get => max; }
        [SerializeField] public bool IsGated { get => isGated; }

        public void Initialize(float maxValue, float regenRateValue, float regenDelayValue, bool gated = false)
        {
            max = maxValue;
            current = maxValue;
            cap = maxValue;
            regenRate = regenRateValue;
            regenDelay = regenDelayValue;
            isGated = gated;
        }
        public BaseComponent()
        {
            current = max;
        }
        public virtual void TakeDamage(float amount)
        {
            current = Mathf.Max(current - amount, 0); // Ensure health doesn’t go below 0
            lastDamageTime = Time.time;
        }

        public virtual void Regen()
        {
            if (Time.time - lastDamageTime > regenDelay)
            {
                current = Mathf.Min(current + regenRate * Time.deltaTime, max);
            }
        }

        public virtual bool IsDepleated()
        {
            return current <= 0;
        }
    }
    [Serializable]
    public class Health : BaseComponent
    {
        public Health()
        {
        }
        public void Heal(float amount)
        {
            current = MathF.Min(current + amount, max);
        }

    }
    [Serializable]

    public class Shield : BaseComponent
    {

        public Shield(bool _isGated = false)
        {
            isGated = _isGated;
        }
    }
    [Serializable]

    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] public Health health;
        [SerializeField] public Shield shield;
        [SerializeField] bool invincible = false;

        public UnityEvent<DamageReport> OnDamageTaken;
        public UnityEvent OnDeath;

        public bool isDead { get; private set; } = false;

        public IDamagable owner;
        public IEntity ownerEntity;
        public void Awake()
        {
            owner = GetComponent<IDamagable>();
            ownerEntity = GetComponent<IEntity>();
            health = new Health();
            shield = new Shield(_isGated: true);
    }

        public void TakeDamage(DamageInfo damage)
        {
            if(damage == null)
            {
                DebugLogger.LogError(DebugData.DebugType.Gameplay, "DamageInfo is null");
                return;
            }
            if (invincible)
            {
                return;
            }
            //damage calculations
            float remainingDamage = damage.baseDamage;


            //after Damage Calculations
            DamageReport report = new DamageReport(ownerEntity, damage);
            report.finalDamage = remainingDamage;

            //Shield Gating
            if (!shield.IsDepleated())
            {
                float absorbedDamage = Mathf.Min(remainingDamage, shield.Current);
                shield.TakeDamage(remainingDamage);
                remainingDamage = shield.IsGated ? 0 : remainingDamage - absorbedDamage;

            }

            if (!health.IsDepleated())
            {
                health.TakeDamage(remainingDamage);
                if (health.IsDepleated())
                {
                    OnDeath?.Invoke();
                    owner.Die(report);
                }
            }

            report.PrintDamageReport();
        }

        private void Update()
        {
            health.Regen();
            shield.Regen();
        }
    }
}

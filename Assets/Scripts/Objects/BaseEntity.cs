using Assets.Scripts.Interfaces;
using Assets.Scripts.Managers;
using Assets.Scripts.Misc;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Objects.Turrets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    [Serializable]
    public abstract class BaseEntity :MonoBehaviour, IEntity, IDamagable
    {
        [SerializeField] public Rigidbody2D rb { get; set; }
        [SerializeField] public Animator animator;
        [SerializeField] public AudioSource audioSource;

        [SerializeField] public HealthComponent health { get; set; }
        [SerializeField] public HurtBox hurtBox;

        [SerializeField] public Inventory inventory;


        [SerializeField] public bool isKillableField = true;
        [SerializeField] public float deathDelayField = 2.0f;
        public float deathDelay { get => deathDelayField; set => deathDelayField = value; }
        public bool isKillable { get => isKillableField; set => isKillableField = value; }
        #region Damage
        public virtual void TakeDamage(DamageInfo damage)
        {
            health.TakeDamage(damage);
        }

        public virtual void Die(DamageReport damage)
        {
            if (!isKillable)
            {
                return;
            }
            if(health.isDead)
            {
                return;
            }
            animator?.SetTrigger("OnDeath");
            if (animator != null)
                animator.speed = animator.GetCurrentAnimatorClipInfo(0).Length / deathDelay;

            StartCoroutine(DieAfterDelay());
        }
        public virtual void DeathEffects()
        {
            return;
        }
        protected IEnumerator DieAfterDelay()
        {
            DeathEffects();

            yield return new WaitForSeconds(deathDelay);
            Destroy(gameObject);
        }
        #endregion


        public virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            health = GetComponent<HealthComponent>();
            audioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
            inventory = GetComponent<Inventory>();
            hurtBox = GetComponent<HurtBox>();

            if (hurtBox == null)
            {
                hurtBox = gameObject.AddComponent<HurtBox>();
            }
            if(inventory == null)
            {
                inventory = gameObject.AddComponent<Inventory>();
            }
            if (health == null)
            {
                health = gameObject.AddComponent<HealthComponent>();
            }
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
            }


        }
    }
}

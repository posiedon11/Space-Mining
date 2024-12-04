using Assets.Scripts.Managers;
using Assets.Scripts.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Scriptable_Objects;
using Assets.Scripts.Interfaces;
using System.Collections;

namespace Assets.Scripts.Objects.Turrets.Projectiles
{
    public class BaseProjectile : MonoBehaviour, IPoolable
    {
        [SerializeField] protected float targetVelocity = 100f;
        [SerializeField] protected float accelDecel = 10f;
        [SerializeField] protected float maxLifeTime = 5f;
        [SerializeField] protected float damage = 50f;
        [SerializeField] protected DamageType damageType = DamageType.Physical;

        [SerializeField] private float remainingLifetime;
        [SerializeField] protected float explosionRadius = 3f;

        public GameObject pickupPrefab { get; set; }
        public string poolTag { get; set; }

        protected DamageInfo damageInfo;
        protected Vector2 direction;


        protected Rigidbody2D rb;
        protected HitBox hitBox;
        protected IEntity owner;

        [SerializeField] protected AudioSource spawnAudioSource;
        [SerializeField] public AudioClip hitSound;
        [SerializeField] public AudioClip fireSound;
        [SerializeField] protected float exitDuration = 0.5f;


        private Animator animator;

        private bool isBeingReturned = false;
        protected virtual void Awake()
        {
            poolTag = "Projectile";
            hitBox = GetComponent<HitBox>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

            if (hitBox == null)
            {
                DebugLogger.LogError(DebugData.DebugType.Gameplay, "Projectile has no HitBox component");
            }
            if (rb == null)
            {
                DebugLogger.LogError(DebugData.DebugType.Gameplay, "Projectile has no Rigidbody2D component");
            }

            if (!IsValid())
            {
                DebugLogger.LogError(DebugData.DebugType.Gameplay, "Projectile is invalid");
                Destroy(gameObject);
            }

            spawnAudioSource = GetComponent<AudioSource>();
            if (spawnAudioSource == null)
            {
                DebugLogger.LogWarning(DebugData.DebugType.Audio, "Projectile has no AudioSource component");
            }
            //DebugLogger.Log(DebugData.DebugType.Gameplay, "Projectile is Valid");
            hitBox.OnHit += HandleHit;
        }
        public bool IsValid()
        {
            return hitBox != null && rb != null;
        }
        public void FixedUpdate()
        {
            remainingLifetime -= Time.deltaTime;
            if (remainingLifetime <= 0 && !isBeingReturned)
            {
                Expire();
                return;
            }
            else if(remainingLifetime >=0  && !isBeingReturned)
                rb.linearVelocity = direction * Mathf.MoveTowards(rb.linearVelocity.magnitude, targetVelocity, accelDecel * Time.deltaTime);


            //transform.position += transform.up * targetVelocity * Time.deltaTime;
        }
        public void Initialize(IEntity owner, float initialVelocity, Vector2 direction, Transform firePoint)
        {

            isBeingReturned = false;

            this.owner = owner;
            hitBox.SetOwner(owner);

            this.direction = direction.normalized;
            rb.linearVelocity = direction * initialVelocity;
            transform.position = firePoint.position;
            transform.rotation = firePoint.rotation;
            remainingLifetime = maxLifeTime;

            damageInfo = new DamageInfo(owner, damage, damageType);
            hitBox?.SetDamageInfo(damageInfo);

            spawnAudioSource?.PlayOneShot(fireSound);
            animator.SetBool("isFlying", true);

        }
        public void SetDamageInfo(DamageInfo damageInfo)
        {
            hitBox.SetDamageInfo(damageInfo);
            this.damageInfo = damageInfo;
        }

        public void HandleHit(Collider2D other, DamageInfo damage)
        {
            if(isBeingReturned) return;
            //cant hit self with projectile
            if (other.TryGetComponent(out IEntity entity))
            {
                if (entity == owner) return;
            }
            //see if damagable
            if (other.TryGetComponent(out IDamagable damagable))
            {
                ////if projectile, dont hit self
                if (damagable is BaseProjectile projectile && projectile.owner == owner) return;
                DebugLogger.Log(DebugData.DebugType.Gameplay, $"Projectile hit {other.name}");
                damagable.TakeDamage(damage);
                Expire();

            }

        }

        private void Expire()
        {
            if (isBeingReturned) return;

            rb.linearVelocity = Vector2.zero;
            if (spawnAudioSource != null && hitSound != null)
            {
                float clipLength = hitSound.length;
                float adjustedPitch = clipLength / exitDuration;
                spawnAudioSource.pitch = adjustedPitch;
                spawnAudioSource?.PlayOneShot(hitSound);

            }
            animator.SetBool("isFlying", false);
            //wait for hit duration
            //set animation speed to hit duration
            animator.speed = animator.GetCurrentAnimatorStateInfo(0).length / exitDuration;

            StartCoroutine(ExpireAfterDelay());
        }

        private IEnumerator ExpireAfterDelay()
        {
            isBeingReturned = true;
            yield return new WaitForSeconds(exitDuration);
            if (spawnAudioSource != null) spawnAudioSource.pitch = 1f;
            ReturnToPool();
        }
        protected void ReturnToPool()
        {
            animator.SetBool("isFlying", false);

            isBeingReturned = true;
            ProjectileManager.Instance?.ReleaseProjectile(this);
            rb.linearVelocity = Vector2.zero;
        }

    }
}

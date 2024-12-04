using Assets.Scripts.Managers;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Misc;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Objects.Pickups;
using Assets.Scripts.Objects.Turrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using Assets.Scripts.Scriptable_Objects;
using Assets.Scripts.Objects;

namespace Assets.Scripts.Ships
{
    public enum Faction
    {
        Player,
        Neutral,
        Enemy
    }


    public class BaseShip : BaseEntity, IDamagable, IEntity
    {
        [Serializable]
        public class Movement
        {
            public float forwardAccel = 10f;
            public float backwardAccel = 5f;
            public float strafeAccel = 5f;
            public float decel = 10f;

            public float rotationAccel = 5f;
            public float rotationDecel = 10f;

            public float maxFowardVelocity = 30f;
            public float maxBackwardVelocity = 10f;
            public float maxStrafeVelocity = 10f;
            public float maxAngularVelocity = 10f;


        }


        [SerializeField] protected AudioClip engineAudioClip;
        public Movement movement;
        public Faction faction;
        protected List<BaseTurret> turrets = new List<BaseTurret>();
        protected TurretManager turretManager;

        public float pickupRadius = 5f;
        private float rotationInput = 0f;
        private Vector2 movementInput = Vector2.zero;
        private bool isMining = false, isFiring = false;
        public Vector2 Velocity { get => rb.linearVelocity; }
        public float Rotation { get => rb.angularVelocity; }

        public override void Awake()
        {

            base.Awake();
            turretManager = GetComponentInChildren<TurretManager>();

            if (inventory == null)
            {
                gameObject.AddComponent<Inventory>();
            }
        }

        public virtual void Update()
        {

        }
        public virtual void FixedUpdate()
        {
            ApplyDecceleration();
            ApplyAcceleration();
            if (turretManager != null )
            {
                FireTurrets();
                MineTurrets();
            }
            TryPickupNearbyItems();

        }



        public virtual void ApplyDecceleration()
        {
            // Decelerate linear velocity when no input
            if (movementInput == Vector2.zero)
            {
                animator?.SetBool("isMoving", false);

                if (audioSource.isPlaying)
                    audioSource.Stop();
                if (rb.linearVelocity.magnitude > 0.1f)
                {
                    rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, Vector2.zero, movement.decel * Time.deltaTime);

                }
                else
                {
                    rb.linearVelocity = Vector2.zero;
                }

            }

            // Decelerate angular velocity when no rotation input
            if (rotationInput == 0 && Mathf.Abs(rb.angularVelocity) > 0.1f)
            {
                if (Mathf.Abs(rb.angularVelocity) > 0.1f)
                    rb.angularVelocity = Mathf.MoveTowards(rb.angularVelocity, 0, movement.rotationDecel * Time.deltaTime);
                else
                    rb.angularVelocity = 0;
            }


        }
        public virtual void ApplyAcceleration()
        {
            //Vector2 targetVelocity = Vector2.zero;
            //Vector2 forwardVector = transform.up * movementInput.y;
            //Vector2 strafeVector = transform.right * movementInput.x;

            //// Forward and backward movement
            //if (movementInput.y != 0)
            //{
            //    float targetSpeed = movementInput.y > 0 ? movement.maxFowardVelocity : movement.maxBackwardVelocity;
            //    float accel = movementInput.y > 0 ? movement.forwardAccel : movement.backwardAccel;
            //    float newSpeed = Mathf.MoveTowards(rb.linearVelocity.magnitude, targetSpeed, accel * Time.deltaTime);
            //    targetVelocity += forwardVector.normalized * newSpeed;
            //}

            //// Strafing movement
            //if (movementInput.x != 0)
            //{
            //    float newSpeed = Mathf.MoveTowards(rb.linearVelocity.magnitude, movement.maxStrafeVelocity, movement.strafeAccel * Time.deltaTime);
            //    targetVelocity += strafeVector.normalized * newSpeed;
            //}

            //if (movementInput != Vector2.zero)
            //{
            //    if (!audioSource.isPlaying)
            //    {
            //        audioSource.resource = engineAudioClip;
            //        audioSource.Play();
            //    }
            //    var t= animator.GetBool("isMoving");
            //    animator?.SetBool("isMoving", true);

            //    rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, ;
            //}

            Vector2 targetVelocity = Vector2.zero;

            // Forward and backward movement
            if (movementInput.y != 0)
            {
                float targetSpeed = movementInput.y > 0 ? movement.maxFowardVelocity : movement.maxBackwardVelocity;
                float accel = movementInput.y > 0 ? movement.forwardAccel : movement.backwardAccel;

                // Calculate the desired velocity in the forward/backward direction
                Vector2 forwardVector = transform.up * movementInput.y;
                targetVelocity += forwardVector.normalized * targetSpeed;
            }

            // Strafing movement
            if (movementInput.x != 0)
            {
                float strafeAccel = movement.strafeAccel;
                float targetStrafeSpeed = movement.maxStrafeVelocity;

                // Calculate the desired velocity in the strafing direction
                Vector2 strafeVector = transform.right * movementInput.x;
                targetVelocity += strafeVector.normalized * targetStrafeSpeed;
            }

            // Smoothly interpolate current velocity toward the target velocity
            Vector2 smoothedVelocity = Vector2.MoveTowards(rb.linearVelocity, targetVelocity, movement.forwardAccel * Time.deltaTime);

            // Apply the smoothed velocity to the Rigidbody2D
            rb.linearVelocity = smoothedVelocity;

            // Handle animations and audio
            if (movementInput != Vector2.zero)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = engineAudioClip;
                    audioSource.Play();
                }

                animator?.SetBool("isMoving", true);
            }

            // Handle rotation
            if (rotationInput != 0)
            {

                float targetAngularVelocity = Mathf.MoveTowards(rb.angularVelocity, rotationInput * movement.maxAngularVelocity, movement.rotationAccel * Time.deltaTime);
                rb.angularVelocity = targetAngularVelocity;
            }
        }

        public virtual void FireTurrets()
        {
            if (isFiring)
            {
                turretManager.FireTurrets(turret => turret is ProjectileTurret);
            }
            else
            {
                turretManager.StopFiringTurrets(turret => turret is ProjectileTurret);
            }

        }
        public virtual void MineTurrets()
        {
            if (isMining)
            {
                turretManager.FireTurrets(turret => turret is BeamTurret);
            }
            else
            {
                turretManager.StopFiringTurrets(turret => turret is BeamTurret);
            }
        }


        #region Actions
        public virtual void MoveAction(Vector2 direction)
        {
            movementInput.y = direction.y;
        }
        public virtual void StrafeAction(float direction)
        {
            movementInput.x = direction;
        }
        public virtual void RotateAction(float direction)
        {
            rotationInput = direction;
        }

        public virtual void AttackAction(bool action)
        {

            isFiring = action;
            if (action)
                DebugLogger.Log(DebugData.DebugType.Controls, "Attacking");
            else
                DebugLogger.Log(DebugData.DebugType.Controls, "Stopped Attacking");
        }
        public virtual void SpecialAction(bool action)
        {
            isMining = action;
            if (action)
                DebugLogger.Log(DebugData.DebugType.Controls, "Mining");
            else
                DebugLogger.Log(DebugData.DebugType.Controls, "Stopped Mining");
        }

        public virtual void SpaceAction()
        {

        }

        #endregion


        protected void TryPickupNearbyItems()
        {
            Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, pickupRadius);
            foreach (Collider2D collider in nearbyObjects)
            {
                BasePickup pickup = collider.gameObject.GetComponent<BasePickup>();
                if (pickup != null)
                {
                    pickup.TryPickup(transform);
                }
            }

        }
        #region IDamagable

        #endregion
    }
}

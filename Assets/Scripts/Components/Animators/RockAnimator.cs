using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Animators
{
    public class RockAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AnimationClip defaultIdleClip;
        [SerializeField] private AnimationClip defaultHitClip;
        [SerializeField] private AnimationClip defaultDeathClip;


        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            if (animator == null)
            {
                Debug.LogError("Animator is not set in RockAnimator");
            }
        }
        private void Start()
        {
            if (animator == null)
            {
                Debug.LogError("Animator or default clips are not set in RockAnimator");
                return;
            }
            AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

            if (defaultIdleClip != null)
                overrideController["Fly"] = defaultIdleClip;
            if (defaultHitClip != null)
                overrideController["Hit"] = defaultHitClip;
            if (defaultDeathClip != null)
                overrideController["Death"] = defaultDeathClip;


            // Assign the override controller back to the Animator
            animator.runtimeAnimatorController = overrideController;
        }

        public void SetAnimationClips(AnimationClip idleClip, AnimationClip hitClip, AnimationClip deathClip)
        {
            // Clone the base AnimatorController if not already done
            AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

            // Replace the clips
            overrideController["Idle"] = idleClip ? idleClip : defaultIdleClip; // Replace "Fly" state clip
            overrideController["Hit"] = hitClip ? hitClip : defaultHitClip; // Replace "Hit" state clip
            overrideController["Death"] = deathClip ? deathClip : defaultHitClip; // Replace "Hit" state clip

            // Assign the override controller back to the Animator
            animator.runtimeAnimatorController = overrideController;
        }
    }
}

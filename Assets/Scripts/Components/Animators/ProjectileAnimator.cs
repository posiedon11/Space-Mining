using Assets.Scripts.Misc;
using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class ProjectileAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AnimationClip defaultFlyClip;
        [SerializeField] private AnimationClip defaultHitClip;

        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            if (animator == null)
            {
                DebugLogger.LogError(DebugData.DebugType.Default,"Animator is not set in ProjectileAnimator");
            }
        }
        private void Start()
        {
            if(animator == null)
            {
                DebugLogger.LogError(DebugData.DebugType.Default, "Animator or default clips are not set in ProjectileAnimator");
                return;
            }
            AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            if(defaultFlyClip != null)
                overrideController["Fly"] = defaultFlyClip;
            if(defaultHitClip != null)
                 overrideController["Hit"] = defaultHitClip;

            // Assign the override controller back to the Animator
            animator.runtimeAnimatorController = overrideController;
        }

        public void SetAnimationClips(AnimationClip flyClip, AnimationClip hitClip)
        {
            // Clone the base AnimatorController if not already done
            AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

            // Replace the clips
            overrideController["Fly"] = flyClip ? flyClip : defaultFlyClip; // Replace "Fly" state clip
            overrideController["Hit"] = hitClip ? hitClip : defaultHitClip; // Replace "Hit" state clip

            // Assign the override controller back to the Animator
            animator.runtimeAnimatorController = overrideController;
        }
    }
}

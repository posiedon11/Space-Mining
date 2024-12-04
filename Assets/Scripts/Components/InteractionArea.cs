using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;
using Assets.Scripts.Misc;
using Assets.Scripts.Scriptable_Objects;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Managers
{
    public class InteractionArea: BaseArea
    {
        [SerializeField] public UnityEvent onEnter;
        [SerializeField] public UnityEvent onExit;

        [SerializeField] private List<string> validTags = new List<string>();

        protected Collider2D collider1;

        private IInteractable interactable;
        protected override void Awake()
        {
            base.Awake();
            interactable = GetComponent<IInteractable>();
            if(interactable == null)
            {
                DebugLogger.LogWarning(DebugData.DebugType.General, $"No IInteractable component found on {gameObject.name}" );
            }
            var colliders = GetComponents<Collider2D>();

            foreach (var collider in colliders)
            {
                Destroy(collider);
            }
            switch (shape)
            {
                case AreaShape.Box:
                    var boxCollider = gameObject.AddComponent<BoxCollider2D>();
                    boxCollider.size = size;
                    break;
                case AreaShape.Circle:
                    var circleCollider = gameObject.AddComponent<CircleCollider2D>();
                    circleCollider.radius = radius;
                    break;
            }
            collider1 = GetComponent<Collider2D>();
            collider1.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (validTags.Contains(collision.tag))
            {
                DebugLogger.Log(DebugData.DebugType.Gameplay, $"{collision.tag} Entered {name}");
                InteractionManager.Instance?.SetCurrentInteractionArea(this);
                InteractionManager.Instance?.SetInteractable(interactable);
                onEnter.Invoke();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (validTags.Contains(collision.tag))
            {
                DebugLogger.Log(DebugData.DebugType.Gameplay, $"{collision.tag} Exited {name}");
                InteractionManager.Instance?.ClearCurrentInteraction(this);

                onExit.Invoke();
            }
        }
    }
}

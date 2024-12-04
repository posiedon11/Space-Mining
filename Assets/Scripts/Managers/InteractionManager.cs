using Assets.Scripts.Interfaces;
using Assets.Scripts.Misc;
using Assets.Scripts.Misc.Menus;
using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class InteractionManager: MonoBehaviour
    {
        public static InteractionManager Instance { get; private set; }
        public InteractionArea currentInteractionArea;
        public IInteractable currentInteractable;
        private PlayerUI playerUI;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            playerUI = FindFirstObjectByType<PlayerUI>();
        }


        public void SetInteractable(IInteractable interactable)
        {
            currentInteractable = interactable;
        }
        public void ClearInteractable(IInteractable interactable)
        {
            if(currentInteractable == interactable)
            {
                currentInteractable = null;
            }
        }
        public void PerformInteraction()
        {
            if(currentInteractable != null)
            {
                currentInteractable.Interact();
            }
        }
        public void SetCurrentInteractionArea(InteractionArea interactionArea)
        {
            currentInteractionArea = interactionArea;
            playerUI?.ShowInteraction(true);
        }
        public void ClearCurrentInteraction(InteractionArea area)
        {
            if(currentInteractionArea == area)
            {
                currentInteractionArea = null;
                playerUI?.ShowInteraction(false);
            }
        }

    }
}

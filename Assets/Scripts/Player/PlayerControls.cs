using Assets.Scripts.Managers;
using Assets.Scripts.Objects.Ships.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerControls : MonoBehaviour
    {

        public PlayerShip playerShip;
        private bool isShifting = false;
        public bool canInteract = false;
        private void Awake()
        {
            var rb = GetComponent<Rigidbody2D>();
            playerShip = GetComponent<PlayerShip>();
            if (playerShip == null)
            {
                Debug.LogError("PlayerShip component is missing");
            }
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            if (ctx.performed || ctx.canceled)
            {
                Vector2 direction = ctx.ReadValue<Vector2>();
                playerShip.MoveAction(direction);

            }
        }
        public void OnShiftModifier(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                isShifting = true;
            }
            else if ( ctx.canceled)
            {
                isShifting = false;
            }
        }
        public void OnRotateOrStrafe(InputAction.CallbackContext ctx)
        {
          
            if (ctx.performed || ctx.canceled)
            {
                float input = ctx.ReadValue<float>();
                if (isShifting)
                {
                    //Debug.Log("Strafing");
                    playerShip.StrafeAction(input);
                }
                else
                {
                    //Debug.Log("Rotating");
                    playerShip.RotateAction(input);
                }
            }
        }
        public void OnRotate(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                float rotation = ctx.ReadValue<float>();
                playerShip.RotateAction(rotation);
            }
        }
        public void OnAttack(InputAction.CallbackContext ctx)
        {
            if(ctx.performed)
            {
                playerShip.AttackAction(true);
            }
            else if(ctx.canceled)
            {
                playerShip.AttackAction(false);
            }
        }

        public void OnSpecial(InputAction.CallbackContext ctx)
        {
            if(ctx.performed)
            {
                playerShip.SpecialAction(true);
            }
            else if(ctx.canceled)
            {
                playerShip.SpecialAction(false);
            }
        }
        public void OnSpace(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                playerShip.SpaceAction();
            }

        }
        public void OnInteract(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && canInteract)
            {
                playerShip.InteractAction();
                InteractionManager.Instance?.PerformInteraction();
            }
        }


        public void SetInteract(bool enable)
        {
            canInteract = enable;
        }
    }
}

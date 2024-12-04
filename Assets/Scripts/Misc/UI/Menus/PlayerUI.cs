using Assets.Scripts.Managers;
using Assets.Scripts.Misc.UI;
using Assets.Scripts.Objects.Ships.Player;
using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Misc.Menus
{
    public class PlayerUI : BaseUI
    {
        private InventoryUI inventoryUI;
        private PauseMenu pauseMenu;
        private SettingsMenu settingsMenu;
        private PlayerHud playerHud;


        [SerializeField] private PlayerShip playerShip;

        protected override void Awake()
        {
            base.Awake();
            if(playerShip == null)
                playerShip = GetComponentInParent<PlayerShip>();
            if(playerShip == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "PlayerShip component is missing");
                return;
            }
            SetOwner(playerShip.gameObject);
            var t = GetComponentInChildren<InventoryUI>(true);
            var t2 = transform.Find("Inventory").gameObject.GetComponent<InventoryUI>();
            inventoryUI = GetComponentInChildren<InventoryUI>(true);
            pauseMenu = GetComponentInChildren<PauseMenu>(true);
            settingsMenu = GetComponentInChildren<SettingsMenu>(true);
            playerHud = GetComponentInChildren<PlayerHud>(true);

            if(inventoryUI == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI,"InventoryUI component is missing");
            }

            if (pauseMenu == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "PauseMenu component is missing");
            }
            if (settingsMenu == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "SettingsMenu component is missing");
            }
            if (playerHud == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "HudManager component is missing");
            }
        }

        public void Start()
        {
            inventoryUI?.Initialize(owner);
            pauseMenu?.Initialize(owner);
            settingsMenu?.Initialize(owner);
            playerHud?.Initialize(owner);
        }

        public void ToggleInventory()
        {
            if (inventoryUI.isActive)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
        public void OpenInventory()
        {
            playerShip.enabled = false;
            inventoryUI.Open();
            playerHud.Close();
        }
        public void CloseInventory()
        {
            playerShip.enabled = true;
            inventoryUI.Close();
            playerHud.Open();
        }


        public void TogglePauseMenu()
        {
            if (pauseMenu.isActive)
            {
                ClosePauseMenu();
            }
            else
            {
                OpenPauseMenu();
            }
        }
        public void OpenPauseMenu()
        {
            //playerShip.enabled = false;
            GameManager.Instance.PauseGame();
            pauseMenu.Open();
            playerHud.Close();
            inventoryUI.Close();
            settingsMenu.Close();
        }
        public void ClosePauseMenu()
        {
            GameManager.Instance.ResumeGame();
            //playerShip.enabled = true;
            playerHud.Open();
            pauseMenu.Close();
            //settingsMenu.Close();

        }

        public void ToggleSettingsMenu()
        {
            if (settingsMenu.isActive)
            {
                CloseSettingsMenu();
            }
            else
            {
                OpenSettingsMenu();
            }
        }
        public void OpenSettingsMenu()
        {
            pauseMenu.Close();
            settingsMenu.Open();
        }
        public void CloseSettingsMenu()
        {
            settingsMenu.Close();
            
            pauseMenu.Open();
        }

        public void ToggleHud()
        {
            if(playerHud.isActive)
            {
                CloseHud();
            }
            else
            {
                OpenHud();
            }
        }
        public void OpenHud()
        {
            playerHud.Open();
        }
        public void CloseHud()
        {
            playerHud.Close();
        }


        public void ToggleShop()
        {
            if (inventoryUI.isActive)
            {
                CloseShop();
            }
            else
            {
                OpenShop();
            }
        }
        public void OpenShop()
        {

        }
        public void CloseShop()
        {

        }


        public void ShowInteraction(bool show)
        {
            if(show)
                playerHud.SetDefaultInteractionPrompt();
            else
                playerHud.ClearInteractPrompt();
        }
        public void HideInteraction()
        {

        }
    }
}

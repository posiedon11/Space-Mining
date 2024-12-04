using Assets.Scripts.Misc.Menus;
using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Misc.UI.Menus
{
    public class MainMenuUI : BaseUI
    {
        private SettingsMenu settingsMenu;
        private MainMenu mainMenu;

        protected override void Awake()
        {
            base.Awake();
            settingsMenu = GetComponentInChildren<SettingsMenu>(true);
            mainMenu = GetComponentInChildren<MainMenu>(true);

            if (settingsMenu == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "SettingsMenu component is missing");
                return;
            }

            if (mainMenu == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "MainMenu component is missing");
                return;
            }

            
        }


        private void Start()
        {
            settingsMenu?.LoadSettings();
            settingsMenu?.Close();
        }

        public void ToggleSettings()
        {
            if(settingsMenu.isActive)
            {
                CloseSettings();
            }
            else
            {
                OpenSettings();
            }
        }
        public void OpenSettings()
        {
            settingsMenu.Open();
            mainMenu.Close();
        }
        public void CloseSettings()
        {
            settingsMenu.Close();
            mainMenu.Open();
        }

        public void ToggleMainMenu()
        {
            if (mainMenu.isActive)
            {
                CloseMainMenu();
            }
            else
            {
                OpenMainMenu();
            }
        }
        public void OpenMainMenu()
        {
            mainMenu.Open();
            settingsMenu.Close();
        }
        public void CloseMainMenu()
        {
            mainMenu.Close();
            settingsMenu.Open();
        }
    }
}

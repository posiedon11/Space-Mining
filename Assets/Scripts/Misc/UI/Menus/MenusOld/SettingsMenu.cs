using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Menus
{
    public class SettingsMenu : BaseMenu
    {
        private Button backButton;
        private Button applyButton;
        private Slider volumeSlider;
        private DropdownMenu resolutionDropdown;
        private DropdownMenu windowModeDropdown;

        private BaseMenu previousMenu;


        protected override void Awake()
        {
            base.Awake();
            
        }
        public override void ShowMenu()
        {
            base.ShowMenu();
            backButton = _root.Q<Button>("BackButton");
            backButton.clicked += OnBackButtonClicked;
        }

        public void LoadMenu(BaseMenu caller)
        {
            previousMenu = caller;
            ShowMenu();
        }

        private void OnBackButtonClicked()
        {
            HideMenu();
            var uiManager = FindFirstObjectByType<UIManager>();
            previousMenu.ShowMenu();
        }
    }
}

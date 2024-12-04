///https://www.youtube.com/watch?v=_jtj73lu2Ko
///this is what helped me
///
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Menus
{
    public class MainMenu : BaseMenu
    {

        private Button startButton;
        private Button quitButton;
        private Button settingsButton;

        private List<Button> _menuButtons = new List<Button>();


        private SettingsMenu settingsMenuEvents;


        protected override void  Awake()
        {
            base.Awake();
           // base.ShowMenu();
        }

        public override void ShowMenu()
        {
            base.ShowMenu();
            settingsMenuEvents = GetComponent<SettingsMenu>();

            startButton = _document.rootVisualElement.Q<Button>("StartButton");
            quitButton = _document.rootVisualElement.Q<Button>("ExitButton");
            settingsButton = _document.rootVisualElement.Q<Button>("SettingsButton");


            startButton.RegisterCallback<ClickEvent>(OnStartButtonClicked);
            quitButton.RegisterCallback<ClickEvent>(OnQuitButtonClicked);
            settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClicked);

            _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
            foreach (var button in _menuButtons)
            {
                button.RegisterCallback<ClickEvent>(OnButtonClicked);
            }
        }
        private void OnStartButtonClicked(ClickEvent evt)
        {
            Debug.Log("Start button clicked");
        }

        private void OnQuitButtonClicked(ClickEvent evt)
        {
            Debug.Log("Quit button clicked");
            Application.Quit();
        }

        private void OnButtonClicked(ClickEvent evt)
        {
            Debug.Log("Button clicked");
        }

        private void OnSettingsButtonClicked(ClickEvent evt)
        {
            HideMenu();
            settingsMenuEvents.LoadMenu(this);
        }
    }
}

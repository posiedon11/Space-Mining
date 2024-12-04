using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Menus
{
    public class UIManager: MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        public VisualTreeAsset mainMenuAsset;
        public VisualTreeAsset settingsMenuAsset;
        public VisualTreeAsset pauseMenuAsset;


        private VisualElement currentMenu;
        private VisualElement previousMenu;

        private UIDocument uiDocument;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject); // Prevent duplicate instances
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scene


            uiDocument = GetComponent<UIDocument>();
            ShowMenu(mainMenuAsset);
        }
        public void ShowMenu(VisualTreeAsset menuAsset)
        {
            Debug.Log("Showing menu");
            if (currentMenu != null)
            {
                uiDocument.rootVisualElement.Clear(); // Clear the old menu
            }
            previousMenu = currentMenu;

            currentMenu = menuAsset.CloneTree();
            uiDocument.rootVisualElement.Add(currentMenu);

            // Notify the new menu script to initialize its events
            var menuScript = GetComponent<IMenuEvents>();
            menuScript?.OnMenuLoaded(currentMenu);
        }

        public void ShowPrevious()
        {
            if (previousMenu != null)
            {
                uiDocument.rootVisualElement.Clear(); // Clear the old menu
                currentMenu = previousMenu;
                uiDocument.rootVisualElement.Add(currentMenu);

                // Notify the new menu script to initialize its events
                var menuScript = GetComponent<IMenuEvents>();
                menuScript?.OnMenuLoaded(currentMenu);
            }
        }
        public interface IMenuEvents
        {
            void OnMenuLoaded(VisualElement menuRoot);
        }
    }
}

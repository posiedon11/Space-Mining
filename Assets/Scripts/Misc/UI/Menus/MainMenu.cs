//Mainmenu.cs
//the games main menu
using Assets.Scripts;
using Assets.Scripts.Misc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.Scriptable_Objects;
using Assets.Scripts.Misc.UI;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Misc.Menus
{
    public class MainMenu : BaseUI
    {

        public void StartGame()
        {
            DebugLogger.Log(DebugData.DebugType.UI, "Game is starting");
            GameManager.Instance.NewGame();
            //LoadScene("IntroScene");
        }

        public void ContinueGame()
        {
            DebugLogger.Log(DebugData.DebugType.UI, "Game is continuing");
            GameManager.Instance.ContinueGame();

        }
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
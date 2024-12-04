using Assets.Scripts.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.Scriptable_Objects;
using Assets.Scripts.Misc.UI;
using Assets.Scripts.Managers;

namespace Assets.Scripts.Misc.Menus
{
    public class PauseMenu : BaseUI
    { 
        public void Start()
        {
        }
        public void LoadScene(string sceneName = "")
        {
            Time.timeScale = 1;

            DebugLogger.Log(DebugData.DebugType.UI, $"{sceneName} is loading");

            SceneManager.LoadScene(sceneName);
        }
        public void QuitGame()
        {
            Application.Quit();
        }
        public override void Open()
        {
            base.Open();
            //GameManager.Instance.PauseGame();
            //PauseGame();
        }
        public override void Close()
        {
            base.Close();
            //GameManager.Instance.ContinueGame();

            //ContinueGame();
        }
    }
}

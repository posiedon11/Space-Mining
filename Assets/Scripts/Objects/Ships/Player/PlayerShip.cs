using Assets.Scripts.Managers;
using Assets.Scripts.Misc;
using Assets.Scripts.Scriptable_Objects;
using Assets.Scripts.Ships;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Objects.Ships.Player
{
    public class PlayerShip : BaseShip
    {
        private string saveFilePath;
        
        public PlayerShip() 
        {
        }

        public virtual void InteractAction()
        {
            // Interact with objects
        }
        public override void Awake()
        {
            base.Awake();
            saveFilePath = $"{Application.persistentDataPath}/saveData.json";
        }

        public void SaveGame()
        {
            //DebugLogger.Log(DebugData.DebugType.Other, "Saving game");
            GameManager.Instance.SaveGame();
        }
       
    }
}

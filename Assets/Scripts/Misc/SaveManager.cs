using Assets.Scripts.Objects.Ships.Player;
using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Misc
{
    [Serializable]
    public class SaveData
    {
        public PlayerSaveData playerData;
    }
    public class SaveManager
    {
        public static SaveManager Instance { get; private set; }
        public PlayerSaveData playerSaveData = new PlayerSaveData();
        public SaveData saveData = new SaveData();
        private string saveFilePath;

        public PlayerShip playerShip;

        public SaveManager()
        {

            saveFilePath = $"{Application.persistentDataPath}/saveData.json";
            saveData = new SaveData();
            saveData.playerData = new PlayerSaveData();

        }
        public void SaveGame()
        {
            DebugLogger.Log(DebugData.DebugType.Other, "Saving game");
            SavePlayerData();
            string jsonData = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(saveFilePath, jsonData);
        }

        public SaveData LoadGame()
        {
            DebugLogger.Log(DebugData.DebugType.Other, "Loading game");

            if(!LoadSaveData()) return null;

            saveData.playerData = LoadPlayerData();

            return saveData;
        }
        private void SavePlayerData()
        {
            if (playerShip == null) return;

            DebugLogger.Log(DebugData.DebugType.Other, "Saving player data");
            playerSaveData = new PlayerSaveData();

            playerSaveData.lastScene = SceneManager.GetActiveScene().name;

            saveData.playerData = playerSaveData;
            playerShip.inventory.SaveInventory(playerSaveData.inventorySaveData);



        }

        public bool LoadSaveData()
        {
            DebugLogger.Log(DebugData.DebugType.Other, "Loading save data");
            if (HasSaveFile())
            {
                string jsonData = File.ReadAllText(saveFilePath);
                saveData = JsonUtility.FromJson<SaveData>(jsonData);
                if (saveData == null)
                {
                    DebugLogger.LogError(DebugData.DebugType.Other, "Save data is null");
                    return false;
                }
                return true;
            }
            return false;
        }
        public PlayerSaveData LoadPlayerData()
        {
            DebugLogger.Log(DebugData.DebugType.Other, "Loading player data");
            if (saveData != null)
            {
                if (playerShip != null)
                {

                    playerShip.inventory.LoadInventory(saveData.playerData.inventorySaveData);
                }

                return saveData.playerData;

            }
            return null;
        }

        public bool HasSaveFile()
        {
            return File.Exists(saveFilePath);
        }
        public void ResetSave()
        {
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }
            playerSaveData = new PlayerSaveData();
        }
    }
}

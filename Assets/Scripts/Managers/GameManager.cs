using Assets.Scripts.Interfaces;
using Assets.Scripts.Misc;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Objects.Ships.Player;
using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public List<IEntity> entities = new List<IEntity>();

        public PlayerShip playerShip;

        public PlayerShip shipPrefab;
        public SaveManager saveManager;
        private SaveData saveData;

        private bool isPaused = false;
        private bool loadGame = false;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                saveManager = new SaveManager();
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        public void Start()
        {
            entities = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IEntity>().ToList();
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            //entities = FindObjectsOfType<MonoBehaviour>().OfType<IEntity>().ToList();
        }

        public void QuitGame()
        {
            Application.Quit();
        }
        public void LoadScene(string sceneName = "")
        {
            Debug.Log($"{sceneName} is loading");
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        public void LoadSceneAsync(string sceneName = "")
        {
            Debug.Log($"{sceneName} is loading");
            SceneManager.LoadSceneAsync(saveData.playerData.lastScene);
        }

        public void PauseGame()
        {
            if (isPaused)
            {
                return;
            }
            isPaused = true;
            DebugLogger.Log(DebugData.DebugType.Gameplay, "Game is pausing");

            Time.timeScale = 0;
            FindEntenties();
            entities.ForEach(entities => entities.enabled = false);
        }
        public void ResumeGame()
        {
            if (!isPaused)
            {
                return;
            }
            isPaused = false;
            FindEntenties();
            DebugLogger.Log(DebugData.DebugType.Gameplay, "Game is continuing");
            Time.timeScale = 1;
            entities.ForEach(entities => entities.enabled = true);
        }


        private void GetPlayer()
        {
            if (playerShip == null)
            {
                GameObject playerObject = GameObject.FindWithTag("Player");
                if (playerObject != null)
                {
                    playerShip = playerObject.GetComponent<PlayerShip>();
                }
            }
            saveManager.playerShip = playerShip;

        }


        public void SetLoadGame(bool load)
        {
            loadGame = load;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            entities = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IEntity>().ToList();

            if (scene.name == "MainMenu")
            {
                Destroy(playerShip.gameObject);
                return;
            }

            SpawnOrAssignMainPlayer();
            if (loadGame && saveData != null)
            {
                //LoadGame();
                loadGame = false;
                playerShip.inventory.LoadInventory(saveData.playerData.inventorySaveData);
            }

        }


        public void NewGame(string firstScene = "IntroScene")
        {
            ResetGame();
            LoadScene(firstScene);

        }
        public void ContinueGame()
        {
            loadGame = true;
            LoadGame();
        }


        public void SaveGame()
        {
            GetPlayer();
            saveManager.SaveGame();
        }
        public void LoadGame()
        {
            GetPlayer();
            var loadedData = saveManager.LoadGame();
            if (loadedData != null)
            {
                if(loadedData.playerData == null || loadedData.playerData.lastScene == null || loadedData.playerData.lastScene == "" || loadedData.playerData.lastScene ==  "MainMenu")
                {
                    DebugLogger.LogWarning(DebugData.DebugType.Gameplay, "No last scene found in save data");
                    LoadScene();
                }
                loadGame = true;
                saveData = loadedData;
                //LoadScene(saveData.playerData.lastScene);
                LoadScene(saveData.playerData.lastScene);
            }
            else
            {
                loadGame = false;
                NewGame();
            }
        }
    


    private void SpawnOrAssignMainPlayer()
    {
        if (playerShip == null)
        {
            //playerShip = Instantiate(shipPrefab);
            var sceneShip = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<PlayerShip>().FirstOrDefault();
            if (sceneShip != null)
            {
                playerShip = sceneShip;
            }
            else
            {
                playerShip = Instantiate(shipPrefab);
            }
            saveManager.playerShip = playerShip;
            DontDestroyOnLoad(playerShip.gameObject);
        }
        else
        {
            foreach (var ship in GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<PlayerShip>())
            {
                if (ship != playerShip)
                {
                    Destroy(ship.gameObject);
                }
            }
        }
        Camera.main.GetComponent<CameraFollow>().SetTarget(playerShip.transform);

    }

    private void PositionPlayerAtSpawnPoint(string spawnString = "DefaultSpawn")
    {
        var spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        if (spawnPoints != null && playerShip != null)
        {
            var spawnPoint = spawnPoints.FirstOrDefault(spawnPoint => spawnPoint.name == spawnString);
            if (spawnPoint != null)
            {
                playerShip.transform.position = spawnPoint.transform.position;
            }
            else
                playerShip.transform.position = Vector3.zero;
        }
        else
        {
            Debug.LogWarning("Spawn point not found or player character not set in GameManager");
        }
    }

    private void FindEntenties()
    {
        entities = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IEntity>().ToList();
    }
    public void ResetGame()
    {
        foreach (var scenePlayer in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<PlayerShip>())
        {
            Destroy(scenePlayer.gameObject);
        }
    }
}
}

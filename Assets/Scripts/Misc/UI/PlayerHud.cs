using Assets.Scripts.Misc;
using Assets.Scripts.Objects.Ships.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Scriptable_Objects;
using TMPro;
using Assets.Scripts.Misc.UI;
namespace Assets.Scripts.Managers
{
    public class StatusBar
    {
        private GameObject parent;

        private Slider slider;
        private TextMeshProUGUI text;
        private float minValue, maxValue;

        public StatusBar(GameObject barPrefab)
        {
            parent = barPrefab;
            slider = parent.GetComponent<Slider>();
            text = parent.GetComponentInChildren<TextMeshProUGUI>();
            if (slider == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "StatusBar: Slider component not found on prefab");
                return;
            }
            if (text == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "StatusBar: Text component not found on prefab");
                return;
            }
            slider.minValue = 0;
            slider.maxValue = 1;
            slider.value = 1;
        }
        public void Initialize(float minValue, float maxValue, float currentValue)
        {
            if (slider == null) return;
            this.minValue = minValue;
            this.maxValue = maxValue;
            //slider.value = Mathf.InverseLerp(minValue, maxValue, currentValue);
            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.value = currentValue;
        }
        public void UpdateValue(float currentValue)
        {
            if (slider == null) return;
            //slider.value = Mathf.InverseLerp(minValue, maxValue, currentValue);
            slider.value = currentValue;
        }
        public void UpdateText()
        {
            if (text == null) return;
            text.text = $"{slider.value} : {slider.maxValue}";
        }
    }
    public class PlayerHud : BaseUI
    {
        [SerializeField] private GameObject healthBarObject, shieldBarObject;
        private StatusBar healthBar, shieldBar;
        public TextMeshProUGUI interactPrompt;
        private PlayerShip playerShip;

        protected override void Awake()
        {
            //playerShip = GetComponentInParent<PlayerShip>();
            if (playerShip == null)
            {
                playerShip = Tools.FindTopLevelEntity<PlayerShip>(transform);
                if (playerShip == null)
                {
                    DebugLogger.LogError(DebugData.DebugType.UI, "PlayerShip is null");
                }
            }

            interactPrompt = transform.Find("InteractPrompt").GetComponent<TextMeshProUGUI>();
            if (interactPrompt == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "InteractPrompt is null");
            }

        }
        private void Start()
        {
            SetHealthBars();
        }
        public void FixedUpdate()
        {
            if (playerShip == null)
            {
                //DebugLogger.LogError(DebugData.DebugType.UI, "PlayerShip is null");
                return;
            }
            healthBar?.UpdateValue(playerShip.health.health.Current);
            healthBar?.UpdateText();

            shieldBar?.UpdateValue(playerShip.health.shield.Current);
            shieldBar?.UpdateText();
        }

        public override bool Initialize(GameObject owner)
        {
            if(!base.Initialize(owner))
            {
                return false;
            }
            playerShip = owner.GetComponent<PlayerShip>();
            SetHealthBars();
            return true;
        }

        private void SetHealthBars()
        {
            if(playerShip == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "PlayerShip is null");
                return;
            }
            healthBar = new StatusBar(healthBarObject);
            shieldBar = new StatusBar(shieldBarObject);

            healthBar.Initialize(0, playerShip.health.health.Max, playerShip.health.health.Current);
            shieldBar.Initialize(0, playerShip.health.shield.Max, playerShip.health.shield.Current);
        }


        public void SetDefaultInteractionPrompt()
        {
            SetInteractPrompt("Press E to interact");
        }
        public void SetInteractPrompt(string newPrompt)
        {
            interactPrompt.text = newPrompt;
            enabled = true;
        }

        public void ClearInteractPrompt()
        {
            interactPrompt.text = "";
            enabled = false;
        }
    }
}

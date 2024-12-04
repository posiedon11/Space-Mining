using Assets.Scripts.Misc.UI;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Scriptable_Objects;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Assets.Scripts.Components;
using Assets.Scripts.Objects.Ships.Player;


namespace Assets.Scripts.Misc.Menus
{
    public class InventoryUI : BaseUI
    {
        [SerializeField] private GameObject itemPanePrefab;
        [SerializeField] private GameObject InventoryGrid;

        private Transform itemGrid;
        [SerializeField] PlayerShip playerShip;
        [SerializeField] private Inventory inventory;
        private TextMeshProUGUI headerText;

        private List<ItemPane> itemPanes = new List<ItemPane>();
        protected override void Awake()
        {
            base.Awake();
            if (itemPanePrefab == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "Item Pane Prefab is null.");
            }
            if (InventoryGrid == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "Inventory Grid is null.");
            }

            if (owner == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "Player is null.");
            }
            itemGrid = InventoryGrid.transform;
            if (itemGrid == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "Item Grid is null.");
            }
            headerText = transform.Find("Header").GetComponent<TextMeshProUGUI>();
            if (headerText == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "Header Text is null.");
            }
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void  Start()
        {
            playerShip = Tools.FindTopLevelEntity<PlayerShip>(owner.transform);
            inventory = playerShip.GetComponent<Inventory>();
            if (inventory == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "Inventory is null.");
                return;
            }
            inventory.OnUpdate += Refresh;
            Refresh();
        }
        // Update is called once per frame
        void Update()
        {

        }

        private void OnItemClicked(BaseItem item)
        {
            DebugLogger.Log(DebugData.DebugType.UI, $"Item {item.Name} clicked.");
        }

        public override void Open()
        {
            base.Open();
            Refresh();
        }
        private void OnEnable()
        {
            
            Refresh();
        }
        public void getInventory()
        {

        }
        public override void Refresh()
        {
            if (inventory == null)
            {
                inventory = owner.GetComponent<Inventory>();
            }
            if (inventory == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "Inventory is null.");
                return;
            }
            base.Refresh();
            headerText.text = "Inventory";

            foreach (Transform child in InventoryGrid.transform)
            {
                Destroy(child.gameObject);
            }

            itemPanes.Clear();
            foreach (var item in inventory.InventoryList)
            {
                var pane = new ItemPane(itemPanePrefab, InventoryGrid.transform, item, defaultSprite);

                itemPanes.Add(pane);
            }
        }



    }
}

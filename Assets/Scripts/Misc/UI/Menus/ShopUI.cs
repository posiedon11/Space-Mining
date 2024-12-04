using Assets.Scripts.Interfaces;
using Assets.Scripts.Misc.Menus;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Objects.Ships.Player;
using Assets.Scripts.Objects.Stations;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Misc.UI.Menus
{
    [Serializable]
    public class ShopUI : BaseUI
    {
        [SerializeField] private BaseStation station;
        [SerializeField] private PlayerShip playerShip;

        [SerializeField] private Inventory playerInventory;
        [SerializeField] private Inventory merchantInventory;


        [SerializeField] private GameObject merchantInventoryGrid;
        [SerializeField] private GameObject playerInventoryGrid;

        [SerializeField] private TextMeshProUGUI merchantHeader;
        [SerializeField] private TextMeshProUGUI playerHeader;

        [SerializeField] private GameObject itemPanePrefab;

        [SerializeField] private List<ItemPane> stationItems = new List<ItemPane>();
        [SerializeField] private List<ItemPane> playerItems = new List<ItemPane>();

        [SerializeField] private PlayerUI playerUI;


        protected override void Awake()
        {
            base.Awake();
            if (itemPanePrefab == null)
            {
                Debug.LogError("Item Pane Prefab is missing");
            }
            if (station == null)
            {
                Debug.LogError("Station is missing");
            }
        }
        public void Start()
        {
            if (station == null)
            {
                Debug.LogError("Station is missing");
            }

            merchantInventory = station.GetComponent<Inventory>();
        }

        public void SetPlayerShip(PlayerShip playerShip)
        {
            this.playerShip = playerShip;
        }

        public override void Open()
        {
            base.Open();
            playerUI = playerShip.GetComponentInChildren<PlayerUI>();
            if(playerUI != null)
            {
                playerUI.CloseHud();
            }
            playerInventory = playerShip.inventory;
            merchantInventory = station.inventory;
            playerInventory.OnUpdate += Refresh;
            Refresh();

            //merchantInventory.OnUpdate += Refresh;

        }

        public override void Close()
        {
            base.Close();

            if (playerUI != null)
            {
                playerUI.OpenHud();
            }
        }

        public override void Refresh()
        {
            GetGrids();
            GetHeaders();

            PopulateLists();
            EnableSellable();
            SetHeaders();
        }
        protected virtual void GetGrids(string playerInventory = "PlayerInventory", string merchantInventory = "ShopInventory")
        {
            merchantInventoryGrid = transform.Find(merchantInventory).Find("InventoryGrid").gameObject;
            playerInventoryGrid = transform.Find(playerInventory).Find("InventoryGrid").gameObject;

            if(merchantInventoryGrid == null || playerInventoryGrid == null)
            {
                Debug.LogError("Merchant or Player Inventory Grid is missing");
            }
        }
        protected virtual void GetHeaders(string playerInventory = "PlayerInventory", string merchantInventory = "ShopInventory")
        {
            //var headerOBJPlayer = transform.Find(playerInventory).Find("Header").gameObject;
            //var headerOBJMerchant = transform.Find(merchantInventory).Find("Header").gameObject;

            playerHeader = transform.Find(playerInventory).Find("Header").gameObject.GetComponent<TextMeshProUGUI>(); ;
            merchantHeader = transform.Find(merchantInventory).Find("Header").gameObject.GetComponent<TextMeshProUGUI>(); ;
        }

        protected virtual void PopulateLists()
        {
            //clear the grids
            foreach(Transform child in merchantInventoryGrid.transform)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in playerInventoryGrid.transform)
            {
                Destroy(child.gameObject);
            }

            PopulateList(merchantInventory, playerInventory, stationItems, merchantInventoryGrid.transform);
            PopulateList(playerInventory, merchantInventory, playerItems, playerInventoryGrid.transform);
        }

        protected virtual void EnableSellable()
        {
            EnableSellableItems(playerItems);
            EnableSellableItems(stationItems);
       }

        private void EnableSellableItems(List<ItemPane> itemsPanes)
        {
            var sellableItems = itemsPanes.Where(x => x.item is ISellable).ToList();
            foreach(var sellable in sellableItems)
            {
                sellable.ToggleButton(true);
            }
        }


        private void SetHeaders()
        {
            if (playerHeader != null && playerInventory != null)
            {
                playerHeader.text = playerInventory.currencies.First(i=>i.currencyType == Components.Currency.CurrencyType.Gold).amount.ToString();
            }
            if (merchantHeader != null && merchantInventory != null)
            {
                merchantHeader.text = merchantInventory.currencies.First(i => i.currencyType == Components.Currency.CurrencyType.Gold).amount.ToString();
            }
        }
        private void PopulateList(Inventory ownerInventory,Inventory buyerInventory,  List<ItemPane> itemPanes, Transform owner)
        {
            itemPanes.Clear();
            foreach (var item in ownerInventory.InventoryList)
            {
                var pane = new ItemPane(itemPanePrefab, owner, item, defaultSprite);
                pane.SetInventories(ownerInventory, buyerInventory);
                itemPanes.Add(pane);
            }
        }

    }
}

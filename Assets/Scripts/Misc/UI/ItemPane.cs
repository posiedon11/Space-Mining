using Assets.Scripts.Interfaces;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Misc.UI
{
    public class ItemPane
    {
        public GameObject gameObject;
        public GameObject owner;
        public Image iconImage;

        public TextMeshProUGUI quantityText;
        public Button paneButton;

        public BaseItem item;

        private Inventory sellerInventory;  // The owners's inventory
        private Inventory buyerInventory;
        public ItemPane(GameObject panePrefab,Transform parent, BaseItem _item, Sprite defaultSprite, string _quantityText = "", bool enableButton = false)
        {
            gameObject = GameObject.Instantiate(panePrefab, parent);
            iconImage = gameObject.transform.Find("Icon").GetComponent<Image>();
            quantityText = gameObject.transform.Find("Quantity").GetComponent<TextMeshProUGUI>();
            paneButton = gameObject.GetComponent<Button>();

            if (!ValidPane())
            {
                GameObject.Destroy(gameObject);
            }

            item = _item;

            iconImage.sprite = item.Sprite != null ? item.Sprite : defaultSprite;
            iconImage.color = item.Color != null ? item.Color : Color.white;
            //iconImage.sprite = iconImage.sprite ? iconImage.sprite : defaultSprite;
            //iconImage.sprite = item.icon ? item.icon : iconImage.sprite;
            quantityText.text = _quantityText;
            quantityText.text = item.amount > 1 ? item.amount.ToString() : _quantityText;
            paneButton.interactable = enableButton;


            paneButton.onClick.AddListener(() => SellItem());

        }


        public void SetInventories(Inventory _ownerInventory, Inventory _buyerInventory)
        {
            sellerInventory = _ownerInventory;
            buyerInventory = _buyerInventory;
        }


        public void SellItem()
        {
            if (sellerInventory == null || buyerInventory == null)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, "Owner or Buyer inventory is null");
                return;
            }
            if(item is ISellable sellable)
            {
                var sellablePrice = sellable.GetSellPrice();
                buyerInventory?.TransferCurriency(sellable.Currency, false);
                buyerInventory?.TransferItem(item);

                sellerInventory?.TransferCurriency(sellable.Currency, true);
                sellerInventory?.TransferItem(item);
            }
        }
        public bool ValidPane()
        {
            bool valid = true;
            string erroreMessage = "";
            if (gameObject == null)
            {
                erroreMessage+= "GameObject is null\n";
                valid = false;
            }
            if (iconImage == null)
            {
                erroreMessage += "IconImage is null\n";
                valid = false;
            }
            if (quantityText == null)
            {
                erroreMessage += "QuantityText is null\n";
                valid = false;
            }
            if (paneButton == null)
            {
                erroreMessage += "PaneButton is null\n";
                valid = false;
            }

            if (!valid)
            {
                DebugLogger.LogError(DebugData.DebugType.UI, erroreMessage);
            }

            return valid;
        }

        public void ToggleButton(bool enable)
        {
            paneButton.interactable = enable;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Scriptable_Objects;
using Assets.Scripts.Misc;
using Assets.Scripts.Components;
using Assets.Scripts.Misc.Menus;
using System.IO;
using Assets.Scripts.Scriptable_Objects.Items;
using Newtonsoft.Json;

namespace Assets.Scripts.Objects.Items
{
    public class Inventory : MonoBehaviour
    {
        public delegate void UpdateInventoryEvent();
        public event UpdateInventoryEvent OnUpdate;

        private List<BaseItem> inventoryList = new List<BaseItem>();
        [SerializeField] private int inventorySize = 10;

        [SerializeField] public List<Currency> currencies = new List<Currency>();

        private string inventorySavePath;

        private JsonSerializerSettings saveSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects
        };

        private void Awake()
        {
            inventorySavePath = Application.persistentDataPath + "/inventory.json";
            //LoadInventory();
        }


        public List<BaseItem> InventoryList { get => inventoryList; }
        public void AddItem(BaseItem item)
        {
            int remaingingInventorySlots = inventorySize - inventoryList.Count;
            if (item == null)
            {
                DebugLogger.LogWarning(DebugData.DebugType.Inventory, "Item is null.");

            }
            var existingItem = inventoryList.FindAll(existingItem => existingItem.ID == item.ID).Where(e => e.amount < e.MaxStackSize);
            foreach (var existing in existingItem)
            {
                var mergedItems = existing.Merge(item);
                if (mergedItems.Count < remaingingInventorySlots)
                {
                    inventoryList.Remove(existing);
                    inventoryList.AddRange(mergedItems);
                }
                else
                {
                    int itemsToAdd = remaingingInventorySlots;
                    if (itemsToAdd > 0)
                        inventoryList.Remove(existing);
                    inventoryList.AddRange(mergedItems.GetRange(0, itemsToAdd));
                }
                DebugLogger.Log(DebugData.DebugType.Inventory, $"Merged {item.Name} with existing item into {mergedItems.Count()} stacks.");
                OnUpdate?.Invoke();

                return;
            }

            if (remaingingInventorySlots <= 0)
            {
                DebugLogger.LogWarning(DebugData.DebugType.Inventory, "Inventory is full.");
                return;
            }

            var splitItems = item.SplitIntoStacks();
            inventoryList.AddRange(splitItems);


            OnUpdate?.Invoke();
            DebugLogger.Log(DebugData.DebugType.Inventory, $"Added {item.Name} to inventory.");
        }

        public void DropInventory()
        {

        }
        public void RemoveItem(BaseItem item)
        {
            if (inventoryList.Contains(item))
            {
                inventoryList.Remove(item);
                DebugLogger.Log(DebugData.DebugType.Inventory, $"Removed {item.Name} to inventory.");
                OnUpdate?.Invoke();

            }
        }



        public void TransferCurriency(Currency currency, bool add = true)
        {
            var ownCurrencie = currencies.Find(c => c.currencyType == currency.currencyType);
            if (ownCurrencie == null)
            {
                //ebugLogger.LogError(DebugData.DebugType.UI, "Currency is null.");
                if (add)
                    currencies.Add(currency);
                else
                {
                    currencies.Add(new Currency(currency, false));
                }
            }
            else
            {
                if (add)
                {
                    ownCurrencie.Add(currency.amount);
                }
                else
                {
                    ownCurrencie.Subtract(currency.amount);
                }
            }
        }


        public void TransferItem(BaseItem item)
        {
            if (item == null) return;
            var ownItem = InventoryList.Find(i => i == item);
            if (ownItem == null)
            {
                DebugLogger.Log(DebugData.DebugType.Inventory, $"Adding Item to {name}.");
                AddItem(item);
                return;
            }
            else
            {
                DebugLogger.Log(DebugData.DebugType.Inventory, $"Removing Item from {name}.");
                RemoveItem(item);
            }

        }

        [ContextMenu("Display Inventory")]
        public void DisplayInventory()
        {
            DebugLogger.Log(DebugData.DebugType.Inventory, "Inventory:");
            foreach (var item in inventoryList)
            {
                Debug.Log($"- {item.Name}, {item.amount}");
            }
        }

        [ContextMenu("Display Currencies")]
        public void DisplayCurrencies()
        {
            var displayString = "Currencies: \n";
            foreach (var currency in currencies)
            {
                displayString += $"{currency.Display()}\n";
            }
            DebugLogger.Log(DebugData.DebugType.Inventory, displayString);
        }



        public void SaveInventory(InventorySaveData saveData)
        {

            foreach (var item in inventoryList)
            {
                //var type = item.GetType().AssemblyQualifiedName;
                var itemData = new InventoryItemData(item.ID, item.amount, item.GetType().AssemblyQualifiedName);
                saveData.inventoryItems.Add(itemData);
            }
        }

        [ContextMenu("Save Inventory")]

        public void SaveInventoryToFile()
        {

            InventorySaveData saveData = new InventorySaveData();
            SaveInventory(saveData);


            string json = JsonUtility.ToJson(saveData);
            File.WriteAllText(inventorySavePath, json);
            //string json = JsonUtility.ToJson(saveData, true);
            //File.WriteAllText(inventorySavePath, json);
            DebugLogger.Log(DebugData.DebugType.Inventory, "Inventory saved.");
        }

        [ContextMenu("Load Inventory")]
        public void LoadInventory(InventorySaveData saveData)
        {
            inventoryList.Clear();
            foreach (var itemSaveData in saveData.inventoryItems)
            {
                var type = Type.GetType(itemSaveData.itemType);
                if (type == null)
                {
                    Debug.LogWarning($"Type {itemSaveData.itemType} not found.");
                    continue;
                }

                ItemData itemData = ItemDatabase.Instance.GetItemByID(itemSaveData.itemID);
                if (itemData == null)
                {
                    Debug.LogWarning($"Item with ID {itemSaveData.itemID} not found in database.");
                    continue;
                }
                //if type == BaseItem
                BaseItem item = (BaseItem)Activator.CreateInstance(type);

                item.amount = itemSaveData.amount;
                item.SetData(itemData);


                inventoryList.Add(item);
            }


            Debug.Log("Inventory loaded.");
        }
        public void LoadInventoryFromFile()
        {;

            if (!File.Exists(inventorySavePath))
            {
                Debug.LogWarning("Inventory file not found.");
                return;
            }

            string json = File.ReadAllText(inventorySavePath);
            InventorySaveData saveData = JsonConvert.DeserializeObject<InventorySaveData>(json);
            LoadInventory(saveData);

        }
    }
}

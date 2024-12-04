using Assets.Scripts.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Scriptable_Objects.Items
{
    public class ItemDatabase : MonoBehaviour
    {
        public static ItemDatabase Instance { get; private set; }
        [SerializeField] private string itemsFolder = "Items";
        [SerializeField] private Dictionary<int, ItemData> itemDictionary = new Dictionary<int, ItemData>();
        [SerializeField] private int itemIdStart = 10;

        [SerializeField] public int Count => itemDictionary.Count;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                LoadAllItems();
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            LoadAllItems();
            GenerateAllIDs();
        }
        [ContextMenu("Load All Items")]
        public void LoadAllItems()
        {
            ItemData[] loadedItems = Resources.LoadAll<ItemData>(itemsFolder);
            itemDictionary.Clear();
            foreach (ItemData item in loadedItems)
            {
                if (!itemDictionary.ContainsKey(item.id))
                    itemDictionary.Add(item.id, item);
                else
                {
                    DebugLogger.LogWarning(DebugData.DebugType.Gameplay, $"Item with ID {item.id} already exists in the database");
                }
            }

        }

        public ItemData GetItemByID(int id)
        {
            itemDictionary.TryGetValue(id, out ItemData item);
            return item;
        }

        public void GenerateID(ItemData item)
        {
            if (item.id <= itemIdStart)
            {
                item.id = itemIdStart + itemDictionary.Count();
                itemDictionary.Add(item.id, item);
            }
        }
        [ContextMenu("Generate All IDs")]
        private void GenerateAllIDs()
        {
            ItemData[] loadedItems = Resources.LoadAll<ItemData>(itemsFolder);

            foreach (var item in loadedItems)
            {
                GenerateID(item);
            }
        }
    }
}

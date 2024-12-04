using Assets.Scripts.Components;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Misc;
using Assets.Scripts.Scriptable_Objects;
using Assets.Scripts.Scriptable_Objects.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace Assets.Scripts.Objects.Items
{
    [Serializable]
    //[JsonObject(MemberSerialization.OptIn)]
    public abstract class BaseItem : ISellable
    {
        [SerializeField] protected ItemData itemData;

        [SerializeField] public int amount;
        [SerializeField] public AudioClip pickupSound;

        public Int32 ID { get => itemData != null ? itemData.id : 0; }
        public string Name { get => itemData != null ? itemData.itemName : null; }
        [JsonIgnore] public string Description { get => itemData != null ? itemData.description : null; }
        [JsonIgnore] public Sprite Sprite { get => itemData != null ? itemData.sprite : null; }
        [JsonIgnore] public Color Color { get => itemData != null ? itemData.color : Color.white; }

        [JsonIgnore] public Int32 MaxStackSize { get => itemData != null ? itemData.maxStackSize : 0; }

        [JsonIgnore] public Currency Currency { get => itemData != null ? itemData.currency : null; }
        [JsonIgnore] public bool IsSellable { get => itemData != null && itemData.isSellable; }
        [JsonIgnore] public bool IsStackable { get => itemData != null && itemData.isStackable; }
        public BaseItem()
        {
            itemData = null;
            amount = 0;
            pickupSound = null;
        }

        public BaseItem(ItemData _itemData)
        {
            if (_itemData == null)
            {
                DebugLogger.LogWarning(DebugData.DebugType.Inventory, "Item Data null in BaseItem");
                return;
            }

            this.itemData = _itemData;
        }

        public virtual void Awake()
        {
        }
        public List<BaseItem> Merge(BaseItem toMerge)
        {
            List<BaseItem> items = new List<BaseItem>();

            if (toMerge == null || toMerge.GetType().Name != this.GetType().Name)
            {
                DebugLogger.LogWarning(DebugData.DebugType.Inventory, "Cannot merge items of different types");
                return items;
            }
            int remainingAmount = amount + toMerge.amount;
            while (remainingAmount > MaxStackSize)
            {
                BaseItem newStack = CreateNewInstance();
                newStack.amount = MaxStackSize;
                items.Add(newStack);

                remainingAmount -= MaxStackSize;
            }
            this.amount = Mathf.Min(remainingAmount, MaxStackSize);
            items.Add(this);
            toMerge.amount = remainingAmount - this.amount;

            return items;
        }


        public List<BaseItem> SplitIntoStacks()
        {
            List<BaseItem> items = new List<BaseItem>();
            while (amount > MaxStackSize)
            {
                BaseItem newStack = CreateNewInstance();
                newStack.amount = MaxStackSize;
                amount -= MaxStackSize;
                items.Add(newStack);
            }
            items.Add(this);
            return items;
        }

        public abstract BaseItem CreateInstance(ItemData _itemData);
        protected abstract BaseItem CreateNewInstance();

        public virtual BaseItem Copy()
        {
            return CreateNewInstance();
        }


        public Currency GetSellPrice()
        {
            return itemData != null && IsSellable ? new Currency(Currency, Currency.amount * amount) : null;
        }

        public void SetData(ItemData data)
        {
            itemData = data;
            DebugLogger.Log(DebugData.DebugType.Inventory, "Item data set to " + itemData.itemName);
        }

        public void Save()
        {
            DebugLogger.Log(DebugData.DebugType.Inventory, "Item saved.");

        }

    }
}

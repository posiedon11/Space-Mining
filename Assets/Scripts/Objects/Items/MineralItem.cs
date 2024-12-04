using Assets.Scripts.Components;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Scriptable_Objects.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Objects.Items
{
    [Serializable]
    public class MineralItem : BaseItem, ISellable
    {
        public enum MineralType { Iron, Gold, Diamond }

        [SerializeField] public MineralType mineralType;


        //public MineralItem(string name, string description, int amount, MineralType mineralType) : base(name, description, amount)
        //{
        //    this.mineralType = mineralType;
        //}

        public MineralItem() : base()
            {
            }
        public MineralItem(ItemData _itemData, int amount = 0) : base (_itemData)
        {
            //this.mineralType = mineralType;
            if(_itemData is MineralData minData)
            {
                mineralType = minData.mineralType;
            }
            this.amount = amount;

        }

        protected override BaseItem CreateNewInstance()
        {
            var newMineral = new MineralItem(this.itemData, this.amount);
            //var newMineral = new MineralItem(this.name, this.description, this.amount, this.mineralType);
            //newMineral.sprite = this.sprite;
            //newMineral.color = this.color;
            //newMineral.currency = new Currency(this.currency, this.currency.amount);
            return newMineral;
        }

        public override BaseItem CreateInstance(ItemData _itemData)
        {
            return new MineralItem(_itemData, this.amount);
        }

    }
}

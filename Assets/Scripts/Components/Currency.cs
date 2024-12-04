using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components
{
    [Serializable]
    public class Currency
    {
        public enum CurrencyType { Gold, Silver, Bronze }

        [SerializeField] public CurrencyType currencyType;
        //how much its worth
        [SerializeField] public float amount;

        public Currency(float amount, CurrencyType currencyType)
        {
            this.amount = amount;
            this.currencyType = currencyType;
        }
        public Currency(Currency oldCurrency, bool sign = true)
        {
            this.currencyType = oldCurrency.currencyType;
            this.amount = sign ? oldCurrency.amount : -oldCurrency.amount;
        }
        public Currency(Currency oldCurrency, float amount)
        {
            this.currencyType = oldCurrency.currencyType;
            this.amount = amount;
        }

        public void Add(float amount)
        {
            this.amount += amount;
        }

        public void Subtract(float amount)
        {
            this.amount -= amount;
        }


        public string Display()
        {
            string currencyString = $"{currencyType}:  {amount}";
            return currencyString;
        }
    }
}

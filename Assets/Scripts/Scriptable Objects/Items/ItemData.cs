using Assets.Scripts.Components;
using Assets.Scripts.Objects.Items;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Scriptable_Objects.Items
{
    [Serializable]
    [CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/Items/Item")]

    public class ItemData : ScriptableObject
    {
        [HideInInspector] public Int32 id;
        [HideInInspector] public string itemName;
        [HideInInspector][TextArea] public string description;

        [HideInInspector] public Sprite sprite;
        [HideInInspector] public Color color;
        public Int32 maxStackSize = 32;

        public Currency currency;
        [HideInInspector] public bool isSellable;
        [HideInInspector] public bool isStackable;
        
    }
}

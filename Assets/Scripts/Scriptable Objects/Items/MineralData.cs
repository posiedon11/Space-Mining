using Assets.Scripts.Objects.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Scriptable_Objects.Items
{
    [Serializable]
    [CreateAssetMenu(fileName = "MineralData", menuName = "ScriptableObjects/Items/Mineral")]
    public class MineralData : ItemData
    {
        public MineralItem.MineralType mineralType;

    }
}

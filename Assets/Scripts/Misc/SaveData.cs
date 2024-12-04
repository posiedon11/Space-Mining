using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Misc
{

    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class InventoryItemData
    {
         public int itemID;
        public int amount;
         public string itemType;


        public InventoryItemData(int itemID, int amount, string type)
        {
            this.itemType = type;
            this.itemID = itemID;
            this.amount = amount;
        }
    }
    [Serializable]
    public class InventorySaveData
    {
        public List<InventoryItemData> inventoryItems = new List<InventoryItemData>();
    }

    [Serializable]
    public class PlayerSaveData
    {
        //public InventorySaveData inventorySaveData = new InventorySaveData();
        public string lastScene = "";
        public InventorySaveData inventorySaveData = new InventorySaveData();
    }
}

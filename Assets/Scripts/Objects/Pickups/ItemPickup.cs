using Assets.Scripts.Objects.Items;
using Assets.Scripts.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Scriptable_Objects;
using Assets.Scripts.Misc;
namespace Assets.Scripts.Objects.Pickups
{
    public class ItemPickup: BasePickup<BaseItem>
    {

        protected override void Awake()
        {
            base.Awake();

        }
        
        protected override void OnPickup(Transform player)
        {
           DebugLogger.Log(DebugData.DebugType.Inventory, $"Picked up {item.amount} {item.Name}.");

            Inventory inventory = player.GetComponent<Inventory>();
            if(inventory != null)
            {

                //var item = new MineralItem(mineralType.ToString(),"A valuable item", amount, mineralType);
                inventory.AddItem(item.Copy());            }
            else
            {
                DebugLogger.LogError(DebugData.DebugType.Inventory, "Player inventory not found.");
            }
            // Add minerals to player
        }
    }
}

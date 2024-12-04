using Assets.Scripts.Misc;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Objects.Pickups
{
    public class MineralPickup : BasePickup<MineralItem>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnPickup(Transform player)
        {
            DebugLogger.Log(DebugData.DebugType.Inventory, $"Picked up {item.amount} {item.Name}.");

            Inventory inventory = player.GetComponent<Inventory>();
            if (inventory != null)
            {
                inventory.AddItem(item.Copy());
            }
            else
            {
                DebugLogger.LogError(DebugData.DebugType.Inventory, "Player inventory not found.");
            }
        }
    }
}

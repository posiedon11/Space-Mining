using Assets.Scripts.Interfaces;
using Assets.Scripts.Managers;
using Assets.Scripts.Misc;
using Assets.Scripts.Misc.UI.Menus;
using Assets.Scripts.Objects.Ships.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Objects.Stations
{
    public class BaseStation : BaseEntity, IInteractable
    {

        [SerializeField]public InteractionArea interactArea;
        [SerializeField] public ShopUI shopUI;

        public override void Awake()
        {
            base.Awake();
            interactArea = GetComponent<InteractionArea>();
            shopUI = GetComponentInChildren<ShopUI>(true);

        }

        protected virtual void PlayerEnter(PlayerShip player)
        {
            //interactArea.PlayerEnter(player);
        }
        public void Interact()
        {
            shopUI?.Open();
        }
    }
}

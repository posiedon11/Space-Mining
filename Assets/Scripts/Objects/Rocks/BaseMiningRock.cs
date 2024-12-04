using Assets.Scripts.Managers;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Misc;
using Assets.Scripts.Objects.Pickups;
using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Objects.Items;

namespace Assets.Scripts.Objects.Rocks
{

    public class BaseMiningRock: BaseRock, IDamagable, IEntity
    {
        public enum RockTier { Low, Medium, High }

        [SerializeField]private RockTier rockTier;
        [SerializeField] private RarityManager.Rarity rarity;
        [SerializeField]
        [Range(0, 1.0f)]
        private float rarityScaleMultiplier = 1.0f;
        [SerializeField]
        [Range(0, 1.0f)]
        private float tierScaleMultiplier;

        [SerializeField] private List<Sprite> tierSprites;

        [SerializeField] private GameObject mineralDropPrefab;
        private MineralPickup mineralPickup;

        [SerializeField] private int baseMineralAmount;
        [SerializeField] private bool randomTier = false, randomRarity = false;

        [SerializeField] protected bool dropMultipleMinerals = false;
        protected Int32 pickupID = 0;


        public override void Awake()
        {
            base.Awake();
            if(mineralDropPrefab == null)
            {
                DebugLogger.LogError(DebugData.DebugType.Gameplay, "Mineral Drop Prefab is missing");
                return;
            }
            mineralPickup = mineralDropPrefab.GetComponent<MineralPickup>();
            if (mineralPickup == null)
            {
                DebugLogger.LogError(DebugData.DebugType.Gameplay, "Mineral Pickup is missing");
            }
            mineralPickup.itemId = mineralPickup.itemData.id;

        }
        public virtual void Start()
        {
            spriteRenderer.sprite = tierSprites[(int)rockTier];
            var color = RarityManager.Instance.GetColorForRarity(rarity);
            spriteRenderer.color = color;
            //mineralPickup.InitializePickupManager(MineralPickupManager.Instance, mineralDropPrefab);
            PickupManager.Instance.AddPickupToPool(mineralDropPrefab);
            


        }
        public override void DeathEffects()
        {
            DropMinerals();

            base.DeathEffects();
        }
        public virtual void DropMinerals()
        {
            int mineralPrefabAmount = 0;
            BasePickup basePickup = PickupManager.Instance.GetPickup<MineralItem>(mineralDropPrefab);

            //inventory.DropInventory();
            if (dropMultipleMinerals)
            {
                for (int i = 0; i < baseMineralAmount * (int)rarity + 1; i++)
                {
                    //BasePickup<MineralItem> pickupItem = PickupManager.Instance.GetPickup<MineralItem>(mineralDropPrefab);
                    if (basePickup == null)
                    {
                        DebugLogger.LogError(DebugData.DebugType.Gameplay, "Mineral Pickup is null");
                        return;
                    }
                    mineralPrefabAmount += mineralPickup.amount;
                }
            }
            else
            {
                //var mineral = mineralPickup.pickupManager.GetPickup(mineralDropPrefab);
                //BasePickup<MineralItem> pickupItem = PickupManager.Instance.GetPickup<MineralItem>(mineralDropPrefab);

                if (basePickup == null)
                {
                    DebugLogger.LogError(DebugData.DebugType.Gameplay, "Mineral Pickup is null");
                    return;
                }
                mineralPrefabAmount += mineralPickup.amount;
            }
            string dropMessage = $"Dropped {baseMineralAmount} minerals, worth {mineralPrefabAmount}, for a total of {baseMineralAmount * mineralPrefabAmount}";
            basePickup?.Initialize(transform, baseMineralAmount * mineralPrefabAmount, mineralPickup.itemData);

            DebugLogger.Log(DebugData.DebugType.Gameplay, dropMessage);
        }

        public override void Initialize()
        {
            if(randomRarity)
            {
                rarity = (RarityManager.Rarity)UnityEngine.Random.Range(0, Enum.GetValues(typeof(RarityManager.Rarity)).Length);
            }
            if(randomTier)
            {
                rockTier = (RockTier)UnityEngine.Random.Range(0, Enum.GetValues(typeof(RockTier)).Length);
            }

            float rarityScaleValue = ((int)rarity+1)*rarityScaleMultiplier;
            float tierScaleValue = ((int)rockTier+1)*tierScaleMultiplier;
            base.Initialize();
            Vector3 rarityScale = new Vector3(rarityScaleValue, rarityScaleValue, 1);
            base.transform.localScale = rarityScale;

        }
    }
}

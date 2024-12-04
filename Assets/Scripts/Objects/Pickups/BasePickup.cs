using Assets.Scripts.Interfaces;
using Assets.Scripts.Managers;
using Assets.Scripts.Misc;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Scriptable_Objects;
using Assets.Scripts.Scriptable_Objects.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Objects.Pickups
{
    [Serializable]
    public abstract class BasePickup : MonoBehaviour, IPoolable
    {
        [SerializeField] protected float pickupRadius = 3f;
        [SerializeField] protected float maxLifeTime = 10f;
        [SerializeField] protected float remainingLifetime;

        protected bool isAlive = false;
        protected bool isBeingReturned = false;

        [SerializeField] public int amount;
        [SerializeField] public GameObject pickupPrefab { get; set; }
        [SerializeField] public string poolTag { get; set; }
        [SerializeField] protected SpriteRenderer spriteRenderer;


        public int itemId;
        [SerializeField] public ItemData itemData;


        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if(itemData == null)
            {
                DebugLogger.LogWarning(DebugData.DebugType.Gameplay, "itemdata null");
            }
            itemId = itemData.id;
            //icon =prefab.GetComponent<SpriteRenderer>().sprite;
        }
        protected abstract void OnPickup(Transform player);
        public virtual void TryPickup(Transform character)
        {
            float distance = Vector2.Distance(character.position, transform.position);
            if (distance < pickupRadius)
            {
                OnPickup(character);
                ReturnToPool();
            }
        }
        public virtual void Initialize(Transform location, int _ammount, ItemData _itemData)
        {
            itemData = _itemData;
            itemId = itemData.id;

            transform.position = location.position;
            transform.rotation = location.rotation;
            remainingLifetime = maxLifeTime;
            if(spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
            spriteRenderer.color = itemData.color;
            spriteRenderer.sprite = itemData.sprite;
            isAlive = true;
            isBeingReturned = false;
            amount = _ammount;

            if (itemData == null)
            {
                DebugLogger.LogError(DebugData.DebugType.Inventory, $"Item with ID {itemId} not found in database.");
            }
            else
            {
                spriteRenderer.sprite = itemData.sprite;
                spriteRenderer.color = itemData.color;
            }
        }
        public virtual void ReturnToPool()
        {
            if (isBeingReturned) return;
            isAlive = false;
            isBeingReturned = true;
            PickupManager.Instance.ReleasePickup(this);
        }


        public abstract void InitializeItemData();
        public bool sameDataType(ItemData data)
        {
            return itemData.GetType() == data.GetType();
        }
    }
    public abstract class BasePickup<TItem> : BasePickup where TItem:BaseItem
    {

        [SerializeField] public TItem item;
        //public PickupManager<TItem> pickupManager;
        protected override void Awake()
        {
            poolTag = "Pickup";


        }

        protected virtual void Start()
        {
            if (item == null)
            {
                DebugLogger.LogError(DebugData.DebugType.Inventory, "Item is null.");
            }
        }
        public virtual void Update()
        {
            

        }

        public virtual void FixedUpdate()
        {
            if(!isAlive)
            {
                return;
            }
            remainingLifetime -= Time.deltaTime;
            if (remainingLifetime <= 0)
            {
                ReturnToPool();
            }
        }
        public override void Initialize(Transform location, int amount, ItemData _itemData )
        {
            base.Initialize(location, amount, _itemData);
            item.amount = amount;
            //if(item == null)
            //{
            //    DebugLogger.LogError(DebugData.DebugType.Inventory, "Item is null.");
            //}
        }

        public override void InitializeItemData()
        {
            item.SetData(itemData);
        }
        //public void InitializePickupManager(PickupManager<TItem> manager, GameObject prefab)
        //{
        //    this.pickupManager = manager;
        //    pickupManager.AddPickupToPool(prefab);
        //    //manager.AddPickupToPool(prefab);
        //}
    }
}

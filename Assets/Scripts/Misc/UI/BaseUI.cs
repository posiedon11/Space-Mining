using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Misc.UI
{
    public abstract class BaseUI :MonoBehaviour
    {
        public GameObject owner;
        public bool isActive = false;
        public Sprite defaultSprite;


        protected virtual void Awake()
        {
            owner = gameObject;
            defaultSprite = Resources.Load<Sprite>("Sprites/DefaultSprite");
        }
        protected virtual void SetOwner(GameObject owner)
        {
            this.owner = owner;
        }
        public virtual void Open()
        {
            isActive = true;
            gameObject.SetActive(true);
        }
        public virtual void Close()
        {
            isActive = false;
            gameObject.SetActive(false);
        }
        public virtual void Toggle()
        {
            if (isActive)
            {
                Close();
            }
            else
            {
                Open();
            }
        }



        public virtual bool Initialize(GameObject owner)
        {
            if (owner == null)
            {
                Debug.LogError($"Owner for {GetType().Name} is null");
                gameObject.SetActive(false);
                return false ;
            }
            SetOwner(owner);
            return true;
        }

        public virtual void Refresh()
        {
            //Debug.Log("Refresh");
        }
    }
}

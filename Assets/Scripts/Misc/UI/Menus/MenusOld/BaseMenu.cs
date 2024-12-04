using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Menus
{
    public class BaseMenu: MonoBehaviour
    {
        [SerializeField] protected VisualTreeAsset _menuAsset;
        protected UIDocument _document { get; private set; }
        protected VisualElement _root { get; private set; }
        public bool LoadOnStart = false;

        protected virtual void Awake()
        {
            if (_menuAsset == null)
            {
                Debug.LogError("Menu asset is not set");
                return;
            }

            _document = GetComponent<UIDocument>();
            if(_document == null)
            {
                Debug.LogError("UIDocument is not set");
                return;
            }

            if(LoadOnStart)
            {
                ShowMenu();
            }
            //_document.visualTreeAsset = _menuAsset;
            //_root = _document.rootVisualElement;
        }

        public virtual void ShowMenu()
        {
            //gameObject.SetActive(true);
            Debug.Log($"Showing menu: {this.GetType().Name}");
            _document.rootVisualElement.Clear();
            _root = _menuAsset.CloneTree();
            _document.rootVisualElement.Add(_root);
        }

        public virtual void HideMenu()
        {
            Debug.Log($"Hiding menu: {this.GetType().Name}");
            _document.rootVisualElement.Clear();

            //gameObject.SetActive(false);
        }

    }
}

using Assets.Scripts.Scriptable_Objects.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using UnityEngine;

namespace Assets.MyEditor.Items
{

    [CustomEditor(typeof(ItemData))]
    public class ItemDataEditor : Editor
    {
        ItemData itemData;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            itemData = (ItemData)target;
            itemData.id = EditorGUILayout.IntField("ID", itemData.id);
            itemData.itemName = EditorGUILayout.TextField("Item Name", itemData.itemName);
            itemData.description = EditorGUILayout.TextArea(itemData.description, GUILayout.Height(60));


            itemData.isSellable = EditorGUILayout.Toggle("Is Sellable", itemData.isSellable);
            itemData.isStackable = EditorGUILayout.Toggle("Is Stackable", itemData.isStackable);

            // Sprite field
            itemData.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", itemData.sprite, typeof(Sprite), false);

            // Color field
            itemData.color = EditorGUILayout.ColorField("Color", itemData.color);

            // Preview the sprite and color overlay
            if (itemData.sprite != null)
            {
                GUILayout.Label("Sprite Preview", EditorStyles.boldLabel);
                Rect spriteRect = GUILayoutUtility.GetRect(128, 128, GUILayout.ExpandWidth(false));

                // Draw the sprite background (color overlay)
                EditorGUI.DrawRect(spriteRect, itemData.color);

                // Draw the sprite

                Texture2D spriteTexture = itemData.sprite.texture;

                Rect textureCoords = new Rect(
                    itemData.sprite.textureRect.x / spriteTexture.width,
                    itemData.sprite.textureRect.y / spriteTexture.height,
                    itemData.sprite.textureRect.width / spriteTexture.width,
                    itemData.sprite.textureRect.height / spriteTexture.height);

                GUI.DrawTextureWithTexCoords(spriteRect, spriteTexture, textureCoords);
            }

            // Save changes
            if (GUI.changed)
            {
                EditorUtility.SetDirty(itemData);
            }
            if (GUILayout.Button("Assign Unique ID"))
            {
                itemData.id = GenerateUniqueID();
                EditorUtility.SetDirty(itemData);
            }

        }
        private int GenerateUniqueID()
        {
            ItemDatabase.Instance.GenerateID(itemData);
            return 1;
        }

    }
    [CustomEditor(typeof(MineralData))]
    public class MineralDataEditor : ItemDataEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            MineralData mineralData = (MineralData)target;
        }
    }


}

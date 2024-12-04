using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.Scriptable_Objects
{
    [Serializable]
    [CreateAssetMenu(fileName = "DebugOptions", menuName = "ScriptableObjects/Debug")]
    public class DebugData : ScriptableObject
    {
        public enum DebugType { Controls, Gameplay, Audio, Graphics, AI, UI, Other, Turrets, Player, Inventory, Damage, HitBox, Default, General, Pools }

        [Header("Global Debug Mode")]
        public bool debugMode = false;

        [Header("Debug Categories")]
        public bool debugControls = false;
        public bool debugGameplay = false;
        public bool debugPlayer = false;
        public bool debugAudio = false;
        public bool debugGraphics = false;
        public bool debugTurrets = false;
        public bool debugAI = false;
        public bool debugUI = false;
        public bool debugOther = false;
        public bool debugInventory = false;
        public bool debugDamage = false;
        public bool debugHitBoxes = false;
        public bool debugDefault = false;
        public bool debugGeneral = false;
        public bool debugPools = false;
        public bool CanDebug(DebugType type)
        {
            if (!debugMode) return false;

            return type switch
            {
                DebugType.Controls => debugControls,
                DebugType.Gameplay => debugGameplay,
                DebugType.Audio => debugAudio,
                DebugType.Graphics => debugGraphics,
                DebugType.AI => debugAI,
                DebugType.UI => debugUI,
                DebugType.Other => debugOther,
                DebugType.Turrets => debugTurrets,
                DebugType.Player => debugPlayer,
                DebugType.Inventory => debugInventory,
                DebugType.Damage => debugDamage,
                DebugType.HitBox => debugHitBoxes,
                DebugType.Default => debugDefault,
                DebugType.General => debugGeneral,
                DebugType.Pools => debugPools,
                _ => false,
            };
        }


        [ContextMenu("Set all to False")]
        public void SetAllToFalse()
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(bool))
                {
                    field.SetValue(this, false);
                }
            }
        }
        [ContextMenu("Set all to True")]
        public void SetAllToTrue()
        {
            FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(bool))
                {
                    field.SetValue(this, true);
                }
            }
        }


    }
}

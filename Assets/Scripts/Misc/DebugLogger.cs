using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Misc
{
    public class DebugLogger
    {
        
        private static DebugData debugData;
        public static void Initialize(DebugData data)
        {
            debugData = data;
        }

        public static void Log( DebugData.DebugType type = DebugData.DebugType.Default, string message = "")
        {
            if (debugData!=null &&  debugData.CanDebug(type))
            {
                Debug.Log($"{type.ToString()}: {message}");
            }
        }
        public static void LogWarning(DebugData.DebugType type = DebugData.DebugType.Default, string message = "")
        {
            if (debugData != null && debugData.CanDebug(type))
            {
                Debug.LogWarning($"{type.ToString()}: {message}");
            }
        }

        public static void LogError(DebugData.DebugType type = DebugData.DebugType.Default, string message = "")
        {
            if (debugData != null && debugData.CanDebug(type))
            {
                Debug.LogError($"{type.ToString()}: {message}");
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitializeDebugLogger()
        {
            // Load your DebugData ScriptableObject
            DebugData debugData = Resources.Load<DebugData>("DebugOptions");
            if (debugData != null)
            {
                DebugLogger.Initialize(debugData);
                Debug.Log("DebugLogger initialized with DebugData.");
            }
            else
            {
                Debug.LogWarning("DebugData ScriptableObject not found in Resources folder.");
            }
        }

    }
}

using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;

namespace Assets.MyEditor.Spawnables
{

    [CustomEditor(typeof(SpawnableData))]
    public class SpawnableDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SpawnableData data = (SpawnableData)target;

            if (data.prefab == null)
            {
                EditorGUILayout.HelpBox("Prefab is not assigned.", MessageType.Error);
            }
            else if (!data.IsSpawnable())
            {
                EditorGUILayout.HelpBox("Prefab does not implement ISpawnable.", MessageType.Error);
            }
            else
            {
                EditorGUILayout.HelpBox("Prefab is valid and implements ISpawnable.", MessageType.Info);
            }
        }
    }
}

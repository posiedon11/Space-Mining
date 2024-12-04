using Assets.Scripts.Misc;
using Assets.Scripts.Scriptable_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Scriptable_Objects
{

    [CreateAssetMenu(fileName = "RarityManager", menuName = "ScriptableObjects/RarityManager")]
    public class RarityManager : ScriptableObject
    {
        public enum Rarity { Common, Uncommon, Rare, Epic, Legendary }
        private static RarityManager instance;
        public static RarityManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = Resources.Load<RarityManager>("RarityManager");
                    if (instance == null)
                    {
                        DebugLogger.LogError(DebugData.DebugType.Gameplay,"Could not find RarityManager in Resources folder.");
                    }
                    else
                    {
                        DebugLogger.Log(DebugData.DebugType.Gameplay, "RarityManager loaded successfully.");
                    }
                }
                return instance;
            }
        }


        [Serializable]
        public class RarityData
        {
            public Rarity rarity;
            public Color color;
        }

        public List<RarityData> rarityColors = new List<RarityData>();

        private void OnValidate()
        {
            int enumLength = Enum.GetValues(typeof(Rarity)).Length;

            // Ensure the list matches the number of rarities
            while (rarityColors.Count < enumLength)
            {
                Rarity rarity = (Rarity)(rarityColors.Count); // Assign rarity based on index
                rarityColors.Add(new RarityData { rarity = rarity, color = Color.white });
            }

            while (rarityColors.Count > enumLength)
            {
                rarityColors.RemoveAt(rarityColors.Count - 1); // Remove excess entries
            }

            // Ensure all rarities are in the list in the correct order
            for (int i = 0; i < rarityColors.Count; i++)
            {
                rarityColors[i].rarity = (Rarity)i;
            }
        }

        public Color GetColorForRarity(Rarity rarity)
        {
            DebugLogger.Log(DebugData.DebugType.Other, "Getting color for rarity: " + rarity);
            //Debug.Log("Getting color for rarity: " + rarity);   
            foreach (var data in rarityColors)
            {
                if (data.rarity == rarity)
                    return data.color;
            }

            return Color.white; // Default color if not found
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using PlateformSurvivor.Save;
using ScriptableObject;
using UnityEngine;

namespace PlateformSurvivor.Menu
{
    public class PersistentDataManager : MonoBehaviour
    {
        [HideInInspector] public StatObject chosenCharacter;
        public readonly Dictionary<string, int> statsUpgrade = new(){{"Health",0}};
        public Dictionary<string, bool> charactersUnlocked = new(){{"Classic",true},{"Speedy",true}};
        public Dictionary<string, bool> stagesUnlocked = new(){{"Stage1",true},{"Stage2",true}};
        public int coins;
        private List<PlayerData> data = new(){new PlayerData()};

        public bool savePersistentData = true;

        private void Awake()
        {
            if (savePersistentData)
            {
                SaveDataManager.LoadJsonData(data);
                coins = data[0].coins;
                foreach (var stage in data[0].stagesUnlocked)
                {
                    stagesUnlocked[stage] = true;
                }

                foreach (var character in data[0].charactersUnlocked)
                {
                    charactersUnlocked[character] = true;
                }

                foreach (var upgrade in data[0].passiveBought)
                {
                    statsUpgrade[upgrade.passive] = upgrade.level;
                }
            }
            else
            {
                coins = 100; 
            }

            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationQuit()
        {
            if (savePersistentData)
            {
                data.Clear();
                PlayerData playerData = new();
                data.Add(playerData);

                data[0].coins = coins;
            
                foreach (var upgrade in statsUpgrade)
                {
                    data[0].passiveBought.Add(new PassiveBought(upgrade.Key, upgrade.Value));
                }

                foreach (var character in charactersUnlocked)
                {
                    if (character.Value)
                    {
                        data[0].charactersUnlocked.Add(character.Key);
                    }
                }
            
                foreach (var stage in stagesUnlocked)
                {
                    if (stage.Value)
                    {
                        data[0].stagesUnlocked.Add(stage.Key);
                    }
                }
            
                SaveDataManager.SaveJsonData(data);
            }
        }

        public bool HasUpgraded()
        {
            return statsUpgrade.Any(pair => pair.Value > 0);
        }
    }
}

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
        public Dictionary<string, int> statsUpgrade = new(){{"Health",0},{"Speed",0},{"JumpForce",0},{"Strength",0}};
        public Dictionary<string, bool> charactersUnlocked = new(){{"Classic",true},{"Speedy",true}};
        public Dictionary<string, bool> stagesUnlocked = new(){{"Stage1",true},{"Stage2",true}};
        public List<string> activeAbilitiesUnlocked = new() { "Dash", "DoubleJump", "Fireball"};
        public List<string> passiveAbilitiesUnlocked = new() { "Speed", "JumpForce"};
        public int coins;
        private List<PlayerData> data = new(){new PlayerData()};

        public bool savePersistentData = true;

        private void Awake()
        {
            if (savePersistentData)
            {
                LoadPersistentData();
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
                SavePersistentData();
            }
        }

        private void LoadPersistentData()
        {
            SaveDataManager.LoadJsonData(data);
            coins = data[0].coins;
            stagesUnlocked = data[0].stagesUnlocked.ToDictionary(s => s, s=>  true);
            charactersUnlocked = data[0].charactersUnlocked.ToDictionary(c => c, c=>  true);
            statsUpgrade = data[0].passiveBought.ToDictionary(p => p.passive, p=>p.level);
            ManageListData(data[0].activeAbilitiesUnlocked, activeAbilitiesUnlocked );
            ManageListData(data[0].passiveAbilitiesUnlocked, passiveAbilitiesUnlocked);
        }

        private void SavePersistentData()
        {
            data.Clear();
            PlayerData playerData = new();
            data.Add(playerData);

            data[0].coins = coins;
            data[0].passiveBought = statsUpgrade.Select(p => new PassiveBought(p.Key, p.Value)).ToList();
            data[0].charactersUnlocked = charactersUnlocked.Where(c => c.Value).Select(c => c.Key).ToList();
            data[0].stagesUnlocked = stagesUnlocked.Where(s => s.Value).Select(s => s.Key).ToList();
            ManageListData(activeAbilitiesUnlocked, data[0].activeAbilitiesUnlocked);
            ManageListData(passiveAbilitiesUnlocked, data[0].passiveAbilitiesUnlocked);

            SaveDataManager.SaveJsonData(data);
        }

        private void ManageListData(List<string> listInput, List<string> listOutput)
        {
            listOutput = listInput.Where(i => !listOutput.Contains(i)).ToList();
        }

        public bool HasUpgraded()
        {
            return statsUpgrade.Any(pair => pair.Value > 0);
        }
    }
}

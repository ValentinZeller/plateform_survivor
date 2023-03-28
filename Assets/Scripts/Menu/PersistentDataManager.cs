using System;
using System.Collections.Generic;
using System.Linq;
using PlateformSurvivor.Save;
using PlateformSurvivor.Service;
using ScriptableObject;
using Unity.VisualScripting;
using UnityEngine;

namespace PlateformSurvivor.Menu
{
    public enum AchievementKey {Healing, Killer1};
    
    public class PersistentDataManager : MonoBehaviour
    {
        [HideInInspector] public StatObject chosenCharacter;
        public Dictionary<string, int> statsUpgrade = new(){{"Health",0},{"Speed",0},{"JumpForce",0},{"Strength",0}};
        public Dictionary<string, bool> charactersUnlocked = new(){{"Classic",true},{"Speedy",true}};
        public Dictionary<string, bool> stagesUnlocked = new(){{"Stage1",true},{"Stage2",false}};
        public Dictionary<string, bool> achievementsUnlocked = new(){{"Healing", false},{"Killer1",false}};
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
                coins = 0;
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
            if (data[0].stagesUnlocked.Count == 0){ return; }
            stagesUnlocked.Where(s => data[0].stagesUnlocked.Contains(s.Key)).ToDictionary(s => s.Key, s => true);
            charactersUnlocked.Where(c => data[0].charactersUnlocked.Contains(c.Key)).ToDictionary(c => c.Key, c => true);
            statsUpgrade.Where(s => data[0].passiveBought.Count(p => p.passive == s.Key) > 0)
                .ToDictionary(s => s.Key, s => data[0].passiveBought.Where(p => p.passive == s.Key).Select(p => p.level));
            achievementsUnlocked.Where(a => data[0].achievementsUnlocked.Contains(a.Key)).ToDictionary(a => a.Key, a => true);
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
            data[0].achievementsUnlocked = achievementsUnlocked.Where(a => a.Value).Select(a => a.Key).ToList();
            ManageListData(activeAbilitiesUnlocked, data[0].activeAbilitiesUnlocked);
            ManageListData(passiveAbilitiesUnlocked, data[0].passiveAbilitiesUnlocked);

            SaveDataManager.SaveJsonData(data);
        }

        private void ManageListData(List<string> listInput, List<string> listOutput)
        {
            listOutput = listInput.Where(i => !listOutput.Contains(i)).ToList();
        }

        public bool HasAchievementUnlocked(string achName)
        {
            return achievementsUnlocked.ContainsKey(achName) && achievementsUnlocked[achName];
        }

        public void UnlockAchievement(AchievementKey key)
        {
            switch (key)
            {
                case AchievementKey.Healing:
                    passiveAbilitiesUnlocked.Add("Health");
                    break;
                case AchievementKey.Killer1:
                    passiveAbilitiesUnlocked.Add("Strength");
                    break;
            }

            achievementsUnlocked[key.ToString()] = true;
            Debug.Log(key.ToString());
        }

        public bool HasUpgraded()
        {
            return statsUpgrade.Any(pair => pair.Value > 0);
        }
    }
}

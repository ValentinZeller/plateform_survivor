using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlateformSurvivor.Save;
using ScriptableObject;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace PlateformSurvivor.Menu
{
    public enum AchievementKey {Healing, Killer1, SurviveStage1};
    
    public class PersistentDataManager : MonoBehaviour
    {
        [HideInInspector] public CharacterObject chosenCharacter;
        public StageObject chosenStage;
        public Dictionary<string, int> statsUpgrade = new();
        public List<string> charactersUnlocked = new(){"Classic","Speedy"};
        public List<string> charactersBought = new();
        public List<string> stagesUnlocked = new(){"Stage1"};
        public List<string> achievementsUnlocked = new();
        public List<string> activeAbilitiesUnlocked = new() { "Dash", "DoubleJump", "Fireball"};
        public List<string> passiveAbilitiesUnlocked = new() { "Speed", "JumpForce"};
        public int coins;
        private List<PlayerData> data = new(){new PlayerData()};

        public bool savePersistentData = true;

        private void Awake()
        {
            for (int i = 0; i < CharacterObject.Keys().Count; i++)
            {
                statsUpgrade.Add(CharacterObject.Keys()[i], 0);
            }
            if (savePersistentData)
            {
                LoadPersistentData();
                coins = 100; //for testing
            }

            DontDestroyOnLoad(gameObject);
        }

        private IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;

            LocalizationSettings.SelectedLocale =
                LocalizationSettings.AvailableLocales.Locales[PlayerPrefs.GetInt("LanguageIndex")];
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
            GamePreferencesManager.LoadPrefs();
            SaveDataManager.LoadJsonData(data);
            coins = data[0].coins;
            if (data[0].stagesUnlocked.Count == 0){ return; }

            for (int i = 0; i < data[0].passiveBought.Count ; i++)
            {
                statsUpgrade[data[0].passiveBought[i].passive] = data[0].passiveBought[i].level;
            }

            ManageListData(data[0].charactersUnlocked, charactersUnlocked);
            ManageListData(data[0].stagesUnlocked, stagesUnlocked);
            ManageListData(data[0].achievementsUnlocked, achievementsUnlocked);
            ManageListData(data[0].activeAbilitiesUnlocked, activeAbilitiesUnlocked );
            ManageListData(data[0].passiveAbilitiesUnlocked, passiveAbilitiesUnlocked);
            ManageListData(data[0].charactersBought, charactersBought);
        }

        private void SavePersistentData()
        {
            data.Clear();
            PlayerData playerData = new();
            data.Add(playerData);

            data[0].coins = coins;
            data[0].passiveBought = statsUpgrade.Select(p => new PassiveBought(p.Key, p.Value)).ToList();

            ManageListData(charactersUnlocked, data[0].charactersUnlocked);
            ManageListData(stagesUnlocked, data[0].stagesUnlocked);
            ManageListData(achievementsUnlocked, data[0].achievementsUnlocked);
            ManageListData(activeAbilitiesUnlocked, data[0].activeAbilitiesUnlocked);
            ManageListData(passiveAbilitiesUnlocked, data[0].passiveAbilitiesUnlocked);
            ManageListData(charactersBought, data[0].charactersBought);

            SaveDataManager.SaveJsonData(data);
            GamePreferencesManager.SavePrefs();
        }

        private void ManageListData(List<string> listInput, List<string> listOutput)
        {
            listOutput.AddRange(listInput.Where(i => !listOutput.Contains(i)).ToList());
        }

        public bool HasAchievementUnlocked(string achName)
        {
            return achievementsUnlocked.Contains(achName);
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
                case AchievementKey.SurviveStage1:
                    stagesUnlocked.Add("Stage2");
                    break;
            }

            achievementsUnlocked.Add(key.ToString());
        }

        public bool HasUpgraded()
        {
            return statsUpgrade.Any(pair => pair.Value > 0);
        }
    }
}

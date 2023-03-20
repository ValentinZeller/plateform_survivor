using System;
using System.Collections.Generic;

namespace PlateformSurvivor.Save
{

    [Serializable]
    public class PassiveBought
    {
        public string passive;
        public int level;

        public PassiveBought(string name, int value)
        {
            passive = name;
            level = value;
        }
    }
    
    [Serializable]
    public class PlayerData : ISaveable
    {
        public int coins;
        public List<string> stagesUnlocked = new();
        public List<string> charactersUnlocked = new();
        public List<string> charactersBought = new();
        public List<string> activeAbilitiesUnlocked = new();
        public List<string> passiveAbilitiesUnlocked = new();
        public List<string> achievementsUnlocked = new();
        public List<PassiveBought> passiveBought = new();

        public void PopulateSaveData(SaveData saveData)
        {
            if (saveData.playerData == null)
            {
                saveData.playerData = new();
            }
            saveData.playerData.coins = coins;
            saveData.playerData.stagesUnlocked = stagesUnlocked;
            saveData.playerData.charactersUnlocked = charactersUnlocked;
            saveData.playerData.activeAbilitiesUnlocked = activeAbilitiesUnlocked;
            saveData.playerData.passiveAbilitiesUnlocked = passiveAbilitiesUnlocked;
            saveData.playerData.achievementsUnlocked = achievementsUnlocked;
            saveData.playerData.charactersBought = charactersBought;
            saveData.playerData.passiveBought = passiveBought;
        }

        public void LoadFromSaveData(SaveData saveData)
        {
            coins = saveData.playerData.coins;
            stagesUnlocked = saveData.playerData.stagesUnlocked;
            charactersUnlocked = saveData.playerData.charactersUnlocked;
            activeAbilitiesUnlocked = saveData.playerData.activeAbilitiesUnlocked;
            passiveAbilitiesUnlocked = saveData.playerData.passiveAbilitiesUnlocked;
            achievementsUnlocked = saveData.playerData.achievementsUnlocked;
            charactersBought = saveData.playerData.charactersBought;
            passiveBought = saveData.playerData.passiveBought;
        }
    }
}
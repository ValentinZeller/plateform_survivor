using System.Collections.Generic;
using UnityEngine;

namespace PlateformSurvivor.Save
{
    [System.Serializable]
    public class SaveData
    {
        public PlayerData playerData;
        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public void LoadFromJson(string json)
        {
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }

    public interface ISaveable
    {
        void PopulateSaveData(SaveData saveData);
        void LoadFromSaveData(SaveData saveData);
    }
    
    [System.Serializable]
    public class PlayerData : ISaveable
    {
        public int coins;
        public List<bool> stagesUnlocked;
        public List<int> charactersUnlocked;
        public List<bool> abilitiesUnlocked;
        public List<bool> achievementsUnlocked;
        public void PopulateSaveData(SaveData saveData)
        {
            if (saveData.playerData == null)
            {
                saveData.playerData = new();
            }
            saveData.playerData.coins = coins;
            saveData.playerData.stagesUnlocked = stagesUnlocked;
            saveData.playerData.charactersUnlocked = charactersUnlocked;
            saveData.playerData.abilitiesUnlocked = abilitiesUnlocked;
            saveData.playerData.achievementsUnlocked = achievementsUnlocked;
        }

        public void LoadFromSaveData(SaveData saveData)
        {
            coins = saveData.playerData.coins;
            stagesUnlocked = saveData.playerData.stagesUnlocked;
            charactersUnlocked = saveData.playerData.charactersUnlocked;
            abilitiesUnlocked = saveData.playerData.abilitiesUnlocked;
            achievementsUnlocked = saveData.playerData.achievementsUnlocked;
        }
    }
}
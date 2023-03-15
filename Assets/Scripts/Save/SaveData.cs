using System.Collections.Generic;
using UnityEngine;

namespace PlateformSurvivor.Save
{
    [System.Serializable]
    public class SaveData
    {
        public int coins;

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
}
using System.Collections.Generic;
using UnityEngine;

namespace PlateformSurvivor.Save
{
    public static class SaveDataManager
    {
        public static void SaveJsonData(IEnumerable<ISaveable> saveables)
        {
            SaveData sd = new SaveData();
            foreach (var saveable in saveables)
            {
                saveable.PopulateSaveData(sd);
            }

            if (FileManager.WriteToFile("playerdata.json", sd.ToJson()))
            {
                Debug.Log("Save successful");
            }
        }
    
        public static void LoadJsonData(IEnumerable<ISaveable> saveables)
        {
            if (FileManager.LoadFromFile("playerdata.json", out var json))
            {
                SaveData sd = new SaveData();
                sd.LoadFromJson(json);

                foreach (var saveable in saveables)
                {
                    saveable.LoadFromSaveData(sd);
                }
            
                Debug.Log("Load complete");
            }
        }
    }
}
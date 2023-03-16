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
        public readonly Dictionary<string, int> statsUpgrade = new();
        public List<StatObject> charactersUnlocked;
        public List<string> stagesUnlocked;
        public int coins;
        public List<PlayerData> data;

        private void Awake()
        {
            SaveDataManager.SaveJsonData(data);
            SaveDataManager.LoadJsonData(data);
            DontDestroyOnLoad(gameObject);
            for (int i = 0; i < StatObject.Keys().Count; i++)
            {
                statsUpgrade.Add(StatObject.Keys()[i], 0);
            }
        }

        public bool HasUpgraded()
        {
            return statsUpgrade.Any(pair => pair.Value > 0);
        }
    }
}

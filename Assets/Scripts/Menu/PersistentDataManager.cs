using System.Collections.Generic;
using System.Linq;
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

        private void Awake()
        {
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

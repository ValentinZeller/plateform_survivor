using System;
using System.Collections.Generic;
using System.Linq;
using PlateformSurvivor.Menu;
using ScriptableObject;
using UnityEngine;

namespace PlateformSurvivor.Service
{
    public class AbilityPoolService : MonoBehaviour
    {
        [SerializeField] private List<AbilityObject> activeAbilities;
        [SerializeField] private List<AbilityObject> passiveAbilities;

        private static AbilityPoolService _instance;
        
        private PersistentDataManager persistentDataManager;

        private void Awake()
        {
            _instance = this;
            PersistentInit();
        }
        
        private void PersistentInit()
        {
            persistentDataManager = FindObjectOfType<PersistentDataManager>();
            if (persistentDataManager != null)
            {
                activeAbilities.Clear();
                passiveAbilities.Clear();
                foreach (var active in persistentDataManager.activeAbilitiesUnlocked)
                {
                    activeAbilities.Add(Resources.Load<AbilityObject>("CustomData/Abilities/" + active));
                }

                foreach (var passive in persistentDataManager.passiveAbilitiesUnlocked)
                {
                    passiveAbilities.Add(Resources.Load<AbilityObject>("CustomData/Abilities/" + passive));
                }
            }
        }

        
        public static List<AbilityObject> GetAbilityListByName(string nameAbility)
        {
            if (_instance.activeAbilities.Select(a => a.abilityName).Contains(nameAbility))
            {
                return _instance.activeAbilities;
            }

            return _instance.passiveAbilities;
        }

        public static List<AbilityObject> GetAbilityListByActive(bool isActive)
        {
            if (isActive)
            {
                return _instance.activeAbilities;
            }
            return _instance.passiveAbilities;
        }
    }
}
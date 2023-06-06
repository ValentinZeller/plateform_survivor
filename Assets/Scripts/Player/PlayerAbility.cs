using System;
using System.Collections.Generic;
using PlateformSurvivor.Service;
using ScriptableObject;
using UnityEngine;

namespace PlateformSurvivor.Player
{
    public class PlayerAbility : MonoBehaviour
    {
        private const float MaxActive = 6;
        private const float MaxPassive = 6;
        
        [HideInInspector] public List<string> abilitiesMaxLevel = new();
        [HideInInspector]public List<string> evolutionReady = new();
        private PlayerStat playerStat;
        private static PlayerAbility Instance { get; set; }
        
        // Stores unlocked ability : [Active | Passive][Ability Name] : level
        private Dictionary<bool, Dictionary<string, int>> abilities = new(){{true,new()},{false, new()}};

        public static Dictionary<bool, Dictionary<string, int>> Abilities { get { return Instance.abilities; } }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance.abilities = null;
            Instance = null;
        }
        
        public void AddAbilityComponent(string abilityName)
        {
            if (!Instance.abilities[true].ContainsKey(abilityName)) {
                Instance.abilities[true].Add(abilityName, 1);
            }
            Behaviour ability = GetComponent(abilityName) as Behaviour;
            if (ability != null) ability.enabled = true;
        }
        
        public void AddAbility(AbilityObject abilityObject)
        {
            string abilityName = abilityObject.abilityName;
            bool isActive = abilityObject.isActive;
            int maxLevel = abilityObject.maxLevel;

            if (Instance.abilities[isActive].ContainsKey(abilityName) && Instance.abilities[isActive][abilityName] <= maxLevel)
            {
                Instance.abilities[isActive][abilityName]++;
                if (isActive)
                {
                    EventManager.Trigger("add_"+abilityName.ToLower(), Instance.abilities[true][abilityName]);
                } else
                {
                    EventManager.Trigger("add_passive", abilityName);
                }
                if (Instance.abilities[isActive][abilityName] == maxLevel)
                {
                    AbilityPoolService.GetAbilityListByActive(isActive).Remove(abilityObject);
                    Instance.abilitiesMaxLevel.Add(abilityName);
                }
            } else if (!Instance.abilities[isActive].ContainsKey(abilityName))
            {
                Instance.abilities[isActive].Add(abilityName, 1);
                if (isActive)
                {
                    AddAbilityComponent(abilityName);
                } else
                {
                    EventManager.Trigger("add_passive", abilityName);
                }
                EventManager.Trigger("update_abilities");
            }
        }
        
        public void CheckEvolution(AbilityObject ability)
        {
            if (ability.evolution == null) return;
            if (ability.isActive)
            {
                if (!Instance.abilitiesMaxLevel.Contains(ability.evolution.active.abilityName))
                {
                    return;
                }

                if (ability.evolution.maxPassive)
                {
                    if (!Instance.abilitiesMaxLevel.Contains(ability.evolution.passive.abilityName))
                    {
                        return;
                    }
                }
                else if (!Instance.abilities[false].ContainsKey(ability.evolution.passive.abilityName))
                {
                    return;
                }
                
                Instance.abilitiesMaxLevel.Remove(ability.abilityName);
                if (ability.evolution.maxPassive)
                {
                    Instance.abilitiesMaxLevel.Remove(ability.evolution.passive.abilityName);
                }
                Instance.evolutionReady.Add(ability.abilityName);
            }
            else
            {
                CheckEvolution(ability.evolution.active);
            }
        }
    }
}
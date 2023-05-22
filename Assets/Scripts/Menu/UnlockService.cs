using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using PlateformSurvivor.Player;
using PlateformSurvivor.Service;
using ScriptableObject;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace PlateformSurvivor.Menu
{
    public class UnlockService : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject player;

        [SerializeField] private List<AbilityObject> activeAbilities;
        [SerializeField] private List<AbilityObject> passiveAbilities;

        [SerializeField] private float timeBeforeEvolve = 120;

        private const float MaxActive = 6;
        private const float MaxPassive = 6;

        private List<string> abilitiesMaxLevel = new();
        private List<string> evolutionReady = new();
        private PersistentDataManager persistentDataManager;
        private static UnlockService Instance { get; set; }

        // Stores unlocked ability : [Active | Passive][Ability Name] : level
        private Dictionary<bool, Dictionary<string, int>> abilitiesUnlocked = new(){{true,new()},{false, new()}};
        
        public static Dictionary<bool, Dictionary<string, int>> AbilitiesUnlocked { get { return Instance.abilitiesUnlocked; } }
        
        private void Awake()
        {
            Instance = this;
            PersistentInit();

            EventManager.AddListener("level_up", _OnLevelUp);
            EventManager.AddListener("got_chest", OnChest);
        }
        private void OnDestroy()
        {
            Instance.abilitiesUnlocked = null;
            Instance = null;
        }

        private void PersistentInit()
        {
            persistentDataManager = FindObjectOfType<PersistentDataManager>();
            if (persistentDataManager != null)
            {
                Instance.activeAbilities.Clear();
                Instance.passiveAbilities.Clear();
                foreach (var active in persistentDataManager.activeAbilitiesUnlocked)
                {
                    Instance.activeAbilities.Add(Resources.Load<AbilityObject>("CustomData/Abilities/" + active));
                }

                foreach (var passive in persistentDataManager.passiveAbilitiesUnlocked)
                {
                    Instance.passiveAbilities.Add(Resources.Load<AbilityObject>("CustomData/Abilities/" + passive));
                }
            }
        }

        private static void _OnLevelUp()
        {
            DisplayUpgrade(true);
        }

        private List<AbilityObject> PickRandomAbilites()
        {
            List<AbilityObject> randomAbilities = new();
            List<AbilityObject> abilities = new();
            
            //Restricts abilities unlockable depending of if the player has the max number of a category
            if (Instance.abilitiesUnlocked[true].Count < MaxActive)
            {
                abilities.AddRange(Instance.activeAbilities);
            }
            if (Instance.abilitiesUnlocked[false].Count < MaxPassive)
            {
                abilities.AddRange(Instance.passiveAbilities);
            }

            for (int i = 0; i < Instance.canvas.transform.childCount; i++)
            {
                if (abilities.Count == 0)
                {
                    break;
                }
                int randomIndex = Random.Range(0, abilities.Count);
                randomAbilities.Add(abilities[randomIndex]);
                abilities.RemoveAt(randomIndex);
            }
            return randomAbilities;

        }

        private static void DisplayUpgrade(bool canDisplay)
        {
            if (canDisplay)
            {
                List<AbilityObject> randomAbilities = Instance.PickRandomAbilites();
                if (randomAbilities.Count == 0)
                {
                    DisplayUpgrade(false);
                    return;
                }

                for (int i = 0; i < Instance.canvas.transform.childCount; i++)
                {
                    if (randomAbilities.Count > i)
                    {
                        string randomUnlock = randomAbilities[i].abilityName;
                        bool randomActive = randomAbilities[i].isActive;

                        Instance.canvas.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { UpgradeOnClick(randomUnlock, randomActive); });
                
                        string randomUnlocktext = randomAbilities[i].abilityDisplayName.GetLocalizedString();
                        // Increment level value
                        if (Instance.abilitiesUnlocked[randomAbilities[i].isActive].ContainsKey(randomUnlock))
                        {
                            randomUnlocktext += " " + (Instance.abilitiesUnlocked[randomAbilities[i].isActive][randomUnlock] + 1);
                        }
                        Instance.canvas.transform.GetChild(i).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = randomUnlocktext;
                    }
                    else
                    {
                        Instance.canvas.transform.GetChild(i).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "";
                    }
                }
            }
            Instance.canvas.gameObject.SetActive(canDisplay);
            Time.timeScale = canDisplay ? 0f : 1f;
        }

        private static void UpgradeOnClick(string itemName, bool isActive)
        {
            for (int i = 0; i < Instance.canvas.transform.childCount; i++)
            {
                Instance.canvas.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            }

            AbilityObject abilityObject = GetAbilityListByActive(isActive).Find(abilityObject => abilityObject.name == itemName);
            UnlockAbility(abilityObject);
            CheckEvolution(abilityObject);

            DisplayUpgrade(false);
            // Recall the level method if there are multiple levels to manage
            EventManager.Trigger("got_xp", 0f);
        }

        private static void UnlockAbility(AbilityObject abilityObject)
        {
            string itemName = abilityObject.abilityName;
            bool isActive = abilityObject.isActive;
            int maxLevel = abilityObject.maxLevel;

            if (Instance.abilitiesUnlocked[isActive].ContainsKey(itemName) && Instance.abilitiesUnlocked[isActive][itemName] <= maxLevel)
            {
                Instance.abilitiesUnlocked[isActive][itemName]++;
                if (isActive)
                {
                    EventManager.Trigger("add_"+itemName.ToLower(), Instance.abilitiesUnlocked[true][itemName]);
                } else
                {
                    EventManager.Trigger("add_passive", itemName);
                }
                if (Instance.abilitiesUnlocked[isActive][itemName] == maxLevel)
                {
                    GetAbilityListByActive(isActive).Remove(abilityObject);
                    Instance.abilitiesMaxLevel.Add(itemName);
                }
            } else if (!Instance.abilitiesUnlocked[isActive].ContainsKey(itemName))
            {
                Instance.abilitiesUnlocked[isActive].Add(itemName, 1);
                if (isActive)
                {
                    AddAbility(itemName);
                } else
                {
                    EventManager.Trigger("add_passive", itemName);
                }
                EventManager.Trigger("update_abilities");
            }
        }
        
        private static void OnChest()
        {
            int luck = 1; // WIP
            List<string> abilitiesName = Instance.abilitiesUnlocked[true].Where(a => Instance.activeAbilities.Any(p => p.abilityName == a.Key)).Select(a => a.Key).ToList();
            abilitiesName.AddRange(Instance.abilitiesUnlocked[false].Where(a => Instance.passiveAbilities.Any(p => p.abilityName == a.Key)).Select(a => a.Key).ToList());

            List<AbilityObject> randomPicked = new();
            
            for (int i = 0; i < luck; i++)
            {
                if (Instance.evolutionReady.Count > 0 && Instance.player.GetComponent<PlayerStat>().GetTime() > Instance.timeBeforeEvolve)
                {
                    EventManager.Trigger("evolution_" + Instance.evolutionReady[0].ToLower());
                    EvolutionObject evolution = Resources
                        .Load<AbilityObject>("CustomData/Abilities/" + Instance.evolutionReady[0]).evolution;
                    Instance.abilitiesUnlocked[true].Remove(Instance.evolutionReady[0]);
                    Instance.abilitiesUnlocked[true].Add(evolution.evolutionName, 1);
                    Instance.evolutionReady.RemoveAt(0);
                    EventManager.Trigger("open_chest", evolution.evolutionName);
                }
                else if (abilitiesName.Count > i)
                {
                    string randomName = abilitiesName[Random.Range(0, abilitiesName.Count)];
                    while (Instance.abilitiesMaxLevel.Contains(randomName))
                    {
                        randomName = abilitiesName[Random.Range(0, abilitiesName.Count)];
                    }

                    AbilityObject random = Instance.GetAbilityListByName(randomName).Find(a => a.abilityName == randomName);
                    UnlockAbility(random);
                    randomPicked.Add(random);
                    EventManager.Trigger("open_chest", random);
                }
                else
                {
                    EventManager.Trigger("got_coins", 10);
                }
            }

            foreach (var ability in randomPicked)
            {
                CheckEvolution(ability);
            }
            EventManager.Trigger("update_abilities");
        }
        
        private static void CheckEvolution(AbilityObject ability)
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
                else if (!Instance.abilitiesUnlocked[false].ContainsKey(ability.evolution.passive.abilityName))
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

        public static void AddAbility(string itemName)
        {
            if (!Instance.abilitiesUnlocked[true].ContainsKey(itemName)) {
                Instance.abilitiesUnlocked[true].Add(itemName, 1);
            }
            Behaviour ability = Instance.player.GetComponent(itemName) as Behaviour;
            if (ability != null) ability.enabled = true;
        }

        private List<AbilityObject> GetAbilityListByName(string nameAbility)
        {
            if (activeAbilities.Select(a => a.abilityName).Contains(nameAbility))
            {
                return activeAbilities;
            }

            return passiveAbilities;
        }

        public static List<AbilityObject> GetAbilityListByActive(bool isActive)
        {
            if (isActive)
            {
                return Instance.activeAbilities;
            }
            return Instance.passiveAbilities;
        }
    }
}

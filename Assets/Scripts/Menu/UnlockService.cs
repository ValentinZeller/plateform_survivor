using System.Collections.Generic;
using PlateformSurvivor.Service;
using ScriptableObject;
using UnityEngine;
using UnityEngine.UI;

namespace PlateformSurvivor.Menu
{
    public class UnlockService : MonoBehaviour
    {
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject player;

        [SerializeField] private List<AbilityObject> activeAbilities;
        [SerializeField] private List<AbilityObject> passiveAbilities;

        private const float MaxActive = 6;
        private const float MaxPassive = 6;

        private List<string> abilitiesMaxLevel = new();
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

                for (int i = 0; i < Instance.canvas.transform.childCount; i++)
                {
                    string randomUnlock = randomAbilities[i].abilityName;
                    bool randomActive = randomAbilities[i].isActive;

                    Instance.canvas.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { UpgradeOnClick(randomUnlock, randomActive); });
                
                    string randomUnlocktext = randomAbilities[i].abilityDisplayName;
                    // Increment level value
                    if (Instance.abilitiesUnlocked[randomAbilities[i].isActive].ContainsKey(randomUnlock))
                    {
                        randomUnlocktext += " " + (Instance.abilitiesUnlocked[randomAbilities[i].isActive][randomUnlock] + 1);
                    }
                    Instance.canvas.transform.GetChild(i).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = randomUnlocktext;
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

            AbilityObject abilityObject = Instance.GetAbilityListByActive(isActive).Find(abilityObject => abilityObject.name == itemName);
            UnlockAbility(abilityObject);

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
                    Instance.GetAbilityListByActive(isActive).Remove(abilityObject);
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
            }
        }
        
        private static void OnChest()
        {
            int luck = 2; // WIP
            List<AbilityObject> abilities = new();
            abilities.AddRange(Instance.activeAbilities);
            abilities.AddRange(Instance.passiveAbilities);

            for (int i = 0; i < luck; i++)
            {
                AbilityObject random = abilities[i];
                UnlockAbility(random);
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

        private List<AbilityObject> GetAbilityListByActive(bool isActive)
        {
            if (isActive)
            {
                return activeAbilities;
            }
            return passiveAbilities;
        }
    }
}

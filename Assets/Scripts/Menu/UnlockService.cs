using System;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private Transform buttonsParent;
        [SerializeField] private GameObject player;
        
        [SerializeField] private Button rerollButton;
        [SerializeField] private Button skipButton;
        [SerializeField] private Button banishButton;

        private const float MaxActive = 6;
        private const float MaxPassive = 6;
        
        private const int UnlockCount = 3;
        private const float UnlockThreshold = 0.75f;
        private int currentUnlockCount = 3;

        private List<string> banishAbilities = new();

        private PersistentDataManager persistentDataManager;
        private PlayerAbility playerAbility;

        private void Awake()
        {
            playerAbility = player.GetComponent<PlayerAbility>();

            EventManager.AddListener("level_up", _OnLevelUp);
        }

        private void Start()
        {
            ManageUtilityButtons();
        }

        private void _OnLevelUp()
        {
            DisplayUpgrade(true);
        }

        private List<AbilityObject> PickRandomAbilites()
        {
            List<AbilityObject> randomAbilities = new();
            List<AbilityObject> abilities = new();
            
            //Restricts abilities unlockable depending of if the player has the max number of a category
            if (PlayerAbility.Abilities[true].Count < MaxActive)
            {
                abilities.AddRange(AbilityPoolService.GetAbilityListByActive(true));
            }
            if (PlayerAbility.Abilities[false].Count < MaxPassive)
            {
                abilities.AddRange(AbilityPoolService.GetAbilityListByActive(false));
            }

            if (Random.Range(0, 1) >= UnlockThreshold - PlayerStat.currentStats["Luck"])
            {
                currentUnlockCount = UnlockCount + 1;
            }
            else
            {
                currentUnlockCount = UnlockCount;
            }

            for (int i = 0; i < currentUnlockCount; i++)
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

        private void DisplayUpgrade(bool canDisplay)
        {
            if (canDisplay)
            {
                List<AbilityObject> randomAbilities = PickRandomAbilites();
                if (randomAbilities.Count == 0)
                {
                    DisplayUpgrade(false);
                    return;
                }

                if (currentUnlockCount == 3)
                {
                    buttonsParent.GetChild(3).gameObject.SetActive(false);
                } else
                {
                    buttonsParent.GetChild(3).gameObject.SetActive(true);
                }

                for (int i = 0; i < currentUnlockCount; i++)
                {
                    if (randomAbilities.Count > i)
                    {
                        string randomUnlock = randomAbilities[i].abilityName;
                        bool randomActive = randomAbilities[i].isActive;

                        buttonsParent.GetChild(i).name = randomUnlock;
                        buttonsParent.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { UpgradeOnClick(randomUnlock, randomActive); });
                
                        string randomUnlocktext = randomAbilities[i].abilityDisplayName.GetLocalizedString();
                        // Increment level value
                        if (PlayerAbility.Abilities[randomAbilities[i].isActive].ContainsKey(randomUnlock))
                        {
                            randomUnlocktext += " " + (PlayerAbility.Abilities[randomAbilities[i].isActive][randomUnlock] + 1);
                        }
                        buttonsParent.GetChild(i).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = randomUnlocktext;
                    }
                    else
                    {
                        buttonsParent.GetChild(i).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "";
                    }
                }
            }
            canvas.SetActive(canDisplay);
            Time.timeScale = canDisplay ? 0f : 1f;
        }

        private void UpgradeOnClick(string itemName, bool isActive)
        {
            for (int i = 0; i < buttonsParent.childCount; i++)
            {
                buttonsParent.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            }

            AbilityObject abilityObject = AbilityPoolService.GetAbilityListByActive(isActive).Find(abilityObject => abilityObject.name == itemName);
            playerAbility.AddAbility(abilityObject);
            playerAbility.CheckEvolution(abilityObject);

            DisplayUpgrade(false);
            // Recall the level method if there are multiple levels to manage
            EventManager.Trigger("got_xp", 0f);
        }

        public void Reroll()
        {
            for (int i = 0; i < buttonsParent.childCount; i++)
            {
                buttonsParent.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            }
            DisplayUpgrade(true);
            PlayerStat.currentStats["Reroll"]--;
            ManageUtilityButtons();
        }
        public void Skip()
        {
            for (int i = 0; i < buttonsParent.childCount; i++)
            {
                buttonsParent.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            }
            DisplayUpgrade(false);
            PlayerStat.currentStats["Skip"]--;
            ManageUtilityButtons();
        }

        public void Banish()
        {
            for (int i = 0; i < buttonsParent.childCount; i++)
            {
                Transform child = buttonsParent.GetChild(i);
                child.GetComponent<Button>().onClick.RemoveAllListeners();
                child.GetComponent<Button>().onClick.AddListener( delegate { BanishOnClick(child.name); });
            }

            banishButton.interactable = false;
        }

        private void BanishOnClick(string abilityName)
        {
            List<AbilityObject> abilityList = AbilityPoolService.GetAbilityListByName(abilityName);
            AbilityObject ability = abilityList.Find(a => a.abilityName == abilityName);
            abilityList.Remove(ability);
            banishAbilities.Add(abilityName);
            
            for (int i = 0; i < buttonsParent.childCount; i++)
            {
                buttonsParent.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            }
            DisplayUpgrade(false);
            PlayerStat.currentStats["Banish"]--;
            ManageUtilityButtons();
        }

        private void ManageUtilityButtons()
        {
            if (PlayerStat.currentStats["Reroll"] < 1)
            {
                rerollButton.interactable = false;
            }
            if (PlayerStat.currentStats["Skip"] < 1)
            {
                skipButton.interactable = false;
            }
            if (PlayerStat.currentStats["Banish"] < 1)
            {
                banishButton.interactable = false;
            }
        }
    }
}

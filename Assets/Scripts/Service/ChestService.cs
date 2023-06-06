using System;
using System.Collections.Generic;
using System.Linq;
using PlateformSurvivor.Menu;
using PlateformSurvivor.Player;
using ScriptableObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace PlateformSurvivor.Service
{
    public class ChestService : MonoBehaviour
    {
        [SerializeField] private GameObject chestCanvas;
        [SerializeField] private Transform parentContent;
        [SerializeField] private GameObject chestAbility;
        
        [SerializeField] private PlayerAbility playerAbility;
        [SerializeField] private PlayerStat playerStat;

        private AbilityObject ability;
        private const int NormalChest = 1;
        private const int EpicChest = 3;
        private const int LegendaryChest = 5;

        private float timeBeforeEvolve = 120;

        private const float ChestEpicThreshold = 0.6f;
        private const float ChestLegendaryThreshold = 0.85f;

        private PersistentDataManager persistentDataManager;

        private void Start()
        {
            persistentDataManager = FindObjectOfType<PersistentDataManager>();
            if (persistentDataManager != null)
            {
                timeBeforeEvolve = persistentDataManager.chosenStage.timeBeforeEvolveSecond;
            }
            EventManager.AddListener("got_chest", OnChest);
        }

        public void DisplayChest(bool canDisplay)
        {
            Time.timeScale = canDisplay ? 0f : 1f;
            chestCanvas.SetActive(canDisplay);
        }

        private void AddAbilityDisplay(string spriteName, string displayName, int lvl)
        {
            GameObject abilityContent = Instantiate(chestAbility, parentContent);
            abilityContent.transform.GetChild(0).GetComponent<Image>().sprite = PlayerHUD.GetAbilitySprite(spriteName);
            abilityContent.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = displayName + " " + lvl;
        }
        
        private void OnChest()
        {
            int luck = NormalChest;
            List<string> abilitiesName = PlayerAbility.Abilities[true].Where(a => AbilityPoolService.GetAbilityListByActive(true).Any(p => p.abilityName == a.Key)).Select(a => a.Key).ToList();
            abilitiesName.AddRange(PlayerAbility.Abilities[false].Where(a => AbilityPoolService.GetAbilityListByActive(false).Any(p => p.abilityName == a.Key)).Select(a => a.Key).ToList());

            List<AbilityObject> randomPicked = new();

            float chestRandomRarity = Random.Range(0, 1);

            if (chestRandomRarity >= ChestLegendaryThreshold - PlayerStat.currentStats["Luck"])
            {
                luck = LegendaryChest;
            } else if (chestRandomRarity >= ChestEpicThreshold - PlayerStat.currentStats["Luck"])
            {
                luck = EpicChest;
            }

            for (int i = 0; i < luck; i++)
            {
                if (playerAbility.evolutionReady.Count > 0 && playerStat.GetTime() > timeBeforeEvolve)
                {
                    EventManager.Trigger("evolution_" + playerAbility.evolutionReady[0].ToLower());
                    EvolutionObject evolution = Resources
                        .Load<AbilityObject>("CustomData/Abilities/" + playerAbility.evolutionReady[0]).evolution;
                    PlayerAbility.Abilities[true].Remove(playerAbility.evolutionReady[0]);
                    PlayerAbility.Abilities[true].Add(evolution.evolutionName, 1);
                    playerAbility.evolutionReady.RemoveAt(0);
                    AddAbilityDisplay(evolution.evolutionName, evolution.displayName.GetLocalizedString(), 1);
                }
                else if (abilitiesName.Count > 0)
                {
                    string randomName = abilitiesName[Random.Range(0, abilitiesName.Count)];
                    while (playerAbility.abilitiesMaxLevel.Contains(randomName))
                    {
                        randomName = abilitiesName[Random.Range(0, abilitiesName.Count)];
                    }

                    AbilityObject random = AbilityPoolService.GetAbilityListByName(randomName).Find(a => a.abilityName == randomName);
                    playerAbility.AddAbility(random);
                    randomPicked.Add(random);
                    AddAbilityDisplay(random.abilityName, random.abilityDisplayName.GetLocalizedString(), PlayerAbility.Abilities[random.isActive][random.abilityName]);
                }
                else
                {
                    EventManager.Trigger("got_coins", 10);
                }
            }
            DisplayChest(true);
            foreach (var abilityObject in randomPicked)
            {
                playerAbility.CheckEvolution(abilityObject);
            }
            EventManager.Trigger("update_abilities");
        }
    }
}
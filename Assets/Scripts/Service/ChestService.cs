using System;
using PlateformSurvivor.Menu;
using PlateformSurvivor.Player;
using ScriptableObject;
using UnityEngine;
using UnityEngine.UI;

namespace PlateformSurvivor.Service
{
    public class ChestService : MonoBehaviour
    {
        [SerializeField] private GameObject chestCanvas;
        [SerializeField] private Transform parentContent;
        [SerializeField] private GameObject chestAbility;

        private AbilityObject ability;

        private static ChestService _instance;

        private void Start()
        {
            _instance = this;
            EventManager.AddListener("open_chest", OpenChest);
        }
        
        private void OpenChest()
        {
            DisplayChest(true);
        }

        public void DisplayChest(bool canDisplay)
        {
            Time.timeScale = canDisplay ? 0f : 1f;
            chestCanvas.SetActive(canDisplay);
        }

        public static void AddAbilityDisplay(string name, string displayName, int lvl)
        {
            GameObject abilityContent = Instantiate(_instance.chestAbility, _instance.parentContent);
            abilityContent.transform.GetChild(0).GetComponent<Image>().sprite = PlayerHUD.GetAbilitySprite(name);
            abilityContent.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = displayName + " " + lvl;
        }
    }
}
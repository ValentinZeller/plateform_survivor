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
        [SerializeField] private TMPro.TextMeshProUGUI abilityName;
        [SerializeField] private Image abilitySprite;

        private AbilityObject ability;

        private void Start()
        {
            EventManager.AddListener("open_chest", OpenChest);
        }
        
        private void OpenChest(object data)
        {
            AbilityObject abilityObject = (AbilityObject)data;
            SetAbility(abilityObject);
            DisplayChest(true);
        }

        public void DisplayChest(bool canDisplay)
        {
            Time.timeScale = canDisplay ? 0f : 1f;
            chestCanvas.SetActive(canDisplay);
            if (!canDisplay) { return; }

            abilityName.text = ability.abilityDisplayName.GetLocalizedString() + " " +  + UnlockService.AbilitiesUnlocked[ability.isActive][ability.abilityName];
            abilitySprite.sprite = PlayerHUD.GetAbilitySprite(ability.abilityName);
        }

        public void SetAbility(AbilityObject abilityObject)
        {
            ability = abilityObject;
        }
    }
}
using System;
using PlateformSurvivor.Player;
using ScriptableObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlateformSurvivor.Service
{
    public class AbilityPickup : MonoBehaviour
    {
        [SerializeField] private GameObject popupCanvas;
        [SerializeField] private Image abilitySprite;
        [SerializeField] private TextMeshProUGUI abilityText;

        private PlayerAbility playerAbility;
        private AbilityObject abilityObject;

        private void Start()
        {
            EventManager.AddListener("got_ability", OnPickup);
            playerAbility = FindObjectOfType<PlayerAbility>();
        }

        private void OnPickup(object data)
        {
            abilityObject = (AbilityObject)data;
            abilitySprite.sprite = PlayerHUD.GetAbilitySprite(abilityObject.abilityName);
            abilityText.text = abilityObject.abilityDisplayName.GetLocalizedString();
            DisplayPickup(true);
        }

        public void DisplayPickup(bool canDisplay)
        {
            Time.timeScale = canDisplay ? 0f : 1f;
            popupCanvas.SetActive(canDisplay);
        }

        public void PickAbility()
        {
            playerAbility.AddAbility(abilityObject);
            DisplayPickup(false);
        }
    }
}
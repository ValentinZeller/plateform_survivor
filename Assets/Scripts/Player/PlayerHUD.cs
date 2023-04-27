using System;
using PlateformSurvivor.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace PlateformSurvivor.Player
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private GameObject pause;
        [SerializeField] private GameObject setting;
        [SerializeField] private Image playerXpBar;
        [SerializeField] private TextMeshProUGUI playerHP;
        [SerializeField] private TextMeshProUGUI playerCoin;
        [SerializeField] private TextMeshProUGUI playerLvl;
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private Transform abilities;
        
        [SerializeField] private PlayerStat stat;
        [SerializeField] private PlayerLevel level;

        private void Update()
        {
            playerHP.text = stat.GetHealth().ToString();
            playerCoin.text = stat.GetCoins().ToString();
            playerLvl.text = level.GetLvl().ToString();
            timer.text = DisplayTime();
            playerXpBar.fillAmount = level.GetXp() / level.GetXpNeeded();
            UpdateAbilities();
        }

        private string DisplayTime()
        {
            string text = "0 : 00";
            string minute = Mathf.FloorToInt(stat.GetTime() / 60).ToString();
            string second = Mathf.FloorToInt(stat.GetTime() % 60).ToString();
            if (second.Length == 1)
            {
                second = "0" + second;
            }
            text = minute + " : " + second;
            return text;
        }

        private void UpdateAbilities()
        {
            foreach (var (isActive, dictionary) in UnlockService.AbilitiesUnlocked)
            {
                int index = 0;
                foreach (var name in dictionary.Keys)
                {
                    Sprite abilitySprite = UnlockService.GetAbilityListByActive(isActive)
                        .Find(ability => ability.name == name).sprite;
                    abilities.GetChild(Convert.ToInt32(!isActive)).GetChild(index).gameObject.GetComponent<Image>().sprite = abilitySprite;
                    index++;
                }
            }
        }

        public void Pause(InputAction.CallbackContext ctx)
        {
            if (ctx.performed && stat.GetHealth() > 0 && !setting.activeSelf)
            {
                Time.timeScale = !pause.activeSelf ? 0f : 1f;
                pause.SetActive(!pause.activeSelf);
            }
        }
    }
}

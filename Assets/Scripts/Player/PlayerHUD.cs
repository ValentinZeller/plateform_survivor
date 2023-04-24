using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlateformSurvivor.Player
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private GameObject pause;
        [SerializeField] private GameObject setting;
        [SerializeField] private TextMeshProUGUI playerHP;
        [SerializeField] private TextMeshProUGUI playerCoin;
        [SerializeField] private TextMeshProUGUI playerLvl;
        [SerializeField] private TextMeshProUGUI playerXp;
        [SerializeField] private TextMeshProUGUI playerXpNeeded;
        [SerializeField] private TextMeshProUGUI timer;
        
        [SerializeField] private PlayerStat stat;
        [SerializeField] private PlayerLevel level;

        private void Update()
        {
            playerHP.text = stat.GetHealth().ToString();
            playerCoin.text = stat.GetCoins().ToString();
            playerLvl.text = level.GetLvl().ToString();
            playerXp.text = level.GetXp().ToString();
            playerXpNeeded.text = level.GetXpNeeded().ToString();
            timer.text = DisplayTime();
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

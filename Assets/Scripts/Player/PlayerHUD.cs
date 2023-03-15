using TMPro;
using UnityEngine;

namespace PlateformSurvivor.Player
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerHP;
        [SerializeField] private TextMeshProUGUI playerCoin;
        [SerializeField] private TextMeshProUGUI playerLvl;
        [SerializeField] private TextMeshProUGUI playerXp;
        [SerializeField] private TextMeshProUGUI playerXpNeeded;
        
        [SerializeField] private PlayerStat stat;
        [SerializeField] private PlayerLevel level;

        private void Update()
        {
            playerHP.text = stat.GetHealth().ToString();
            playerCoin.text = stat.GetCoins().ToString();
            playerLvl.text = level.GetLvl().ToString();
            playerXp.text = level.GetXp().ToString();
            playerXpNeeded.text = level.GetXpNeeded().ToString();
        }
    }
}

using TMPro;
using UnityEngine;

namespace PlateformSurvivor.Player
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerHP;
        [SerializeField] private TextMeshProUGUI playerCoin;
        [SerializeField] private PlayerStat stat;

        private void Update()
        {
            playerHP.text = stat.GetHealth().ToString();
            playerCoin.text = stat.GetCoins().ToString();
        }
    }
}

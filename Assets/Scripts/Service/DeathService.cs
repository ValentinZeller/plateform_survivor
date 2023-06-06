using PlateformSurvivor.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PlateformSurvivor.Service
{
    public class DeathService : MonoBehaviour
    {
        [SerializeField] private GameObject deathScreen;
        [SerializeField] private GameObject resultScreen;
        [SerializeField] private Button revive;
        [SerializeField] private PlayerStat playerStat;
        [SerializeField] private TMPro.TextMeshProUGUI liveValue;
        private void Start()
        {
            EventManager.AddListener("death", OnDeath);
        }
        
        private void OnDeath()
        {
            Time.timeScale = 0;
            deathScreen.SetActive(true);
            liveValue.text = playerStat.currentStats["Live"].ToString();
            if (playerStat.currentStats["Live"] > 0)
            {
                revive.interactable = true;
            }
        }

        public void Revive()
        {
            playerStat.currentStats["Live"]--;
            EventManager.Trigger("regen_health", playerStat.currentStats["Health"] / 2);
            Time.timeScale = 1;
            deathScreen.SetActive(false);
        }

        public void DisplayResults()
        {
            Time.timeScale = 1;
            deathScreen.SetActive(false);
            resultScreen.SetActive(true);
        }

        public void BackToMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}

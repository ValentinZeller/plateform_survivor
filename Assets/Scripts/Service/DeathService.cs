using UnityEngine;
using UnityEngine.SceneManagement;

namespace PlateformSurvivor.Service
{
    public class DeathService : MonoBehaviour
    {
        [SerializeField] private GameObject deathScreen;
        private void Start()
        {
            EventManager.AddListener("death", OnDeath);
        }
        
        private void OnDeath()
        {
            Time.timeScale = 0;
            deathScreen.SetActive(true);
        }

        public void BackToMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }
    }
}

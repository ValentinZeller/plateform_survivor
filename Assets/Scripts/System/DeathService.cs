using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathService : MonoBehaviour
{
    [SerializeField] GameObject deathScreen;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddListener("death", OnDeath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDeath()
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

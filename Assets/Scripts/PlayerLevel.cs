using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    private float xp = 0f;
    private float xpNeeded = 2f;
    private float lvl = 1;

    private PlayerMovement playerMovement;
    [SerializeField] private Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("XP"))
        {
            xp++;
            Destroy(collision.gameObject);
            LevelUp();
        }
    }

    private void LevelUp()
    {
        if (xp >= xpNeeded)
        {
            xp = 0;
            xpNeeded += 1f;
            lvl++;
            Time.timeScale = 0;
            canvas.gameObject.SetActive(true);
        }
    }

    public void UnlockAbility(int number)
    {
        switch (number)
        {
            case 0:
                playerMovement.UnlockDash();
                break;
            case 1:
                playerMovement.UnlockDoubleJump();
                break;
        }
        canvas.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

}

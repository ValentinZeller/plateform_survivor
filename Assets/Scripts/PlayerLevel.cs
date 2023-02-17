using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    private float xp = 0f;
    private float xpNeeded = 2f;
    private float lvl = 1;
    // Start is called before the first frame update

    void Start()
    {
        
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
            EventManager.Trigger("level_up");
        }
    }

}

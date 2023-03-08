using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum itemType { Health, XP, Coin };

public class Item : MonoBehaviour
{
    public itemType type;
    public float value;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            switch(type)
            {
                case itemType.Health:
                    EventManager.Trigger("regen_health", value);
                    break;
                case itemType.XP:
                    EventManager.Trigger("got_xp", value);
                    break;
                case itemType.Coin:
                    EventManager.Trigger("got_coin", value);
                    break;

            }
            Destroy(gameObject);
        }
    }
}

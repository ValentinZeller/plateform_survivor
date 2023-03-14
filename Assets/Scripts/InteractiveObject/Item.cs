using PlateformSurvivor.Service;
using UnityEngine;

namespace PlateformSurvivor.InteractiveObject
{
    public enum ItemType { Health, Xp, Coin };

    public class Item : MonoBehaviour
    {
        public ItemType type;
        public float value;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                switch(type)
                {
                    case ItemType.Health:
                        EventManager.Trigger("regen_health", value);
                        break;
                    case ItemType.Xp:
                        EventManager.Trigger("got_xp", value);
                        break;
                    case ItemType.Coin:
                        EventManager.Trigger("got_coin", value);
                        break;

                }
                Destroy(gameObject);
            }
        }
    }
}
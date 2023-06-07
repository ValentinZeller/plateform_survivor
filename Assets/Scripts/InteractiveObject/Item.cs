using PlateformSurvivor.Service;
using ScriptableObject;
using UnityEngine;

namespace PlateformSurvivor.InteractiveObject
{
    public enum ItemType { Health, Xp, Coin, Chest, Ability };

    public class Item : MonoBehaviour
    {
        public ItemType type;
        public float value;
        public AbilityObject abilityObject;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                switch(type)
                {
                    case ItemType.Health:
                        EventManager.Trigger("got_health", value);
                        break;
                    case ItemType.Xp:
                        EventManager.Trigger("got_xp", value);
                        break;
                    case ItemType.Coin:
                        EventManager.Trigger("got_coin", value);
                        break;
                    case ItemType.Chest:
                        EventManager.Trigger("got_chest");
                        break;
                    case ItemType.Ability:
                        EventManager.Trigger("got_ability", abilityObject);
                        break;
                }
                Destroy(gameObject);
            }
        }
    }
}
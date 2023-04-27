using PlateformSurvivor.Service;
using UnityEngine;

namespace PlateformSurvivor.InteractiveObject
{
    public class InteractableBlock : MonoBehaviour, IDamageable
    {
        
        [SerializeField] private bool isDestroyable;
        [SerializeField] private int coins;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private GameObject item;

        private const float LeftOffset = -0.71f;
        private const float RightOffset = 0.71f;
        private int currentCoins;
        private bool isEmpty;

        private void Start()
        {
            currentCoins = coins;
            EventManager.AddListener("reload_block", Reload);
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Vector3 position = transform.position;
            float deltaX = position.x - collision.GetContact(0).point.x;
            float deltaY = position.y - collision.GetContact(0).point.y;
            if (deltaY > 0 && deltaX < RightOffset && deltaX > LeftOffset && collision.gameObject.CompareTag("Player"))
            {
                Damage(1);
            }
        }

        private void Reload()
        {
            isEmpty = false;
            currentCoins = coins;
            sprite.color = new Color(1, 0.92f, 0);
        }

        public bool IsDestroyable()
        {
            return isDestroyable;
        }

        public void Damage(float damage)
        {
            if (isDestroyable)
            {
                Destroy(gameObject);
            } 
        
            if (currentCoins > 0)
            {
                currentCoins--;
                EventManager.Trigger("got_coin",1);
            }

            if (item != null && !isEmpty)
            {
                Instantiate(item, new Vector2(transform.position.x, transform.position.y + 1.5f), Quaternion.identity);
                isEmpty = true;
            }

            if (isEmpty || currentCoins == 0)
            {
                sprite.color = new Color(0.48f, 0.35f, 0.22f);
            }
        }
    }
}

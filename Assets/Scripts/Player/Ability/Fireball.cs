using System.Collections;
using PlateformSurvivor.Service;
using UnityEngine;

namespace PlateformSurvivor.Player.Ability
{
    public class Fireball : MonoBehaviour
    {
        [SerializeField] private GameObject fireProjectile;
        
        private float fireCooldown = 1.5f;
        private bool canFire = true;
        private readonly Vector2 offset = new(1f, 0.5f);
        private Vector2 velocity = new(10, -10);
        private float fireStrength;
        private PlayerStat stat;

        private void Start()
        {
            stat = GetComponent<PlayerStat>();
            EventManager.AddListener("add_fireball", OnAddFireball);
        }

        private void Update()
        {
            if (canFire)
            {
                GameObject instance = Instantiate(fireProjectile, (Vector2)transform.position + offset * transform.localScale.x, Quaternion.identity);
                instance.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * transform.localScale.x, velocity.y);
                instance.GetComponent<FireballBehavior>().SetStrength(stat.currentStats["Strength"] + fireStrength);
                StartCoroutine(FireAction());
            }
        }

        private IEnumerator FireAction()
        {
            canFire = false;
            yield return new WaitForSeconds(fireCooldown);
            canFire = true;
        }

        private void OnAddFireball(object data)
        {
            int level = (int)data;
            switch (level)
            {
                case 2:
                    fireCooldown -= 0.125f;
                    break;
                case 3:
                    velocity.x += velocity.x + 0.05f;
                    break;
                case 4:
                    fireStrength += 1;
                    break;
                case 5:
                    fireCooldown -= 0.125f;
                    break;
                case 6:
                    velocity.x += velocity.x + 0.05f;
                    break;
                case 7:
                    fireStrength += 1;
                    break;
                case 8:
                    fireCooldown -= 0.125f;
                    break;
            }
        }
    }
}

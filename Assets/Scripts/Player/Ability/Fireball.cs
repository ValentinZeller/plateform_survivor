using System.Collections;
using PlateformSurvivor.Service;
using UnityEngine;

namespace PlateformSurvivor.Player.Ability
{
    public class Fireball : MonoBehaviour, IEvolution
    {
        [SerializeField] private GameObject fireProjectile;
        
        private float fireCooldown = 1.5f;
        private bool canFire = true;
        private bool bothSide = false;
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
                SpawnFireball(transform.localScale.x);
                if (bothSide)
                {
                    SpawnFireball(- transform.localScale.x);
                }
                StartCoroutine(FireAction());
            }
        }

        private void SpawnFireball(float direction)
        {
            GameObject instance = Instantiate(fireProjectile, (Vector2)transform.position + offset * direction, Quaternion.identity);
            instance.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * direction, velocity.y);
        }

        private IEnumerator FireAction()
        {
            canFire = false;
            yield return new WaitForSeconds(fireCooldown - fireCooldown * stat.currentStats["Cooldown"]);
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
                    bothSide = true;
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

        public bool IsEvolved { get; set; }
        public void OnEvolution()
        {
            IsEvolved = true;
        }
    }
}

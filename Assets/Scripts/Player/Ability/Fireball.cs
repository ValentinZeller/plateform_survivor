using System;
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
        private float fireAmount = 1;
        private readonly Vector2 offset = new(1f, 0.5f);
        private Vector2 velocity = new(10, -10);
        private float fireStrength = 1;
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
                for (int i = 0; i < fireAmount + stat.currentStats["Amount"]; i++)
                {
                    float direction = i % 2 == 0 ? 1 : -1;
                    float velocityYOffset = 2 * Mathf.Floor(i/2);
                    SpawnFireball(transform.localScale.x * direction, velocityYOffset);
                }
                
                StartCoroutine(FireAction());
            }
        }

        private void SpawnFireball(float direction, float velocityYOffset)
        {
            GameObject instance = Instantiate(fireProjectile, (Vector2)transform.position + offset * direction, Quaternion.identity);
            instance.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * direction, velocity.y + velocityYOffset);
            instance.GetComponent<FireballBehavior>().SetStrength(fireStrength + stat.currentStats["Strength"]);
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
                    fireAmount++;
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

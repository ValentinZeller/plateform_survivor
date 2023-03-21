using System.Collections;
using System.Collections.Generic;
using PlateformSurvivor.Player.Ability;
using UnityEngine;

namespace PlateformSurvivor.Enemy
{
    public class EnemyShoot : MonoBehaviour
    {
        [SerializeField] private GameObject bullet;
        [SerializeField] private EnemyBehavior behavior;

        private bool canShoot = true;
        private readonly Vector2 offset = new(1f, 0.3f);
        private float cooldown = 3f;
        
        private void Update()
        {
            if (canShoot)
            {
                SpawnBullet(behavior.GetDirection());
                StartCoroutine(FireAction());
            }
        }

        private void SpawnBullet(float direction)
        {
            GameObject instance = Instantiate(bullet, (Vector2)transform.position + offset * direction, Quaternion.identity);
            instance.GetComponent<BulletBehavior>().SetDirection(direction);
        }

        private IEnumerator FireAction()
        {
            canShoot = false;
            yield return new WaitForSeconds(cooldown);
            canShoot = true;
        }
    }
}
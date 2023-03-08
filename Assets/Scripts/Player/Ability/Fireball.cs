using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private float fireCooldown = 1.5f;
    private bool canFire = true;
    private Vector2 offset = new Vector2(1f, 0.5f);
    private Vector2 velocity = new Vector2(10, -10);
    private float fireStrength = 0f;

    [SerializeField] private GameObject fireProjectile;
    private PlayerStat stat;
    void Start()
    {
        stat = GetComponent<PlayerStat>();
        EventManager.AddListener("add_fireball", OnAddFireball);
    }

    void Update()
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
                velocity.x += velocity.x + 5 / 100;
                break;
            case 4:
                fireStrength += 1;
                break;
            case 5:
                fireCooldown -= 0.125f;
                break;
            case 6:
                velocity.x += velocity.x + 5 / 100;
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

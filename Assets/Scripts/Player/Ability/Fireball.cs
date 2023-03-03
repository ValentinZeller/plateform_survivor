using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private float fireCooldown = 1.5f;
    private bool canFire = true;
    private Vector2 offset = new Vector2(1f, 0.5f);
    private Vector2 velocity = new Vector2(10, -10);

    [SerializeField] private GameObject fireProjectile;
    void Start()
    {
        
    }

    void Update()
    {
        if (canFire)
        {
            GameObject instance = Instantiate(fireProjectile, (Vector2)transform.position + offset * transform.localScale.x, Quaternion.identity);
            instance.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * transform.localScale.x, velocity.y);
            StartCoroutine(FireAction());
        }
    }

    private IEnumerator FireAction()
    {
        canFire = false;
        yield return new WaitForSeconds(fireCooldown);
        canFire = true;
    }
}

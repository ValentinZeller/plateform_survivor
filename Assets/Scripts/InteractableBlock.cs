using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBlock : MonoBehaviour, IDamageable
{
    [SerializeField] private bool isDestroyable;
    [SerializeField] private int coins = 1;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] GameObject item;
    void Start()
    {
        
    }

    
    void Update()
    {
        
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
        
        if (coins > 0)
        {
            coins--;
            EventManager.Trigger("got_coin");
        } else if (coins == 0)
        {
            sprite.color = new Color(0.48f,0.35f,0.22f);
        }

        if (coins <= 0 && item != null)
        {
            Instantiate(item, new Vector2(transform.position.x, transform.position.y + 1.5f), Quaternion.identity);
        }
    }
}

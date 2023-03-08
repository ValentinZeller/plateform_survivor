using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBlock : MonoBehaviour, IDamageable
{
    private bool isEmpty = false;
    [SerializeField] private bool isDestroyable;
    [SerializeField] private int coins = 1;
    private int currentCoins;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] GameObject item;
    void Start()
    {
        currentCoins = coins;
        EventManager.AddListener("reload_block", Reload);
    }

    
    void Update()
    {
        
    }

    public void Reload()
    {
        isEmpty = false;
        coins = currentCoins;
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

        if (isEmpty || coins == 0)
        {
            sprite.color = new Color(0.48f, 0.35f, 0.22f);
        }
    }
}

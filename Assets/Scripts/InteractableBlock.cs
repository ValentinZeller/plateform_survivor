using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBlock : MonoBehaviour, IDamageable
{
    private bool isDestroyable;
    private int coins = 1;
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
        } else
        {
            coins--;
            EventManager.Trigger("got_coin");
        }
    }
}

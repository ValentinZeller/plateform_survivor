using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    private float baseSpeed = 8f;
    private float baseJumpForce = 16f;

    public float currentSpeed;
    public float currentJumpForce;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = baseSpeed;
        currentJumpForce = baseJumpForce;
        EventManager.AddListener("add_passive", _OnAddPassive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _OnAddPassive(object data)
    {
        string itemName = (string)data;
        switch (itemName)
        {
            case "Speed":
                currentSpeed = baseSpeed + baseSpeed * 5 / 100 * UnlockService.AbilitiesUnlocked[false][itemName];
                break;
        }
    }
}

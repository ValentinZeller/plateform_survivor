using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    private Dictionary<string, float> baseStats = new();
    public Dictionary<string, float> currentStats = new();

    // Start is called before the first frame update
    void Start()
    {
        baseStats.Add("Speed", 8f);
        baseStats.Add("JumpForce", 16f);

        currentStats.Add("Speed", baseStats["Speed"]);
        currentStats.Add("JumpForce", baseStats["JumpForce"]);

        EventManager.AddListener("add_passive", _OnAddPassive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _OnAddPassive(object data)
    {
        string itemName = (string)data;
        currentStats[itemName] = baseStats[itemName] + baseStats[itemName] * 5 / 100 * UnlockService.AbilitiesUnlocked[false][itemName];
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour, IDamageable
{
    [SerializeField] StatObject stat;
    private Dictionary<string, float> baseStats = new();
    public Dictionary<string, float> currentStats = new();

    public float Health { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        baseStats.Add("Speed", stat.speed);
        baseStats.Add("JumpForce", stat.jumpForce);
        baseStats.Add("Strength", stat.strength);
        baseStats.Add("Health", stat.health);

        currentStats.Add("Speed", baseStats["Speed"]);
        currentStats.Add("JumpForce", baseStats["JumpForce"]);
        currentStats.Add("Strength", baseStats["Strength"]);
        currentStats.Add("Health", baseStats["Health"]);

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

    public void Damage(float damage)
    {
        currentStats["Health"]--;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStat : MonoBehaviour, IDamageable
{
    [SerializeField] StatObject stat;
    private Dictionary<string, float> baseStats = new();
    public Dictionary<string, float> currentStats = new();

    private PersistentDataManager persistentDataManager;
    private int currentCoins = 0;
    private float health;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<PersistentDataManager>())
        {
            persistentDataManager = FindObjectOfType<PersistentDataManager>();
            stat = persistentDataManager.chosenCharacter;
        }

        UnlockService.AddAbility(Enum.GetName(typeof(activeAbility), stat.startAbility));

        for (int i = 0; i < StatObject.Keys().Count; i++ )
        {
            float percent = 0;
            UpgradeObject currentUpgrade = Resources.Load<UpgradeObject>("CustomData/Upgrades/" + StatObject.Keys()[i]);
            if (currentUpgrade != null)
            {
                percent = currentUpgrade.percentEffect;
            }
            baseStats.Add(StatObject.Keys()[i], stat[i]);
            float bonusStats = 0;
            if (persistentDataManager != null)
            {
                bonusStats = baseStats[StatObject.Keys()[i]] * percent * persistentDataManager.statsUpgrade[StatObject.Keys()[i]];
            }
            currentStats.Add(StatObject.Keys()[i], baseStats[StatObject.Keys()[i]] + bonusStats);
        }

        health = currentStats["Health"];
        EventManager.AddListener("add_passive", OnAddPassive);
        EventManager.AddListener("got_coin", GotCoin);
        EventManager.AddListener("regen_health", RegenHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAddPassive(object data)
    {
        string itemName = (string)data;
        AbilityObject currentAbility = Resources.Load<AbilityObject>("CustomData/Abilities/" + itemName);
        currentStats[itemName] = currentStats[itemName] + currentStats[itemName] * currentAbility.percent * UnlockService.AbilitiesUnlocked[false][itemName];
    }

    public float GetHealth()
    {
        return health;
    }

    private void GotCoin(object data)
    {
        currentCoins += (int)data;
        if (persistentDataManager != null)
        {
            persistentDataManager.coins++;
        }
    }

    private void RegenHealth(object data)
    {
        float regen = (float)data;
        health += regen;
        if (health > currentStats["Health"])
        {
            health = currentStats["Health"];
        }
    }

    public void Damage(float damage)
    {
        health--;
        if (health <= 0)
        {
            EventManager.Trigger("death");
        }
    }
}

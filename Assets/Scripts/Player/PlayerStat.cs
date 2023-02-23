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

    public float Health { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<PersistentDataManager>())
        {
            persistentDataManager = FindObjectOfType<PersistentDataManager>();
            stat = persistentDataManager.chosenCharacter;
        }
        
        for (int i = 0; i < StatObject.Keys().Count; i++ )
        {
            baseStats.Add(StatObject.Keys()[i], stat[i]);
            currentStats.Add(StatObject.Keys()[i], baseStats[StatObject.Keys()[i]] + baseStats[StatObject.Keys()[i]] * 5/100 * persistentDataManager.statsUpgrade[StatObject.Keys()[i]] );
        }

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
        if (currentStats["Health"] <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}

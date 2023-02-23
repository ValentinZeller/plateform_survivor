using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDataManager : MonoBehaviour
{
    [HideInInspector] public StatObject chosenCharacter;
    [HideInInspector] public Dictionary<string, int> statsUpgrade = new();
    public int coins = 100;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < StatObject.Keys().Count; i++)
        {
            statsUpgrade.Add(StatObject.Keys()[i], 0);
        }
    }

    public bool HasUpgraded()
    {
        foreach (KeyValuePair<string, int> pair in statsUpgrade)
        {
            if (pair.Value > 0)
            {
                return true;
            }
        }

        return false;
    }
}

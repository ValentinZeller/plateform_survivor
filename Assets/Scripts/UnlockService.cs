using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockService : MonoBehaviour
{
    private float maxActive = 6;
    private float maxPassive = 6;

    [SerializeField] private GameObject canvas;
    [SerializeField] GameObject player;

    [SerializeField] private List<AbilityObject> activeAbilities;
    [SerializeField] private List<AbilityObject> passiveAbilities;

    // Stores unlocked ability : [Active | Passive][Ability Name] : level
    private Dictionary<bool, Dictionary<string, int>> _abilitiesUnlocked = new();
    public static Dictionary<bool, Dictionary<string, int>> AbilitiesUnlocked { get { return Instance._abilitiesUnlocked; } }

    private static UnlockService _instance;
    private static UnlockService Instance { get { return _instance; } }


    private void Awake()
    {
        _instance = this;
        EventManager.AddListener("level_up", _OnLevelUp);
        _instance._abilitiesUnlocked.Add(true, new());
        _instance._abilitiesUnlocked.Add(false, new());
    }
    void OnDestroy()
    {
        _abilitiesUnlocked = null;
        _instance = null;
    }


    private void _OnLevelUp()
    {
        DisplayUpgrade(true);
    }

    private List<AbilityObject> PickRandomAbilites()
    {
        List<AbilityObject> randomAbilities = new();
        List<AbilityObject> abilities = new();

        //Restricts abilities unlockable depending of if the player has the max number of a category
        if (_instance._abilitiesUnlocked[true].Count < maxActive)
        {
            abilities.AddRange(_instance.activeAbilities);
        }
        if (_instance._abilitiesUnlocked[false].Count < maxPassive)
        {
            abilities.AddRange(_instance.passiveAbilities);
        }

        for (int i = 0; i < _instance.canvas.transform.childCount; i++)
        {
            int randomIndex = Random.Range(0, abilities.Count);
            randomAbilities.Add(abilities[randomIndex]);
            abilities.RemoveAt(randomIndex);
        }
        return randomAbilities;

    }

    public static void DisplayUpgrade(bool canDisplay)
    {
        if (canDisplay)
        {
            List<AbilityObject> randomAbilities = _instance.PickRandomAbilites();

            for (int i = 0; i < _instance.canvas.transform.childCount; i++)
            {
                string randomUnlock = randomAbilities[i].abilityName;
                bool randomActive = randomAbilities[i].isActive;

                _instance.canvas.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { UnlockItem(randomUnlock, randomActive); });
                
                string randomUnlocktext = randomUnlock;
                // Increment level value
                if (_instance._abilitiesUnlocked[randomAbilities[i].isActive].ContainsKey(randomUnlock))
                {
                    randomUnlocktext += " " + (_instance._abilitiesUnlocked[randomAbilities[i].isActive][randomUnlock] + 1);
                }
                _instance.canvas.transform.GetChild(i).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = randomUnlocktext;
            }
        }
        _instance.canvas.gameObject.SetActive(canDisplay);
        Time.timeScale = canDisplay ? 0f : 1f;
    }
    
    public static void UnlockItem(string itemName, bool isActive)
    {
        for (int i = 0; i < _instance.canvas.transform.childCount; i++)
        {
            _instance.canvas.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
        }

        if (_instance._abilitiesUnlocked[isActive].ContainsKey(itemName) && _instance._abilitiesUnlocked[isActive][itemName] <= 8)
        {
            _instance._abilitiesUnlocked[isActive][itemName]++;
            if (_instance._abilitiesUnlocked[isActive][itemName] == 8)
            {
                if (isActive)
                {
                    _instance.activeAbilities.Remove(_instance.activeAbilities.Find(abilityObject => abilityObject.name == itemName));
                } else
                {
                    _instance.activeAbilities.Remove(_instance.passiveAbilities.Find(abilityObject => abilityObject.name == itemName));
                }
            }
        } else if (!_instance._abilitiesUnlocked[isActive].ContainsKey(itemName))
        {
            _instance._abilitiesUnlocked[isActive].Add(itemName, 1);
            if (isActive)
            {
                AddAbility(itemName);
            }
        }

        DisplayUpgrade(false);
    }

    public static void AddAbility(string itemName)
    {
        Behaviour ability = _instance.player.GetComponent(itemName) as Behaviour;
        ability.enabled = true;
    }
}

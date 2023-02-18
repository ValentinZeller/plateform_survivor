using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockService : MonoBehaviour
{

    [SerializeField] private GameObject canvas;
    [SerializeField] private List<string> unlocks;
    private Dictionary<string, int> _unlocked = new Dictionary<string, int>();
    public static Dictionary<string, int> Unlocked { get { return Instance._unlocked; } }
    [SerializeField] GameObject player;

    private static UnlockService _instance;
    private static UnlockService Instance { get { return _instance; } }


    private void Awake()
    {
        _instance = this;
        EventManager.AddListener("level_up", _OnLevelUp);
    }
    void OnDestroy()
    {
        _unlocked.Clear();
        _instance = null;
    }


    private void _OnLevelUp()
    {
        DisplayUpgrade(true);
    }

    public static void DisplayUpgrade(bool canDisplay)
    {
        if (canDisplay)
        {
            List<string> randomUnlocks= new List<string>();
            for (int i = 0; i < _instance.canvas.transform.childCount; i++)
            {
                string temp;
                do {
                    temp = _instance.unlocks[Random.Range(0, _instance.unlocks.Count)];
                } while (randomUnlocks.Contains(temp));
                randomUnlocks.Add(temp);
            }
            for (int i = 0; i < _instance.canvas.transform.childCount; i++)
            {
                string randomUnlock = randomUnlocks[i];
                _instance.canvas.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { UnlockItem(randomUnlock); });

                string randomUnlocktext = randomUnlock;
                if (_instance._unlocked.ContainsKey(randomUnlock))
                {
                    randomUnlocktext += " " + (_instance._unlocked[randomUnlock] + 1);
                }
                _instance.canvas.transform.GetChild(i).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = randomUnlocktext;
            }
        }
        _instance.canvas.gameObject.SetActive(canDisplay);
        Time.timeScale = canDisplay ? 0f : 1f;
    }
    
    public static void UnlockItem(string itemName)
    {
        for (int i = 0; i < _instance.canvas.transform.childCount; i++)
        {
            _instance.canvas.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
        }

        if (_instance._unlocked.ContainsKey(itemName) && _instance._unlocked[itemName] <= 8)
        {
            _instance._unlocked[itemName]++;
            if (_instance._unlocked[itemName] == 8)
            {
                _instance.unlocks.Remove(itemName);
            }
        } else
        {
            _instance._unlocked.Add(itemName, 1);
            AddAbility(itemName);
        }

        DisplayUpgrade(false);
    }

    public static void AddAbility(string itemName)
    {
        Behaviour ability = _instance.player.GetComponent(itemName) as Behaviour;
        ability.enabled = true;

    }
}

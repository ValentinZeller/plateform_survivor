using System;
using PlateformSurvivor.Player;
using ScriptableObject;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace PlateformSurvivor.Menu
{
    public class Results : MonoBehaviour
    {
        private PersistentDataManager persistentDataManager;
        
        [SerializeField] private PlayerStat playerStat;
        [SerializeField] private PlayerLevel playerLevel;
        
        [SerializeField] private TextMeshProUGUI stage;
        [SerializeField] private TextMeshProUGUI character;
        [SerializeField] private TextMeshProUGUI time;
        [SerializeField] private TextMeshProUGUI coin;
        [SerializeField] private TextMeshProUGUI lvl;
        [SerializeField] private TextMeshProUGUI kill;
        
        [SerializeField] private Transform abilities;
        [SerializeField] private GameObject slot;

        private void Awake()
        {
            if (FindObjectOfType<PersistentDataManager>())
            {
                persistentDataManager = FindObjectOfType<PersistentDataManager>();
            }
        }
        private void Start()
        {
            stage.text = persistentDataManager.chosenStage.displayName.GetLocalizedString();
            character.text = persistentDataManager.chosenCharacter.displayName;
            time.text = PlayerHUD.DisplayTime(playerStat.GetTime());
            coin.text = playerStat.GetCoins().ToString();
            lvl.text = playerLevel.GetLvl().ToString();
            kill.text = playerStat.GetKills().ToString();
            
            foreach (var (isActive, dictionary) in UnlockService.AbilitiesUnlocked)
            {
                int index = 0;
                foreach (var name in dictionary.Keys)
                {
                    Sprite abilitySprite;
                    if (Resources.Load<AbilityObject>("CustomData/Abilities/"+name))
                    {
                        abilitySprite = Resources.Load<AbilityObject>("CustomData/Abilities/"+name).sprite;
                    }
                    else
                    {
                        abilitySprite = Resources.Load<EvolutionObject>("CustomData/Evolutions/"+name).sprite;
                    }

                    GameObject icon = Instantiate(slot, abilities);
                    icon.GetComponent<Image>().sprite = abilitySprite;
                    index++;
                }
            }
        }
    }
}

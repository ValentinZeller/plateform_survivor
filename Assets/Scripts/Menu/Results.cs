using System;
using PlateformSurvivor.Player;
using ScriptableObject;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
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
        [SerializeField] private Transform damage;
        [SerializeField] private GameObject typedDamage;

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
            
            foreach (var (isActive, dictionary) in PlayerAbility.Abilities)
            {
                foreach (var ability in dictionary)
                {
                    Sprite abilitySprite;
                    if (Resources.Load<AbilityObject>("CustomData/Abilities/"+ability.Key))
                    {
                        abilitySprite = Resources.Load<AbilityObject>("CustomData/Abilities/"+ability.Key).sprite;
                    }
                    else
                    {
                        abilitySprite = Resources.Load<EvolutionObject>("CustomData/Evolutions/"+ability.Key).sprite;
                    }

                    GameObject icon = Instantiate(slot, abilities);
                    icon.transform.GetChild(0).GetComponent<Image>().sprite = abilitySprite;
                    
                    GameObject abilityLvl = new GameObject();
                    abilityLvl.transform.parent = icon.transform;
                    abilityLvl.transform.localPosition = new Vector3(0, -15, 0);
                    
                    
                    TextMeshProUGUI abilityLvlText = abilityLvl.AddComponent<TextMeshProUGUI>();
                    abilityLvlText.text = ability.Value.ToString();
                    abilityLvlText.fontSize = 18;
                    abilityLvlText.alignment = TextAlignmentOptions.Top;
                    
                    abilityLvl.GetComponent<RectTransform>().sizeDelta = new Vector2(5, 5);
                }
            }

            foreach (var damageSource in playerStat.damageDone)
            {
                GameObject typed = Instantiate(typedDamage, damage);
                typed.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = damageSource.Key;
                typed.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = damageSource.Value.ToString();
            }
        }
    }
}

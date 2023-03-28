using System.Collections.Generic;
using ScriptableObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlateformSurvivor.Menu
{
    public class CharacterSelection : MonoBehaviour
    {
        [SerializeField] private ToggleGroup characterToggleGroup;
        [SerializeField] private GameObject togglePrefab;
        [SerializeField] private Button startCharacterButton;
        [SerializeField] private Button buyButton;
        [SerializeField] private TextMeshProUGUI descText;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private PersistentDataManager persistentDataManager;
        
        private List<StatObject> characters = new();
        private void Start()
        {
            coinText.text = persistentDataManager.coins.ToString();
            
            foreach (var character in persistentDataManager.charactersUnlocked)
            {
                if (character.Value)
                {
                    characters.Add(Resources.Load<StatObject>("CustomData/PlayerStats/"+character.Key));
                }
            }

            foreach(StatObject character in characters)
            {
                GameObject instance = Instantiate(togglePrefab, characterToggleGroup.transform);
                instance.name = character.name;
                instance.GetComponentInChildren<Text>().text = character.name;
                instance.GetComponent<Toggle>().group = characterToggleGroup;
                instance.GetComponent<Toggle>().onValueChanged.AddListener(delegate { SelectCharacter(); });
            }
        }
        
        private void Update()
        {
            coinText.text = persistentDataManager.coins.ToString();
        }

        private void BuyCharacter(StatObject character)
        {
            persistentDataManager.coins -= (int)character.price;
            persistentDataManager.charactersBought.Add(character.name);
            SelectCharacter();
        }

        private bool CanBuy(StatObject character) 
        {
            if (character.price < persistentDataManager.coins)
            {
                return true;
            }
            return false;
        }

        public void SelectCharacter()
        {
            startCharacterButton.interactable = false;
            buyButton.interactable = false;
            buyButton.onClick.RemoveAllListeners();
            buyButton.gameObject.SetActive(false);

            if (characterToggleGroup.AnyTogglesOn())
            {
                string nameSelected = characterToggleGroup.GetFirstActiveToggle().name;
                StatObject characterObject = characters.Find(c => c.name == nameSelected);
                descText.text = nameSelected;
                priceText.text = characterObject.price.ToString();
                
                if (characterObject.price == 0 || persistentDataManager.charactersBought.Contains(nameSelected))
                    // Already bought
                {
                    persistentDataManager.chosenCharacter = characterObject;
                    startCharacterButton.interactable = true;
                } else if (CanBuy(characterObject))
                    // Buyable Character, can buy
                {
                    buyButton.gameObject.SetActive(true);
                    buyButton.interactable = true;
                    buyButton.onClick.AddListener(delegate { BuyCharacter(characterObject); });
                }
                else 
                    // Buyable Character, can't buy
                {
                    buyButton.gameObject.SetActive(true);
                }
            }
            else
            {
                descText.text = "";
                priceText.text = "";
            }
        }
    }
}

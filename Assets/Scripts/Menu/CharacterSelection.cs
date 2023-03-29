using System.Collections.Generic;
using System.Linq;
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
        
        private List<CharacterObject> characters = new();
        private void Start()
        {
            coinText.text = persistentDataManager.coins.ToString();

            characters = Resources.LoadAll<CharacterObject>("CustomData/Characters/").ToList();
            characters.Sort(CompareOrder);
            
            foreach(CharacterObject character in characters)
            {
                if (persistentDataManager.charactersUnlocked.Contains(character.name))
                {
                    GameObject instance = Instantiate(togglePrefab, characterToggleGroup.transform);
                    instance.name = character.name;
                    instance.GetComponentInChildren<Text>().text = character.displayName;
                    instance.GetComponent<Toggle>().group = characterToggleGroup;
                    instance.GetComponent<Toggle>().onValueChanged.AddListener(delegate { SelectCharacter(); });   
                }
            }
        }
        
        private void Update()
        {
            coinText.text = persistentDataManager.coins.ToString();
        }

        private int CompareOrder(CharacterObject c1, CharacterObject c2)
        {
            if (c1.displayOrder < c2.displayOrder)
            {
                return -1;
            }

            return 1;
        }

        private void BuyCharacter(CharacterObject character)
        {
            persistentDataManager.coins -= character.price;
            persistentDataManager.charactersBought.Add(character.name);
            SelectCharacter();
        }

        private bool CanBuy(CharacterObject character) 
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
            descText.text = "";
            priceText.text = "";

            if (characterToggleGroup.AnyTogglesOn())
            {
                string nameSelected = characterToggleGroup.GetFirstActiveToggle().name;
                CharacterObject characterObject = characters.Find(c => c.name == nameSelected);
                descText.text = characterObject.description;
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
        }
    }
}

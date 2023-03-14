using System.Collections.Generic;
using ScriptableObject;
using UnityEngine;
using UnityEngine.UI;

namespace PlateformSurvivor.Menu
{
    public class CharacterSelection : MonoBehaviour
    {
        [SerializeField] private ToggleGroup characterToggleGroup;
        [SerializeField] private GameObject togglePrefab;
        [SerializeField] private Button startCharacterButton;
        [SerializeField] private PersistentDataManager persistentDataManager;
        
        private List<StatObject> characters;
        private void Start()
        {
            characters = persistentDataManager.charactersUnlocked;
            foreach(StatObject character in characters)
            {
                GameObject instance = Instantiate(togglePrefab, characterToggleGroup.transform);
                instance.name = character.name;
                instance.GetComponentInChildren<Text>().text = character.name;
                instance.GetComponent<Toggle>().group = characterToggleGroup;
            }
        }

        public void SelectCharacter()
        {
            persistentDataManager.chosenCharacter = characters.Find(stat => stat.name == characterToggleGroup.GetFirstActiveToggle().name);
        }

        private void Update()
        {
            startCharacterButton.interactable = characterToggleGroup.AnyTogglesOn();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    public List<StatObject> characters;
    [SerializeField] ToggleGroup characterToggleGroup;
    [SerializeField] GameObject togglePrefab;
    [SerializeField] Button startCharacterButton;
    [SerializeField] PersistentDataManager persistentDataManager;
    void Start()
    {
        foreach(StatObject character in characters)
        {
            GameObject instance = Instantiate(togglePrefab, characterToggleGroup.transform);
            instance.name = character.name;
            instance.GetComponentInChildren<Text>().text = character.name;
            instance.GetComponent<Toggle>().group = characterToggleGroup;
        };
    }

    public void SelectCharacter()
    {
        persistentDataManager.chosenCharacter = characters.Find(stat => stat.name == characterToggleGroup.GetFirstActiveToggle().name);
    }

    private void Update()
    {
        if (characterToggleGroup.AnyTogglesOn())
        {
            startCharacterButton.interactable = true;
        } else
        {
            startCharacterButton.interactable = false;
        }
    }
}

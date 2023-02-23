using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuService : MonoBehaviour
{
    public List<StatObject> characters;
    [SerializeField] ToggleGroup characterToggleGroup;
    [SerializeField] SceneService sceneService;
    [SerializeField] GameObject togglePrefab;
    [SerializeField] Button startButton;
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

    public void DisplaySceneMenu()
    {
        persistentDataManager.chosenCharacter = characters.Find(stat => stat.name == characterToggleGroup.GetFirstActiveToggle().name);
        if (sceneService.scenes.Count <= 1 )
        {
            sceneService.SwapScene(sceneService.scenes[0]);
        } else
        {

        }
    }

    private void Update()
    {
        if (sceneService.GetCurrentSceneID() == 0)
        {
            if (characterToggleGroup.AnyTogglesOn())
            {
                startButton.interactable = true;
            }
            else
            {
                startButton.interactable = false;
            }
        }
    }
}

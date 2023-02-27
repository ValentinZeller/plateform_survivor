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
    [SerializeField] Button startCharacterButton;
    [SerializeField] PersistentDataManager persistentDataManager;

    [SerializeField] GameObject stageScreen;
    [SerializeField] ToggleGroup stageToggleGroup;
    [SerializeField] Button startStageButton;
    void Start()
    {
        foreach(StatObject character in characters)
        {
            GameObject instance = Instantiate(togglePrefab, characterToggleGroup.transform);
            instance.name = character.name;
            instance.GetComponentInChildren<Text>().text = character.name;
            instance.GetComponent<Toggle>().group = characterToggleGroup;
        };

        foreach(string stage in sceneService.stages)
        {
            GameObject instance = Instantiate(togglePrefab, stageToggleGroup.transform);
            instance.name = stage;
            instance.GetComponentInChildren<Text>().text = stage;
            instance.GetComponent<Toggle>().group = stageToggleGroup;
        }
    }

    public void StartGame()
    {
        sceneService.SwapScene(stageToggleGroup.GetFirstActiveToggle().name);
    }

    public void DisplayStageMenu()
    {
        persistentDataManager.chosenCharacter = characters.Find(stat => stat.name == characterToggleGroup.GetFirstActiveToggle().name);
        if (sceneService.stages.Count < 2)
        {
            sceneService.SwapScene(sceneService.stages[0]);
        } else
        {
            stageScreen.SetActive(true);
        }
    }

    private void Update()
    {
        if (sceneService.GetCurrentSceneID() == 0)
        {
            if (characterToggleGroup.AnyTogglesOn())
            {
                startCharacterButton.interactable = true;
            }
            else
            {
                startCharacterButton.interactable = false;
            }

            if (stageToggleGroup.AnyTogglesOn())
            {
                startStageButton.interactable = true;
            } else
            {
                startStageButton.interactable = false;
            }
        }
    }
}

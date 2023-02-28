using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelection : MonoBehaviour
{
    public List<string> stages;
    [SerializeField] GameObject stageScreen;
    [SerializeField] ToggleGroup stageToggleGroup;
    [SerializeField] GameObject togglePrefab;
    [SerializeField] Button startStageButton;
    [SerializeField] PersistentDataManager persistentDataManager;

    private void Start()
    {
        foreach (string stage in stages)
        {
            GameObject instance = Instantiate(togglePrefab, stageToggleGroup.transform);
            instance.name = stage;
            instance.GetComponentInChildren<Text>().text = stage;
            instance.GetComponent<Toggle>().group = stageToggleGroup;
        }
    }

    private void Update()
    {
        if (stageToggleGroup.AnyTogglesOn())
        {
            startStageButton.interactable = true;
        } else
        {
            startStageButton.interactable = false;
        }
    }

    private void LoadStage(string stageName)
    {
        SceneManager.LoadScene(stageName);
    }

    public void DisplayStageSelection()
    {
        if (stages.Count < 2)
        {
            LoadStage(stages[0]);
        } else
        {
            stageScreen.SetActive(true);
        }
    }

    public void StartGame()
    {
        LoadStage(stageToggleGroup.GetFirstActiveToggle().name);
    }
}

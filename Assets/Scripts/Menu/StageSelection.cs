using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PlateformSurvivor.Menu
{
    public class StageSelection : MonoBehaviour
    {
        [SerializeField] private GameObject stageScreen;
        [SerializeField] private ToggleGroup stageToggleGroup;
        [SerializeField] private GameObject togglePrefab;
        [SerializeField] private Button startStageButton;
        [SerializeField] private PersistentDataManager persistentDataManager;
        
        private List<string> stages = new();
        private void Start()
        {
            foreach (var stage in persistentDataManager.stagesUnlocked)
            {
                if (stage.Value)
                {
                    stages.Add(stage.Key);
                }   
            }

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

        private static void LoadStage(string stageName)
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
}

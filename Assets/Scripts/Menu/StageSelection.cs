using System.Collections.Generic;
using System.Linq;
using ScriptableObject;
using TMPro;
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
        [SerializeField] private TextMeshProUGUI descText;
        [SerializeField] private PersistentDataManager persistentDataManager;
        
        private List<StageObject> stages = new();
        private void Start()
        {
            stages = Resources.LoadAll<StageObject>("CustomData/Stages").ToList();
            stages.Sort(CompareOrder);

            foreach (StageObject stage in stages)
            {
                if (persistentDataManager.stagesUnlocked.Contains(stage.name))
                {
                    GameObject instance = Instantiate(togglePrefab, stageToggleGroup.transform);
                    instance.name = stage.name;
                    instance.GetComponentInChildren<Text>().text = stage.displayName;
                    instance.GetComponent<Toggle>().group = stageToggleGroup;
                    instance.GetComponent<Toggle>().onValueChanged.AddListener(delegate { SelectStage(); });   
                }
            }
        }

        private int CompareOrder(StageObject s1, StageObject s2)
        {
            if (s1.displayOrder < s2.displayOrder)
            {
                return -1;
            }

            return 1;
        }

        private void LoadStage(string stageName)
        {
            StageObject stageObject = stages.Find(s => s.name == stageName);
            persistentDataManager.chosenStage = stageObject;
            SceneManager.LoadScene(stageName);
        }

        public void DisplayStageSelection()
        {
            if (persistentDataManager.stagesUnlocked.Count < 2)
            {
                LoadStage(stages[0].name);
            } else
            {
                stageScreen.SetActive(true);
            }
        }

        private void SelectStage()
        {
            startStageButton.interactable = false;
            descText.text = "";
            if (stageToggleGroup.AnyTogglesOn())
            {
                string stageName = stageToggleGroup.GetFirstActiveToggle().name;
                StageObject stageObject = stages.Find(s => s.name == stageName);
                descText.text = stageObject.description;
                startStageButton.interactable = true;
            }
        }

        public void StartGame()
        {
            LoadStage(stageToggleGroup.GetFirstActiveToggle().name);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

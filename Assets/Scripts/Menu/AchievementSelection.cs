using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlateformSurvivor.Menu
{
    public class AchievementSelection : MonoBehaviour
    {
        [SerializeField] private ToggleGroup achievementToggleGroup;
        [SerializeField] private GameObject togglePrefab;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private PersistentDataManager persistentDataManager;

        private List<string> achievementList = new();

        private void Start()
        {
            foreach (var ach in persistentDataManager.achievementsUnlocked)
            {
                if (ach.Value)
                {
                    achievementList.Add(ach.Key);
                }
                
                GameObject instance = Instantiate(togglePrefab, achievementToggleGroup.transform);
                instance.name = ach.Key;
                instance.GetComponentInChildren<Text>().text = ach.Key;
                instance.GetComponent<Toggle>().group = achievementToggleGroup;
                instance.GetComponent<Toggle>().onValueChanged.AddListener(delegate { UpdateDesc(); });
            }
        }

        private void UpdateDesc()
        {
            string achievement = achievementToggleGroup.GetFirstActiveToggle().name;
            descriptionText.text = achievement;
            if (achievementList.Contains(achievement))
            {
                descriptionText.text += "Unlocked";
            }
        }
    }
}
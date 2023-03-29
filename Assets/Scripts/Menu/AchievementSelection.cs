using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObject;
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

        private List<AchievementObject> achievements = new();

        private void Start()
        {
            achievements = Resources.LoadAll<AchievementObject>("CustomData/Achievements").ToList();
            achievements.Sort(CompareOrder);

            foreach (var achievement in achievements)
            {
                GameObject instance = Instantiate(togglePrefab, achievementToggleGroup.transform);
                instance.name = achievement.name;
                instance.GetComponentInChildren<Text>().text = achievement.displayName;
                instance.GetComponent<Toggle>().group = achievementToggleGroup;
                instance.GetComponent<Toggle>().onValueChanged.AddListener(delegate { UpdateDesc(); });
            }
        }
        
        private int CompareOrder(AchievementObject a1, AchievementObject a2)
        {
            if (a1.displayOrder < a2.displayOrder)
            {
                return -1;
            }

            return 1;
        }

        private void UpdateDesc()
        {
            string achievement = achievementToggleGroup.GetFirstActiveToggle().name;
            AchievementObject achievementObject = achievements.Find(a => a.name == achievement);
            descriptionText.text = achievementObject.description;
            if (persistentDataManager.achievementsUnlocked.Contains(achievementObject.name))
            {
                descriptionText.text += "Unlocked";
            }
        }
    }
}
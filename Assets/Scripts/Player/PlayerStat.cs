using System;
using System.Collections.Generic;
using PlateformSurvivor.Menu;
using PlateformSurvivor.Service;
using ScriptableObject;
using UnityEngine;

namespace PlateformSurvivor.Player
{
    public class PlayerStat : MonoBehaviour, IDamageable
    {
        [SerializeField] private StatObject stat;

        private PersistentDataManager persistentDataManager;
        private int currentCoins;
        private float health;
        private int enemiesKilled;
        private int healthPicked;
        private int chestPicked;
        private Dictionary<string, float> baseStats = new();

        private const int ENEMY_ACHIEVEMENT = 15;
        private const int HEALTH_ACHIEVEMENT = 3;
        
        public Dictionary<string, float> currentStats = new();

        private void Awake()
        {
            if (FindObjectOfType<PersistentDataManager>())
            {
                persistentDataManager = FindObjectOfType<PersistentDataManager>();
                stat = persistentDataManager.chosenCharacter;
            }
            UnlockService.AddAbility(Enum.GetName(typeof(ActiveAbility), stat.startAbility));

            InitStat();

            health = currentStats["Health"];
            EventManager.AddListener("add_passive", OnAddPassive);
            EventManager.AddListener("got_coin", GotCoin);
            EventManager.AddListener("regen_health", RegenHealth);
            EventManager.AddListener("enemy_killed", OnKill);
            EventManager.AddListener("got_health", HealthPicked);
            EventManager.AddListener("got_chest", ChestPicked);
        }

        private void InitStat()
        {
            for (int i = 0; i < StatObject.Keys().Count; i++ )
            {
                float percent = 0;
                UpgradeObject currentUpgrade = Resources.Load<UpgradeObject>("CustomData/Upgrades/" + StatObject.Keys()[i]);
                if (currentUpgrade != null)
                {
                    percent = currentUpgrade.percentEffect;
                }
                baseStats.Add(StatObject.Keys()[i], stat[i]);
                float bonusStats = 0;
                if (persistentDataManager != null)
                {
                    bonusStats = baseStats[StatObject.Keys()[i]] * percent * persistentDataManager.statsUpgrade[StatObject.Keys()[i]];
                }
                currentStats.Add(StatObject.Keys()[i], baseStats[StatObject.Keys()[i]] + bonusStats);
            }
        }

        private void OnAddPassive(object data)
        {
            string itemName = (string)data;
            AbilityObject currentAbility = Resources.Load<AbilityObject>("CustomData/Abilities/" + itemName);
            currentStats[itemName] = currentStats[itemName] + currentStats[itemName] * currentAbility.percent *
                UnlockService.AbilitiesUnlocked[false][itemName];
        }

        private void GotCoin(object data)
        {
            currentCoins += (int)data;
            if (persistentDataManager != null)
            {
                persistentDataManager.coins++;
            }
        }

        private void OnKill()
        {
            enemiesKilled++;
            if (!persistentDataManager.HasAchievementUnlocked("Killer1") && enemiesKilled > ENEMY_ACHIEVEMENT)
            {
                persistentDataManager.UnlockAchievement(AchievementKey.Killer1);
            }
        }

        private void HealthPicked(object data)
        {
            healthPicked++;
            if (!persistentDataManager.HasAchievementUnlocked("Healing") && healthPicked > HEALTH_ACHIEVEMENT)
            {
                persistentDataManager.UnlockAchievement(AchievementKey.Healing);
            }

            float regen = (float)data;
            RegenHealth(regen);
        }

        private void ChestPicked()
        {
            chestPicked++;
        }

        private void RegenHealth(object data)
        {
            float regen = (float)data;
            health += regen;
            if (health > currentStats["Health"])
            {
                health = currentStats["Health"];
            }
        }
        
        public float GetHealth()
        {
            return health;
        }

        public int GetCoins()
        {
            return currentCoins;
        }

        public void Damage(float damage)
        {
            health--;
            if (health <= 0)
            {
                EventManager.Trigger("death");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using PlateformSurvivor.Menu;
using PlateformSurvivor.Service;
using ScriptableObject;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.SmartFormat.Utilities;

namespace PlateformSurvivor.Player
{
    public class PlayerStat : MonoBehaviour, IDamageable
    {
        [SerializeField] private CharacterObject character;
        
        private PersistentDataManager persistentDataManager;
        private int currentCoins;
        private float health;
        private int enemiesKilled;
        private int healthPicked;
        private int chestPicked;
        private float timeCount;
        private float regenTime = 1;
        private Dictionary<string, float> baseStats = new();

        private const int EnemyAchievement = 15;
        private const int HealthAchievement = 3;
        private const int SurviveAchievement = 300;
        
        public Dictionary<string, float> damageDone = new();
        public static Dictionary<string, float> currentStats = new();

        private void Start()
        {
            GetComponent<PlayerInput>().actions.FindAction("Dash").Disable();
            if (FindObjectOfType<PersistentDataManager>())
            {
                persistentDataManager = FindObjectOfType<PersistentDataManager>();
                character = persistentDataManager.chosenCharacter;
            }
            GetComponent<PlayerAbility>().AddAbilityComponent(Enum.GetName(typeof(ActiveAbility), character.startAbility));

            InitStat();

            health = currentStats["Health"];
            EventManager.AddListener("add_passive", OnAddPassive);
            EventManager.AddListener("got_coin", GotCoin);
            EventManager.AddListener("regen_health", RegenHealth);
            EventManager.AddListener("enemy_killed", OnKill);
            EventManager.AddListener("got_health", HealthPicked);
            EventManager.AddListener("got_chest", ChestPicked);
            EventManager.AddListener("damage_done", DamageDone);
        }

        private void InitStat()
        {
            for (int i = 0; i < CharacterObject.Keys().Count; i++ )
            {
                float percent = 0;
                UpgradeObject currentUpgrade = Resources.Load<UpgradeObject>("CustomData/Upgrades/" + CharacterObject.Keys()[i]);
                if (currentUpgrade != null)
                {
                    percent = currentUpgrade.percentEffect;
                }
                baseStats.Add(CharacterObject.Keys()[i], character[i]);
                float bonusStats = 0;
                if (persistentDataManager != null)
                {
                    if (CharacterObject.Keys()[i] == "Regen")
                    {
                        bonusStats = percent * persistentDataManager.statsUpgrade[CharacterObject.Keys()[i]];
                    }
                    else
                    {
                        bonusStats = baseStats[CharacterObject.Keys()[i]] * percent * persistentDataManager.statsUpgrade[CharacterObject.Keys()[i]];
                    }
                }
                currentStats.Add(CharacterObject.Keys()[i], baseStats[CharacterObject.Keys()[i]] + bonusStats);
            }
        }

        private void Update()
        {
            timeCount += Time.deltaTime;
            if (timeCount > SurviveAchievement)
            {
                if (persistentDataManager != null && persistentDataManager.chosenStage.name == "Stage1" && !persistentDataManager.HasAchievementUnlocked(AchievementKey.SurviveStage1.ToString()))
                {
                    persistentDataManager.UnlockAchievement(AchievementKey.SurviveStage1);
                }
            }

            if (Time.time >= regenTime && currentStats["Regen"] > 0)
            {
                RegenHealth(0f);
                regenTime++;
            }
        }

        private void OnAddPassive(object data)
        {
            string itemName = (string)data;
            AbilityObject currentAbility = Resources.Load<AbilityObject>("CustomData/Abilities/" + itemName);
            currentStats[itemName] = currentStats[itemName] + currentStats[itemName] * currentAbility.percent *
                PlayerAbility.Abilities[false][itemName];
        }

        private void GotCoin(object data)
        {
            int coins = (int)data;
            coins += Mathf.FloorToInt(coins * currentStats["CoinRate"]);
            currentCoins += coins;
            if (persistentDataManager != null)
            {
                persistentDataManager.coins += coins;
            }
            EventManager.Trigger("update_coin");
        }

        private void OnKill()
        {
            enemiesKilled++;
            if (persistentDataManager != null && !persistentDataManager.HasAchievementUnlocked(AchievementKey.Killer1.ToString()) && enemiesKilled > EnemyAchievement)
            {
                persistentDataManager.UnlockAchievement(AchievementKey.Killer1);
            }

            EventManager.Trigger("update_kill");
        }

        private void HealthPicked(object data)
        {
            healthPicked++;
            if (persistentDataManager != null && !persistentDataManager.HasAchievementUnlocked(AchievementKey.Healing.ToString()) && healthPicked > HealthAchievement)
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
            health += regen + currentStats["Regen"];
            if (health > currentStats["Health"])
            {
                health = currentStats["Health"];
            }
            EventManager.Trigger("update_health");
        }

        private void DamageDone(object data)
        {
            TypedDamage damage = (TypedDamage)data;
            if (damageDone.ContainsKey(damage.type))
            {
                damageDone[damage.type] += damage.value;
            }
            else
            {
                damageDone.Add(damage.type, damage.value);
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

        public int GetKills()
        {
            return enemiesKilled;
        }

        public float GetTime()
        {
            return timeCount;
        }

        public void Damage(float damage)
        {
            health -= damage - currentStats["Protection"];
            EventManager.Trigger("update_health");
            if (health <= 0)
            {
                EventManager.Trigger("death");
            }
        }
    }
}

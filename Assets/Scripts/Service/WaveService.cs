using System.Collections;
using System.Collections.Generic;
using PlateformSurvivor.Menu;
using ScriptableObject;
using UnityEngine;

namespace PlateformSurvivor.Service
{
    public class WaveService : MonoBehaviour
    {
        [SerializeField] private List<WaveObject> waves;
        [SerializeField] private Transform player;
        private const float MaxEnemies = 40;
        private const int PlayerRadius = 8;
        
        private const float MinX = -35;
        private const float MaxX = 31;
        private const float MinY = -4;
        private const float MaxY = 30;

        private static readonly Vector3 BossPos = new Vector3(0, -3, 0);

        private float waveDuration = 60f; //second
        private int waveIndex;
        private float waveDurationCounter;
        private float currentEnemies;

        private PersistentDataManager persistentDataManager;
        void Start()
        {
            if (FindObjectOfType<PersistentDataManager>())
            {
                persistentDataManager = FindObjectOfType<PersistentDataManager>();
                waves = persistentDataManager.chosenStage.waves;
                waveDuration = persistentDataManager.chosenStage.waveDurationSecond;
            }
            StartCoroutine(nameof(Spawn));
        }
        
        void Update()
        {
            waveDurationCounter += Time.deltaTime;
            if (waveDurationCounter >= waveDuration && waveIndex < waves.Count)
            {
                waveDurationCounter = 0f;
                waveIndex++;
                EventManager.Trigger("reload_block");
            }

            currentEnemies = transform.childCount;
        }

        private Vector3 GenerateRandomSpawn()
        {
            Vector3 randomSpawn;
            do
            {
                float randomSpawnX = Random.Range(MinX, MaxX);
                float randomSpawnY = Random.Range(MinY, MaxY);
                randomSpawn = new Vector3(randomSpawnX, randomSpawnY, 0);
            } while (Physics.OverlapSphere(randomSpawn, 2).Length > 0 || Vector3.Distance(randomSpawn, player.position) < PlayerRadius);

            return randomSpawn;
        }

        private IEnumerator Spawn()
        {
            if (currentEnemies < MaxEnemies && !waves[waveIndex].isBossWave) { 
                List<Vector3> spawns = new();

                int spawnCount = (currentEnemies < waves[waveIndex].minCount) ? waves[waveIndex].minCount : waves[waveIndex].enemy.Count;
                for (int i = 0; i < spawnCount; i++)
                {
                    spawns.Add(GenerateRandomSpawn());
                }
 
                for (int i = 0; i < spawns.Count; i++)
                {
                    Instantiate(waves[waveIndex].enemy[i % waves[waveIndex].enemy.Count], spawns[i], Quaternion.identity, transform);
                }
            }

            if (waves[waveIndex].isBossWave)
            {
                Instantiate(waves[waveIndex].enemy[0], BossPos, Quaternion.identity, transform);
            }

            yield return new WaitForSeconds(waves[waveIndex].spawnRate);
            StartCoroutine(nameof(Spawn));
        }
    }
}

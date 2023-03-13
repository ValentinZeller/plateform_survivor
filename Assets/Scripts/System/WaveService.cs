using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveService : MonoBehaviour
{
    //private float spawnCooldown = 5f;
    private int waveIndex = 0;
    private float waveDuration = 60f; //second
    private float waveDurationCounter = 0f;
    private float maxEnemies = 20;
    private float currentEnemies = 0;

    private float minX = -35;
    private float maxX = 31;
    private float minY = -4;
    private float maxY = 30;

    [SerializeField] List<WaveObject> waves;
    [SerializeField] Transform player;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
        waveDurationCounter += Time.deltaTime;
        if (waveDurationCounter >= waveDuration)
        {
            waveDurationCounter = 0f;
            waveIndex++;
            if (waveIndex > waves.Count)
            {
                waveIndex = waves.Count;
            }
        }
    }

    private Vector3 GenerateRandomSpawn()
    {
        Vector3 randomSpawn = new();
        do
        {
            float randomSpawnX = Random.Range(minX, maxX);
            float randomSpawnY = Random.Range(minY, maxY);
            randomSpawn = new Vector3(randomSpawnX, randomSpawnY, 0);
        } while (Physics.OverlapSphere(randomSpawn, 2).Length > 0);

        return randomSpawn;
    }

    private IEnumerator Spawn()
    {
        if (currentEnemies < maxEnemies && !waves[waveIndex].isBossWave) { 
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
            Instantiate(waves[waveIndex].enemy[0], GenerateRandomSpawn(), Quaternion.identity, transform);
        }

        yield return new WaitForSeconds(waves[waveIndex].spawnRate);
        StartCoroutine("Spawn");
    }
}

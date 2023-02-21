using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveService : MonoBehaviour
{
    //private float spawnCooldown = 5f;
    private int waveIndex = 0;
    private float waveDuration = 30f; //second
    private float waveDurationCounter = 0f;

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

    private IEnumerator Spawn()
    {
        List<Vector3> spawns = new();
        for (int i = 0; i < waves[waveIndex].count; i++)
        {
            float randomSpawnX = Random.Range(minX, maxX);
            float randomSpawnY = Random.Range(minY, maxY);

            Vector3 randomSpawn = new Vector3(randomSpawnX, randomSpawnY, 0);
            if (Physics.OverlapSphere(randomSpawn, 2).Length > 0)
            {
                do
                {
                    randomSpawnX = Random.Range(minX, maxX);
                    randomSpawnY = Random.Range(minY, maxY);
                    randomSpawn = new Vector3(randomSpawnX, randomSpawnY, 0);
                } while (Physics.OverlapSphere(randomSpawn, 2).Length > 0);
            }

            spawns.Add(randomSpawn);
        }
 
        for (int i = 0; i < spawns.Count; i++)
        {
            Instantiate(waves[waveIndex].enemy, spawns[i], Quaternion.identity);
        }
        
        yield return new WaitForSeconds(waves[waveIndex].waitTime);
        StartCoroutine("Spawn");
    }
}

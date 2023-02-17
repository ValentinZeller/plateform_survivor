using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveService : MonoBehaviour
{
    private float spawnCooldown = 5f;

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform player;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Spawn()
    {
        float randomSpawnX = Random.Range(player.position.x - 10, player.position.x + 11);
        if (Mathf.Abs(randomSpawnX - player.position.x) <= 1)
        {
            randomSpawnX *= 2;
        }
        Vector3 randomSpawn = new Vector3(randomSpawnX, 0.5f, 0);
        Instantiate(enemyPrefab, randomSpawn, Quaternion.identity);
        yield return new WaitForSeconds(spawnCooldown);
        StartCoroutine("Spawn");
    }
}

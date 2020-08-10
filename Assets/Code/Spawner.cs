using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float time;
    public float minSpawnDelay;
    public float maxSpawnDelay;

    public float spawnRadius;

    public GameObject[] spawnables;

    public float destoryDelay; //if 0 dont destory

    void Start()
    {
        time = Random.Range(minSpawnDelay, maxSpawnDelay);
    }


    void Update()
    {
        time -= Time.deltaTime;

        if (time <= 0)
        {
            var obj = Instantiate(spawnables[Random.Range(0, spawnables.Length)], transform.position + Random.insideUnitSphere * spawnRadius, transform.rotation);
            if (destoryDelay != 0)
            {
                obj.AddComponent<SelfDestruct>();
                obj.GetComponent<SelfDestruct>().destroyDelay = destoryDelay;
            }

            time = Random.Range(minSpawnDelay, maxSpawnDelay);
        }
    }
}


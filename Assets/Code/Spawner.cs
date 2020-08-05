using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private float time;
    public float spawnDelay;

    public float spawnRadius;

    public GameObject[] spawnables;

    void Start()
    {
        
    }


    void Update()
    {
        time = time + 1f * Time.deltaTime;

        if (time >= spawnDelay)
        {
            Instantiate(spawnables[Random.Range(0, spawnables.Length)], transform.position + Random.insideUnitSphere * spawnRadius, transform.rotation);
            time = 0;
        }
    }
}


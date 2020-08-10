using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    private float time;
    public float destroyDelay;
    
    void Start()
    {
        time = destroyDelay;
    }

    void Update()
    {
        time -= Time.deltaTime;

        if (time <= 0)
            Destroy(gameObject);
    }
}

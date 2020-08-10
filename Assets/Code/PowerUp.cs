using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float speed;
    
    void Start()
    {
        
    }

    void Update()
    {
        var move = Mathf.Sin(Time.time) * Time.deltaTime * speed;


        transform.Translate(0, move, 0, Space.World); //move the item according to the sine wave
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCamera : MonoBehaviour
{
    [Header("Player Stats")]
    public Vector3 offset;
    public Transform target;

    [Header("Internal Variables")]
    public float smoothTime;
    private Vector3 velocity;
    private Vector3 center;

    void Start()
    {
        
    }

    void Update()
    {
        center = Vector3.SmoothDamp(center, target.position + offset, ref velocity, smoothTime);
        transform.position = center;
    }
}

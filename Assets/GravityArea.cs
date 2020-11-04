using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityArea : MonoBehaviour
{
    public List<Rigidbody> rigidBodys = new List<Rigidbody>();

    public Vector3 pullDirection;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     foreach (Rigidbody rb in rigidBodys)
        {
            rb.AddForce(pullDirection * Physics.gravity.y);
        }   
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rigidBodys.Add(rb);
            rb.useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rigidBodys.Remove(rb);
            rb.useGravity = true;
        }
    }
}

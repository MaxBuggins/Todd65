using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePiece : MonoBehaviour
{
    [Header("Pipe")]
    public float suckForce;

    public Transform nextPipe;
    private Vector3 nextPipePos;

    public List<Rigidbody> rbs; //list of all rigidbodys now being sucked through pipe

    private void Start()
    {
        if (nextPipe == null) //if nowhere to go then dont bother with triggers and this script
        {
            foreach (Collider collider in GetComponents<Collider>())
            {
                if (collider.isTrigger == true)
                    collider.enabled = false;
            }
            enabled = false;
        }

        else
            nextPipePos = nextPipe.position;
    }

    void Update()
    {
       foreach (Rigidbody rb in rbs)
        {
            var offset = nextPipePos - rb.gameObject.transform.position;
            rb.AddForce(offset * suckForce);
            //rb.mass = Mathf.Sqrt(rb.mass);
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        var rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rbs.Add(rb);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        var rb = other.GetComponentInParent<Rigidbody>();

        if (rb != null) //makes sure object can be sucked
        {
            rb.useGravity = true;
            rbs.Remove(rb);
        }
    }
}

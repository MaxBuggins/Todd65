using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePiece : MonoBehaviour
{
    [Header("Pipe")]
    public float suckForce;

    public Transform nextPipe;
    private Vector3 nextPipePos;

    public bool isEnd;

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
            if (rb == null)
            {
                rbs.Remove(rb);
                return;
            }
            var offset = nextPipePos - rb.gameObject.transform.position;
            rb.useGravity = false;
            rb.AddForce(offset * suckForce);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rbs.Add(rb);
            rb.constraints = RigidbodyConstraints.None;
        }

        var player = other.GetComponent<Player>(); //makes the player suck so its not fixed
        if (player != null)
            player.sucked = true;
    }

    private void OnTriggerExit(Collider other)
    {
        var rb = other.GetComponentInParent<Rigidbody>();

        if (rb != null) //makes sure object can be sucked
        {
            rb.useGravity = true;
            rbs.Remove(rb);
        }

        var player = other.GetComponent<Player>(); //makes the player no longer suck
        if (player != null)
        {
            player.sucked = false;
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 1);
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
        }
    }
}

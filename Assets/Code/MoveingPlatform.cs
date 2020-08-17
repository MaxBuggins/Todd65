using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveingPlatform : MonoBehaviour
{
    public float time;
    [Header("Variables")]
    public bool pushOff = false;
    public float moveSpeed = 5f;
    public float stopDelay = 0.5f;
    public bool moveing = false;


    [Header("Unity Things")]
    private Vector3 startPosition;
    public Vector3 endPosition;
    private Vector3 nextPosition;

    void Start()
    {
        startPosition = transform.position;
        endPosition += startPosition; //makes the next postion relative to the start position
        nextPosition = endPosition;
    }

    void FixedUpdate()
    {
        if (moveing == false) //for when the object stops
        {
            time -= Time.deltaTime;

            if (time < 0)
                moveing = true;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

        if (transform.position == nextPosition) //stops movement if position is met
        {
            moveing = false;
            time = stopDelay;

            if (nextPosition == startPosition)
                nextPosition = endPosition;
            else
                nextPosition = startPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<Player>();

        if (player != null && pushOff == true)
        {
            player.Dead();
        }

        if (other.gameObject.GetComponent<Rigidbody>() != null)
        {
                other.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.gameObject.GetComponent<Rigidbody>() != null)
        {
            other.transform.parent = null;
        }
    }
}

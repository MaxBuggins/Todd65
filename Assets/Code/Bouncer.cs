﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    public float bounceForce = 2;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            animator.Play("Base Layer.Bounce");

            rb.velocity = Vector3.zero;

            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                if (player.lastBounceTimeStamp < Time.time - 0.1f)
                    rb.AddForce(transform.forward * bounceForce * player.gravityMultiplyer, ForceMode.VelocityChange);
                player.lastBounceTimeStamp = Time.time;
            }
            else
                rb.AddForce(transform.forward * bounceForce, ForceMode.VelocityChange);
        }
    }
}

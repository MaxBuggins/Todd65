using JetBrains.Annotations;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Camera))]
public class SharedCamera : MonoBehaviour
{
    public static SharedCamera instance;

    [Header("Camera Chacrchteristics")]
    public bool equalShare = true;
    public bool perspective = true;
    public List<Transform> targets;
    public Vector2 offset;
    private Vector3 defultPos;
    public float turnSpeed = 90f;

    public enum Axis{x, y, z}
    public Axis axis;
    public bool posative;

    [Header("Follow Settings")]

    Vector3 center;
    public float smoothTime;
    private Vector3 velocity;

    [Header("Zoom Settings")]
    public float zoomSpeed = 1;
    private float zoom;

    public float zoomOffset;

    public float minZoom;
    public float maxZoom;
    public GameRotationChange currentZone;

    [Header("Unity Things")]
    private Camera cam;

    private void Start()
    {
        instance = this;
        cam = GetComponent<Camera>();
    }

    private void LateUpdate() //camera updates after everything has moved
    {
        if (targets.Count == 0) //no targets to follow
            return;

        float playersTotalAngle = 0;

        if (targets.Count != 0)
        {
            Vector3 angles = transform.eulerAngles;
            
            foreach (Transform player in targets)
            {
                playersTotalAngle += player.GetComponent<Player>().currentZone.transform.eulerAngles.y;
            }
            float desiredAngle = (playersTotalAngle) / targets.Count + 180; //averages angle inbetween players


            angles.y = Mathf.MoveTowardsAngle(angles.y, desiredAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = angles;
        }

        defultPos = offset.x * Vector3.up - offset.y * transform.forward; //applys the offset to camera

        //moves the camera
        Vector3 centerpoint = GetCenterPoint();
        if(equalShare == false)
            centerpoint = GetFrontTarget();


        center = Vector3.SmoothDamp(center, centerpoint + defultPos, ref velocity, smoothTime);
        transform.position = center;

        //zooms the camera
        if (perspective == true)
        {
            var tan = Mathf.Tan(cam.fieldOfView / 2);

            var idealZoom = ((GetGreatestDistance() / 2) / tan) * zoomOffset;

            zoom = Mathf.MoveTowards(zoom, idealZoom, Time.deltaTime * zoomSpeed);
            transform.position += Vector3.forward * zoom;
        }

        else
        {
            var idealZoom = ((GetGreatestDistance() / 2) + zoomOffset);
            zoom = Mathf.MoveTowards(zoom, idealZoom, Time.deltaTime * zoomSpeed);
            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
            cam.orthographicSize = zoom;
        }
    }

    Vector3 GetCenterPoint() //gets the center point for the camera to look at
    {
        if (targets.Count == 1)
            return targets[0].position; //if only one player is left

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }

    float GetGreatestDistance() //gets the distance between the two furthest targets
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }

    Vector3 GetFrontTarget()
    {
        var furthestTarget = targets[0];
        foreach (Transform target in targets)
        {
            if (target.position.x > furthestTarget.position.x)
                furthestTarget = target;
        }
        return (furthestTarget.position);
    }
}

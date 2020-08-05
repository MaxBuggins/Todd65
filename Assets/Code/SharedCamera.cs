using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Camera))]
public class SharedCamera : MonoBehaviour
{
    [Header("Camera Chacrchteristics")]
    public List<Transform> targets;
    public Vector3 defultPos;

    public enum Axis{x, y, z}
    public Axis axis;
    public bool posative;

    public float smoothTime;
    private Vector3 velocity;

    public float zoomOffset;
    public float minZoom;
    public float maxZoom;

    [Header("Unity Things")]
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate() //camera updates after everything has moved
    {
        if (targets.Count == 0) //no targets to follow
            return;
        
        //moves the camera
        Vector3 centerpoint = GetCenterPoint();
        transform.position = Vector3.SmoothDamp(transform.position, centerpoint + defultPos, ref velocity, smoothTime);

        //zooms the camera
        print(GetGreatestDistance());

        var tan = Mathf.Tan(cam.fieldOfView / 2);

        var zoom = ((GetGreatestDistance() / 2) / tan) * zoomOffset;

        transform.position += Vector3.forward * Mathf.Lerp(minZoom, maxZoom, zoom);

        print(transform.position.z);

        //float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomAdjustment);
        //cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
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
}

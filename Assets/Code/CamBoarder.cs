using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBoarder : MonoBehaviour
{
    public bool vertical = false;
    public float smoothTime;

    public Vector3 offset;

    private Camera cam;
    private SharedCamera camShare;

    void Awake()
    {
        cam = Camera.main;
        camShare = cam.GetComponent<SharedCamera>();
        offset = transform.position;
    }

    void Update()
    {
        var newPos = Vector3.zero;

        if (vertical == false)
            newPos = new Vector3(cam.transform.position.x - (cam.orthographicSize * 2.1f), cam.transform.position.y, offset.z);
        if (vertical == true)
            newPos = new Vector3(cam.transform.position.x, cam.transform.position.y - (cam.orthographicSize * 1.3f), offset.z);

        transform.position = newPos;
        
    }
}

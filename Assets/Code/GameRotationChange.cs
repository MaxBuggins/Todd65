using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRotationChange : MonoBehaviour
{
    [Header("!ONLY use Position Constraints!")]
    public Rotation playerPos; //ONLY use Position Constraints or it will proberly break
    public bool customLockPos = false;
    public float lockPos; //for locking the players to the same position relative to rotation

    public Quaternion camRot;

    private SharedCamera sharedCam;

    private void Start()
    {
        sharedCam = FindObjectOfType<SharedCamera>();

        //so i don't have to manualy put it in, yeah im kinda efficent

        if (customLockPos == false)
        {
            if (playerPos == Rotation.z)
                lockPos = transform.position.z;

            if (playerPos == Rotation.x)
                lockPos = transform.position.x;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        Player player = other.GetComponent<Player>();

        //if (player.sucked == true) //does not gett effected if being sucked
            //return;

        player.changeOritation(playerPos, lockPos);
        player.currentZone = this;

        if (SharedCamera.instance.currentZone == this)
            return;

        SharedCamera.instance.currentZone = this;
    }
}

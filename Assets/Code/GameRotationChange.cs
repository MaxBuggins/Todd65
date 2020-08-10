using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRotationChange : MonoBehaviour
{
    [Header("!ONLY use Position Constraints!")]
    public Rotation rotation; //ONLY use Position Constraints or it will proberly break

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;

        other.GetComponent<Player>().changeOritation(rotation);
    }
}

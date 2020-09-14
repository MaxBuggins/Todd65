using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    public Transform[] childPieces;
    public List<Transform> childNodeList = new List<Transform>();

    [System.Serializable]
    public class RouteInsert
    {
        public Route route;
        public int startPos;
        public MoveDirection moveDirection;
    }
    public RouteInsert[] nextRoutes;


    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        FillNodes();

        for(int i = 0; i < childNodeList.Count; i++)
        {
            Vector3 currentPos = childNodeList[i].position;
            if(i > 0)
            {
                Vector3 previousPos = childNodeList[i - 1].position;
                Gizmos.DrawLine(previousPos, currentPos);
            }
        }
    }


    void FillNodes()
    {
        childNodeList.Clear();
        childPieces = GetComponentsInChildren<Transform>();

        foreach(Transform child in childPieces)
        {
            if(child != this.transform)
            {
                childNodeList.Add(child);
            }
        }
    }
}

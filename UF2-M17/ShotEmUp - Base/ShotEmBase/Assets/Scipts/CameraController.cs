using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 m_PosToMove;

    public void MoveAway(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}

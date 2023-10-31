using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public void MoveAway(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_1 : MonoBehaviour
{
    Vector3 m_NewPosition = new Vector3(0, 0, -10);

    [SerializeField]
    private GameEventVector3 m_CameraChange;

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            m_CameraChange.Raise(m_NewPosition);
        }

    }
}

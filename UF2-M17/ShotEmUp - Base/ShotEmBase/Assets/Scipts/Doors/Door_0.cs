using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_0 : MonoBehaviour
{
    [SerializeField]
    private GameEventVector3 m_CameraChange;

    Vector3 m_NewPosition = new Vector3(16.11f, 0, -10);

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            m_CameraChange.Raise(m_NewPosition);
        }

    }

}

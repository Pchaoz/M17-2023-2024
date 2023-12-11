using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArea : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Parent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("HE DETECTADO AL PLAYER");
            m_Parent.GetComponent<Enemy>().SetTarget(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_Parent.GetComponent<Enemy>().SetTarget(null);
        }
    }
}

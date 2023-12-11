using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableArea : MonoBehaviour
{
    [SerializeField]
    GameObject m_Parent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            m_Parent.GetComponent<Enemy>().ShootTarget(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            m_Parent.GetComponent<Enemy>().ShootTarget(false);
    }
}

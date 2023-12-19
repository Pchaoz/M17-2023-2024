using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableArea : MonoBehaviour
{
    private bool m_OnRange;

    public bool CanShoot => m_OnRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            m_OnRange = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            m_OnRange = false;
    }
}

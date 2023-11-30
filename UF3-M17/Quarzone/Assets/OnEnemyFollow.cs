using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnemyFollow : MonoBehaviour
{
    [SerializeField]
    EnemyController m_enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("TE ESTOY VIENDO JOSE");
            m_enemy.OnPlayerFollow(other.gameObject.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            Debug.Log("c fue");
            m_enemy.OnPlayerOutRange();
        }
    }
}

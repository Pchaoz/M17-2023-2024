using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnemyAttack : MonoBehaviour
{

    [SerializeField]
    EnemyController m_enemy;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_enemy.OnPlayerAttack(other.gameObject.transform);
        }
    }
}

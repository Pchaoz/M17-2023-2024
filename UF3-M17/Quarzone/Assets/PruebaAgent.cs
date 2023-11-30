using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PruebaAgent : MonoBehaviour
{
    [SerializeField]
    Transform m_Destino;

    void Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = m_Destino.position;
    }

}

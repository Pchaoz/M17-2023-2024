using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        IDLE,
        PATROL,
        FOLLOW,
        SHOOT
    }
    [SerializeField]
    private EnemyState m_CurrentState;

    private void ChangeState(EnemyState newState)
    {
        //Debug.Log(newState);

        if (newState == m_CurrentState)
            return;

        ExitState();
        //Debug.Log(newState);
        InitState(newState);
    }
    private void InitState(EnemyState currentState)
    {
        m_CurrentState = currentState;

        switch (m_CurrentState)
        {
            case EnemyState.IDLE:
                if (m_DetectionArea.Target != null) 
                    ChangeState(EnemyState.FOLLOW);
                else if (m_DetectionArea.Target == null)
                    ChangeState(EnemyState.PATROL); 
                break;
            case EnemyState.PATROL:
                break;
            case EnemyState.FOLLOW:
                break;
            case EnemyState.SHOOT:
                m_Rb.velocity = Vector3.zero;
                StartCoroutine(m_ShootCoorutine);
                break;
        }
    }

    private void UpdateState()
    {
        switch (m_CurrentState)
        {
            case EnemyState.IDLE:
                break;
            case EnemyState.PATROL:

                if (m_DetectionArea.Target != null)
                    ChangeState(EnemyState.FOLLOW);

                Patrol();                    
                break;
            case EnemyState.FOLLOW:

                if (m_DetectionArea.Target == null)
                    ChangeState(EnemyState.PATROL);

                if (m_ShootArea.CanShoot)
                    ChangeState(EnemyState.SHOOT);

                Follow();
                break;
            case EnemyState.SHOOT:

                if (m_DetectionArea.Target == null) { }
                    ChangeState(EnemyState.IDLE);

                if (!m_ShootArea.CanShoot)
                    ChangeState(EnemyState.IDLE);

                transform.LookAt(m_DetectionArea.Target.transform);
                break;
        }
    }

    private void ExitState()
    {
        switch (m_CurrentState)
        {
            case EnemyState.IDLE:
                break;
            case EnemyState.PATROL:
                break;
            case EnemyState.FOLLOW:
                break;
            case EnemyState.SHOOT:
                StopCoroutine(m_ShootCoorutine);
                break;
        }
    }

    ///--------------------------------------------------------------------\\\

    [Header("Customizable IA stats")]
    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private float m_ShootCD;

    private Rigidbody m_Rb;
    private DetectionArea m_DetectionArea;
    private ShootableArea m_ShootArea;
    private NavMeshAgent m_Agent;

    [SerializeField]
    private List<Transform> m_Nodes;
    [SerializeField]
    private Transform m_ActiveNode;

    private IEnumerator m_ShootCoorutine;

    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Rb = GetComponent<Rigidbody>();
        m_ShootArea = GetComponentInChildren<ShootableArea>();
        m_DetectionArea = GetComponentInChildren<DetectionArea>();
        m_ShootCoorutine = Shoot();
        InitState(EnemyState.PATROL);
    }

    private void Update()
    {
        UpdateState();
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            //DISPARA CON LA POSIBILIDAD DE FALLAR EL DISPARO
            
            yield return new WaitForSeconds(m_ShootCD);
        }
    }
    private void Patrol()
    {
        if (m_ActiveNode == null || Vector3.Distance(transform.position, m_ActiveNode.position) < 1)
        {
            int nodeToGet = Random.Range(0, m_Nodes.Count);
            m_ActiveNode = m_Nodes[nodeToGet];
        }
        m_Agent.SetDestination(m_ActiveNode.position);
    }
    private void Follow()
    {
        if (m_DetectionArea.Target != null)
        {
            transform.LookAt(m_DetectionArea.Target.transform);
            m_Agent.SetDestination(m_DetectionArea.Target.transform.position);
        }
    }
}

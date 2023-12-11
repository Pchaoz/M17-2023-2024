using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

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
                if (m_Target == null)
                    ChangeState(EnemyState.PATROL);
                else
                    ChangeState(EnemyState.FOLLOW);
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

                //SE MUEVE EN EL NAVMESH
                break;
            case EnemyState.FOLLOW:

                transform.position = Vector3.MoveTowards(transform.position, m_Target.transform.position, m_Speed * Time.deltaTime);
                transform.LookAt(m_Target.transform);
                break;
            case EnemyState.SHOOT:
                transform.LookAt(m_Target.transform);
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
    private GameObject m_Target;

    private IEnumerator m_ShootCoorutine;

    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_ShootCoorutine = Shoot();
        InitState(EnemyState.IDLE);
    }

    private void Update()
    {
        UpdateState();
    }

    public void SetTarget(GameObject target)
    {
        m_Target = target;
        if (target == null)
            ChangeState(EnemyState.PATROL);
        else
            ChangeState(EnemyState.FOLLOW);
    }
    public void ShootTarget(bool inArea)
    {
        if (m_Target != null || inArea)
            ChangeState(EnemyState.SHOOT);

        if (!inArea)
            ChangeState(EnemyState.IDLE);
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            //DISPARA CON LA POSIBILIDAD DE FALLAR EL DISPARO
            
            yield return new WaitForSeconds(m_ShootCD);
        }
    }
}

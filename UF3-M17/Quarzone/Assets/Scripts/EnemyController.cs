using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.VersionControl.Asset;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    float m_ShootCD = 3f;
    [SerializeField]
    float m_AwaitingOrdersTime = 2f;

    [SerializeField]
    float m_Hp = 20;

    private enum States { IDLE, PATROL, TRACK, ATTACK };
    private States m_CurrentState;
    private States m_LastState;
    private float m_StateDeltaTime;

    private Coroutine m_ShootCoorutine;
    private Coroutine m_PatrolCoroutine;
    private Transform m_Target;

    [SerializeField]
    private List<Transform> m_Waypoints;
    private Vector3 m_CurrentWaypoint;

    private NavMeshAgent m_Agent;

    private GameManager m_GameManager;

    private void Awake()
    {
        m_GameManager = GameManager.Instance;
    }

    private void Start()
    {
        m_Waypoints = m_GameManager.GetWaypoints();
        m_Agent = GetComponent<NavMeshAgent>();
        ChangeState(States.IDLE);
    }


    void Update()
    {
        //Debug.Log(m_CurrentState);
        UpdateState(m_CurrentState);
    }

    private void ChangeState(States newState)
    {

        if (newState == m_CurrentState)
            return;

        ExitState(m_CurrentState);
        InitState(newState);

    }

    private void InitState(States initState)
    {
        m_CurrentState = initState;
        m_StateDeltaTime = 0;

        switch (m_CurrentState)
        {
            case States.IDLE:
                m_PatrolCoroutine = StartCoroutine(Patrol());
                break;
            case States.PATROL:
                break;
            case States.TRACK:
                break;
            case States.ATTACK:
                m_ShootCoorutine = StartCoroutine(Shoot());
                break;
        }

    }

    private void UpdateState(States updateState)
    {
        m_StateDeltaTime += Time.deltaTime;

        switch (updateState)
        {
            case States.IDLE:
                if (m_PatrolCoroutine == null)
                    m_PatrolCoroutine = StartCoroutine(Patrol());
                break;
            case States.ATTACK:
                transform.LookAt(m_Target);
                break;
            case States.PATROL:       
                if(m_CurrentWaypoint == Vector3.zero) //Vector3 no puede ser null ni aun que lo inicialices en el start, siempre lo inicializa a 000
                {
                    Vector3 targetDestination = m_Waypoints[Random.Range(0, m_Waypoints.Count)].position;
                    m_Agent.destination = targetDestination;
                    m_CurrentWaypoint = targetDestination;
                }else
                {
                    if(m_LastState == States.TRACK || m_LastState == States.ATTACK)
                    {
                        m_LastState = States.IDLE;
                        m_Agent.destination = m_CurrentWaypoint;
                    }                  
                }
                break;
            default:
                break;

        }
    }

    private void ExitState(States exitState)
    {
        switch (exitState)
        {
            case States.IDLE:
                break;
            case States.PATROL:
                StopCoroutine(m_PatrolCoroutine);
                break;
            case States.TRACK:
                m_LastState = States.ATTACK;
                break;
            case States.ATTACK:
                StopCoroutine(m_ShootCoorutine);
                m_LastState = States.ATTACK;
                break;
            default:
                break;
        }

    }

    private IEnumerator Shoot()
    {
        transform.LookAt(m_Target);

        RaycastHit hit;
        for (int i = 0; i < 8; i++)
        {
            float rightDispersion = Random.Range(-.1f, 0.1f);
            float upDispersion = Random.Range(-.1f, 0.1f);

            Vector3 direction = transform.forward + rightDispersion * transform.right + upDispersion * transform.up;
            if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity))
            {
                Debug.DrawLine(transform.position, hit.point, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), 2f);
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    Debug.Log("Odiwi te he pillao");
                }
            }
            //Debug.DrawLine(transform.position, m_Target.position, Color.red, 2f);
        }
        yield return new WaitForSeconds(m_ShootCD);
        ChangeState(States.TRACK);
    }

    private IEnumerator Patrol()
    {
        while (true)
        {
            //ChangeState(States.IDLE);
            yield return new WaitForSeconds(m_AwaitingOrdersTime);
            ChangeState(States.PATROL);
        }
    }

    public void OnPlayerFollow(Transform playerTransform)
    {
        m_Target = playerTransform;
        ChangeState(States.TRACK);
    }

    public void OnPlayerAttack(Transform playerTransform)
    {
        m_Target = playerTransform;
        ChangeState(States.ATTACK);
    }

    public void OnPlayerOutRange ()
    {
        ChangeState(States.PATROL);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Waypoint"))
        {
            ChangeState(States.IDLE);
            m_CurrentWaypoint = Vector3.zero;
        }
    }


}

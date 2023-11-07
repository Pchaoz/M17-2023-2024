using System;
using UnityEngine;

public class EnemyMelee : MonoBehaviour, CanDie //INTERFAZ PARA QUE LOS ENEMIGOS TENGAN EL MISMO DELEGADO DE MUERTE
{
    //STATE MACHINE STUFF

    //STATES
    private enum States { NONE, RUN, IDLE, FOLLOW, HIT };
    private States m_CurrentState;

    private void ChangeState(States newState)
    {
        if (newState == m_CurrentState)
            return;

        ExitState();
        InitState(newState);
    }

    private void UpdateState()
    {


        switch (m_CurrentState)
        {
            case States.IDLE:

                break;
            case States.RUN:
                m_Rb.velocity = new Vector2(m_WhereToGo * m_Ms, m_Rb.velocity.y);

                if (m_WhereToGo == -1)
                {
                    transform.eulerAngles = Vector3.up * 180;
                }
                else if (m_WhereToGo == 1)
                {
                    transform.eulerAngles = Vector3.zero;
                }
                break;

            case States.FOLLOW:

                if (m_Target  != null)
                {
                    Vector2 actualPos = transform.position; //MI POSICION
                    Vector2 playerPos = m_Target.transform.position; //LA DEL ENEMIGO

                    Vector3 follow = (playerPos - actualPos).normalized ; //CALCULAS HACIA DONDE ES

                    if (playerPos.x < actualPos.x)
                    {
                        transform.eulerAngles = Vector3.up * 180;
                    }
                    else if (playerPos.x > actualPos.x)
                    {
                        transform.eulerAngles = Vector3.zero;
                    }
                    m_Rb.velocity = new Vector2(follow.x * m_Ms, m_Rb.velocity.y); //LO SIGUES
                }else
                {
                    //DOES NOTHING
                }
               
                break;
            case States.HIT:
                break;
        }
    }
    private void InitState(States currentState)
    {
        m_CurrentState = currentState;

        switch (m_CurrentState)
        {
            case States.IDLE:
                m_Rb.velocity = Vector3.zero;
                m_Animator.Play("Enemy_Idle");
                //REPRODUCIR ANIMACION DE IDLE
                break;
            case States.RUN:
                m_WhereToGo = -1;
                //REPRODUCIR ANIMACION DE MOVERSE
                m_Animator.Play("Enemy_Walk");
                break;
            case States.FOLLOW:
                //REPRODUCIR ANIMACION DE MOVERSE
                m_Animator.Play("Enemy_Walk");
                break;
            case States.HIT:
               
                m_Rb.velocity = Vector2.zero; //PEGA QUIETO
                SendDamage(m_Damage);
                m_Animator.Play("Enemy_Melee");  //REPRODUCIR ANIMACION GOLPE

                break;
        }
    }
    private void ExitState()
    {
        switch (m_CurrentState)
        {
            case States.RUN:
                break;

            case States.FOLLOW:
                break;

            case States.HIT:
                break;
        }
    }
    // ------------------------------------------------------------------------------------------ \\


    public event Action<GameObject> DeathEvent; //DELEGADO DE MUERTE DEL ENEMIGO

    private Rigidbody2D m_Rb; //EL RIGIDBODY
    
    private GameObject m_Target;
    private float m_WhereToGo; //DIRECCION A LA QUE IRA CADA VEZ QUE ENTRE EN EL METODO PATRULLA (SOLO DEBERIA ENTRAR UNA VEZ)
    [SerializeField]
    private float m_Ms; //MOVEMENT SPEED
    [SerializeField]
    private int m_Damage;
    [SerializeField]
    private int m_Hp;

    [SerializeField]
    private GameObject m_DetectionRange;
    [SerializeField]
    private GameObject m_HitRange;
    [SerializeField]
    private GameObject m_Hitbox;
    private Animator m_Animator;


    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rb = GetComponent<Rigidbody2D>();
        InitState(States.RUN);
        m_DetectionRange.GetComponent<EnemyDetector>().FollowPlayerEvent += PlayerDetected;
        m_HitRange.GetComponent<EnemyHit>().HitPlayerEvent += PlayerAttack;
    }

    private void PlayerDetected(GameObject obj)
    {
        m_Target = obj;
        ChangeState(States.FOLLOW);
    }
    private void PlayerAttack(bool c)
    {
        if (c)
        {
            ChangeState(States.HIT); //LE PEGO
        }
    }
    private void SendDamage(int dmg)
    {
        m_Hitbox.GetComponent<HitBoxController>().LoadDamage(dmg);
    }
    private void ReciveDamage(int dmg)
    {
        m_Hp -= dmg;

        if (m_Hp < 1)
        {
            OnDeath();
        }
    }
    private void Update()
    {
        UpdateState();
        //Debug.Log(m_CurrentState);
    }

    void OnDeath() //EVENTO O DELEGADO EN EL QUE AVISARA QUE HA MUERTO
    {
        DeathEvent?.Invoke(this.gameObject); //ME MUERO ASI QUE AVISO PARA BORRARME DE LA LISTA
        Destroy(this.gameObject); //ME MUERO :(
    }
    public void ReturnToFollow()
    {
        ChangeState(States.FOLLOW);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall") 
        {
            m_WhereToGo = m_WhereToGo * -1;            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerHitBox")
        {
            Debug.Log("Me ha pegado el player");
            int dmg = collision.gameObject.GetComponent<HitBoxController>().m_Damage;
            ReciveDamage(dmg);
        }
    }
    private void OnDestroy()
    {
        m_DetectionRange.GetComponent<EnemyDetector>().FollowPlayerEvent -= PlayerDetected; //ME DESSUCRIBO PORQUE SI ME MUERO YA NO HACE FALTA NINGUN EVENTO
        m_HitRange.GetComponent<EnemyHit>().HitPlayerEvent -= PlayerAttack; //ME DESSUCRIBO PORQUE SI ME MUERO YA NO HACE FALTA NINGUN EVENTO
    }
}

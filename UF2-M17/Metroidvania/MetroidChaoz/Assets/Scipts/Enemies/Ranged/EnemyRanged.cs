using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyRanged : MonoBehaviour, CanDie
{
    //STATE MACHINE STUFF

    //STATES
    private enum States { NONE, RUN, FOLLOW, SHOOT };
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
                Vector2 actualPos = transform.position; //MI POSICION
                Vector2 playerPos = m_Target.transform.position; //LA DEL ENEMIGO
                if (playerPos.x < actualPos.x)
                {
                    transform.eulerAngles = Vector3.up * 180;
                    ;
                }
                else if (playerPos.x > actualPos.x)
                {
                    transform.eulerAngles = Vector3.zero;
                }
                Vector3 follow = (playerPos - actualPos).normalized; //CALCULAS HACIA DONDE ES
                m_Rb.velocity = new Vector2(follow.x * m_Ms, m_Rb.velocity.y); //LO SIGUES
                break;
            case States.SHOOT:
                break;
        }
    }
    private void InitState(States currentState)
    {
        m_CurrentState = currentState;

        switch (m_CurrentState)
        {
            case States.RUN:
                m_WhereToGo = -1;
                //REPRODUCIR ANIMACION DE MOVERSE
                m_Animator.Play("Ranged_Walk");
                break;
            case States.FOLLOW:
                //REPRODUCIR ANIMACION DE MOVERSE
                m_Animator.Play("Ranged_Walk");
                break;
            case States.SHOOT:
                m_Rb.velocity = Vector2.zero; //PEGA QUIETO
                m_Animator.Play("Ranged_Shoot");
                //REPRODUCIR ANIMACION GOLPE
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

            case States.SHOOT:
                break;
        }
    }
    // ------------------------------------------------------------------------------------------ \\



    public event Action<GameObject> DeathEvent; //DELEGADO DE MUERTE DEL ENEMIGO

    private Rigidbody2D m_Rb; //EL RIGIDBODY
    private Animator m_Animator;

    private GameObject m_Target;
    private float m_WhereToGo; //DIRECCION A LA QUE IRA CADA VEZ QUE ENTRE EN EL METODO PATRULLA (SOLO DEBERIA ENTRAR UNA VEZ)
    [SerializeField]
    private float m_Ms; //MOVEMENT SPEED
    [SerializeField]
    private int m_Damage; //ENEMY DAMAGE

    [SerializeField]
    private GameObject m_DetectionRange;
    [SerializeField]
    private GameObject m_HitRange;
    [SerializeField]
    private GameObject m_BulletPrefab;
    [SerializeField]
    private int m_Hp;

    private void Start()
    {
        m_Rb = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        InitState(States.RUN);
        m_DetectionRange.GetComponent<EnemyDetector>().FollowPlayerEvent += PlayerDetected;
        m_DetectionRange.GetComponent<EnemyDetector>().UnfollowPlayerEvent += PlayerLost;
        m_HitRange.GetComponent<EnemyHit>().HitPlayerEvent += PlayerAttack;
    }
    private void Update()
    {
        UpdateState();
    }

    private void PlayerDetected(GameObject obj)
    {
        m_Target = obj;
        InitState(States.FOLLOW);
    }
    private void PlayerLost (bool c)
    {
        if (c)
        {
            InitState(States.RUN);
            m_Target = null;
        }
    }
    private void PlayerAttack(bool c)
    {
        if (c)
        {
            InitState(States.SHOOT); //LE PEGO
        }
    }
    public void Shoot()
    {
        GameObject bullet = Instantiate(m_BulletPrefab);
        bullet.transform.position = transform.position;
        bullet.GetComponent<BulletController>().LoadDamageAndShoot(m_Damage, m_Target.transform.position);
    }
    private void ReciveDamage(int dmg)
    {
        m_Hp -= dmg;

        if (m_Hp < 1)
        {
            OnDeath();
        }
    }
    void OnDeath() //EVENTO O DELEGADO EN EL QUE AVISARA QUE HA MUERTO
    {
        DeathEvent?.Invoke(this.gameObject); //ME MUERO ASI QUE AVISO PARA BORRARME DE LA LISTA
        Destroy(this.gameObject); //ME MUERO :(
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

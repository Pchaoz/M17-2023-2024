using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour, CanDie //INTERFAZ PARA QUE LOS ENEMIGOS TENGAN EL MISMO DELEGADO DE MUERTE
{
    //STATE MACHINE STUFF

    //STATES
    private enum States { NONE, RUN, FOLLOW, HIT };
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
                break;

            case States.FOLLOW:
                Vector2 actualPos = transform.position; //MI POSICION
                Vector2 playerPos = m_Target.transform.position; //LA DEL ENEMIGO

                Vector3 follow = (playerPos - actualPos); //CALCULAS HACIA DONDE ES
                m_Rb.velocity = new Vector2(follow.x * m_Ms, m_Rb.velocity.y); //LO SIGUES
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
            case States.RUN:
                m_WhereToGo = -1;
                //REPRODUCIR ANIMACION DE MOVERSE
                break;
            case States.FOLLOW:
                //REPRODUCIR ANIMACION DE MOVERSE
                break;
            case States.HIT:
                m_Rb.velocity = Vector2.zero; //PEGA QUIETO
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
    private GameObject m_DetectionRange;

    private void Start()
    {
        m_Rb = GetComponent<Rigidbody2D>();
        InitState(States.RUN);
        m_DetectionRange.GetComponent<PlayerDetector>().FollowPlayerEvent += PlayerDetected;
    }

    private void PlayerDetected(GameObject obj)
    {
        m_Target = obj;
        InitState(States.FOLLOW);
    }

    private void Update()
    {
        UpdateState();
    }

    void OnDeath() //EVENTO O DELEGADO EN EL QUE AVISARA QUE HA MUERTO
    {
        m_DetectionRange.GetComponent<PlayerDetector>().FollowPlayerEvent += PlayerDetected;
        DeathEvent?.Invoke(this.gameObject); //ME MUERO ASI QUE AVISO PARA BORRARME DE LA LISTA
        Destroy(this.gameObject); //ME MUERO :(
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall") 
        {
            m_WhereToGo = m_WhereToGo * -1;
        }
        
        if (collision.gameObject.tag == "Player")
        {
            //CODIGO TEMPORAL PARA HACER PRUEBAS CON LAS OLEADAS
            OnDeath();
        }
    }

    

}

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    //STATE MACHINE STUFF

    //STATES
    private enum States { NONE, IDLE, RUN, JUMP, HIT, HITCMB };
    private States m_CurrentState;
    private bool canCombo;

    private void ChangeState(States newState)
    {
        //Debug.Log(newState);

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
                if (m_MovementAction.ReadValue<Vector2>().x != 0) {
                    ChangeState(States.RUN);
                }
                break;

            case States.RUN:
                float PosToMNove = m_MovementAction.ReadValue<Vector2>().x;
                //Debug.Log(PosToMNove); ESTO ESTABA DE PRUEBA PARA VER BIEN LOS INPUTS DE MOVIMIENTO

                if (PosToMNove > 0 && canMoveFoward)
                {
                    m_Rb.velocity = new Vector2(PosToMNove * m_Ms, m_Rb.velocity.y); //LEER EL INPUT Y MOVERSE HACIA LA DERECHA
                }
                else if (PosToMNove < 0 && canMoveBackwards)
                {
                    m_Rb.velocity = new Vector2(PosToMNove * m_Ms, m_Rb.velocity.y); //LEER EL INPUT Y MOVERSE HACIA LA IZQUIERDA
                }
                
                if (PosToMNove == 0)
                {
                    ChangeState(States.IDLE);
                }
                break;

            case States.JUMP:
                if (!isJumping)
                {
                    //RAYCAST PARA EL SUELO Y SALTAR?? MIRAR DEL AÑO PASADO
                }
                break;

            case States.HIT:
                break;

            case States.HITCMB:
                break;
        }
    }
    private void InitState(States currentState)
    {
        m_CurrentState = currentState;

        switch (m_CurrentState)
        {
            case States.IDLE:

                m_Rb.velocity = Vector2.zero;
                //REPRODUCIR ANIMACION DE IDLE
                break;

            case States.RUN:
                //REPRODUCIR ANIMACION DE CORRER
                break;
            case States.JUMP:
                //REPRODUCIR ANIMACION DE SALTO
                break;
            case States.HIT:
                m_Rb.velocity = Vector2.zero; //PEGA QUIETO
                //REPRODUCIR ANIMACION GOLPE 1
                canCombo = false; //PORQUE SOLO EN UN MOMENTO PRECISO DE LA ANIMACION PODRA ACTIVAR LA POSIBILIDAD DE HACER EL COMBO
                break;
        }
    }
    private void ExitState()
    {
        switch (m_CurrentState)
        {
            case States.IDLE:
                break;

            case States.RUN:
                break;

            case States.JUMP:
                break;

            case States.HIT:
                break;

            case States.HITCMB:
                break;
        }
    }
    // ------------------------------------------------------------------------------------------ \\

    //INPUT ACTIONS
    [SerializeField]
    private InputActionAsset m_InputAsset;
    private InputActionAsset m_Input;
    private InputAction m_MovementAction;

    //PLAYER STATS
    [Header("Character stats")]
    [SerializeField]
    private int m_Hp;
    [SerializeField]
    private int m_Ms;
    [SerializeField]
    private int m_NormalDamage;
    [SerializeField]
    private int m_ComboDamage;
    [SerializeField]
    private int m_JumpForce;

    private bool isJumping;
    private bool canMoveFoward;
    private bool canMoveBackwards;

    //RIGIDBODY AND STUFF
    private Rigidbody2D m_Rb;

    private Vector3 m_ColliderBottom;
    private void Awake()
    {
        Assert.IsNotNull(m_InputAsset);
        m_Input = Instantiate(m_InputAsset);
        m_MovementAction = m_Input.FindActionMap("Movement").FindAction("Walk");
        m_Input.FindActionMap("Movement").FindAction("Jump").performed += Jump;
        m_Input.FindActionMap("Movement").Enable();

        m_Rb = GetComponent<Rigidbody2D>();
        m_ColliderBottom = Vector3.up * GetComponent<BoxCollider2D>().size.y / 2; //LA PART DE ABAIX DEl COLLIDER
        canMoveFoward = true;
        canMoveBackwards = true;
    }

    private void Start()
    {
        InitState(States.IDLE);
    }
    void Update()
    {
        UpdateState();
    }

    void Jump(InputAction.CallbackContext actionContext)
    {
        if (!isJumping)
        {
            Vector3 initialPos = transform.position - m_ColliderBottom;
            RaycastHit2D hit = Physics2D.Raycast(initialPos, Vector2.down, 0.3f, LayerMask.GetMask("Ground"));
            if (hit.collider != null)
            {
                //Debug.DrawLine(initialPos, hit.point, new UnityEngine.Color(1f, 0f, 1f), 5f);
                m_Rb.velocity = new Vector2(m_Rb.velocity.x, m_JumpForce);
                isJumping = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("He tocado algo"); //ESTO ESTABA PARA HACER PRUEBAS
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) 
        {
            Debug.Log("JUMP RESET");
            isJumping = false;
        }
       
        if (collision.gameObject.tag == "Wall")
        {
            if (collision.gameObject.transform.position.x > transform.position.x)
            {
                canMoveFoward = false;
            }else
            {
                canMoveBackwards = false;
            }
        }
        if (collision.gameObject.tag == "Enemy")
        {
            //CODIGO TEMPORAL PARA HACER PRUEBAS CON LAS OLEADAS
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
         if (collision.gameObject.tag == "Wall")
         {
            canMoveBackwards = true;           
            canMoveFoward = true;
         }
    }

    private void OnDestroy()
    {
        //m_Input.FindActionMap("Standard").FindAction("Attack").performed -= AttackAction; //SUBSTITUIR POR EL MIO CUANDO PEGUE
        m_Input.FindActionMap("Movement").FindAction("Jump").performed -= Jump;
        m_Input.FindActionMap("Movement").Disable();
    }


}

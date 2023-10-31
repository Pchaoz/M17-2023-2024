using JetBrains.Annotations;
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
    private enum States { NONE, IDLE, RUN, JUMP, LIGHTHIT, HEAVYHIT, HITCMB };
    private States m_CurrentState;
    private bool canLightCombo;
    private bool canHardCombo;
    private Animator m_Animator;

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

                float PosToMNove = m_MovementAction.ReadValue<Vector2>().x; //LECTURA DEL INPUT SYSTEM

                if (PosToMNove == -1)
                {
                    transform.eulerAngles = Vector3.up * 180;
;                }
                else if (PosToMNove == 1)
                {
                    transform.eulerAngles = Vector3.zero;
                }
                m_Rb.velocity = new Vector2(PosToMNove * m_Ms, m_Rb.velocity.y);  //MOVIMIENTO PERSONAJE

                //Debug.Log(PosToMNove); ESTO ESTABA DE PRUEBA PARA VER BIEN LOS INPUTS DE MOVIMIENTO
               
                if (PosToMNove == 0)
                {
                    ChangeState(States.IDLE);
                }
                break;

            case States.JUMP:
                break;

            case States.LIGHTHIT:
                SendDamage(m_LightDmg);
                break;

            case States.HEAVYHIT:
                SendDamage(m_HeavyDmg);
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
                m_Animator.Play("Player_Idle");
                break;

            case States.RUN:
                //REPRODUCIR ANIMACION DE CORRER
                m_Animator.Play("Player_Walk");
                break;
            case States.JUMP:
                //REPRODUCIR ANIMACION DE SALTO
                //m_Animator.Play("Player_Jump");
                break;
            case States.LIGHTHIT:
                m_Rb.velocity = Vector2.zero; //PEGA QUIETO
                //REPRODUCIR ANIMACION GOLPE 1
                m_Animator.Play("Player_LightHit");
                break;
            case States.HEAVYHIT:
                m_Rb.velocity = Vector2.zero; //PEGA QUIETO
                //REPRODUCIR ANIMACION GOLPE 2
                m_Animator.Play("Player_HeavyHit");
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

            case States.LIGHTHIT:
                break;

            case States.HEAVYHIT:
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
    private int m_LightDmg;
    [SerializeField]
    private int m_HeavyDmg;
    [SerializeField]
    private int m_ComboDamage;
    [SerializeField]
    private int m_JumpForce;

    private bool isJumping;

    //RIGIDBODY AND STUFF
    private Rigidbody2D m_Rb;

    [SerializeField]
    private GameEvent1Int OnHpChange;

    [SerializeField]
    private GameObject m_Hitbox;

    private Vector3 m_ColliderBottom;
    private void Awake()
    {
        Assert.IsNotNull(m_InputAsset);
        m_Input = Instantiate(m_InputAsset);
        m_MovementAction = m_Input.FindActionMap("Movement").FindAction("Walk");
        m_Input.FindActionMap("Movement").FindAction("Jump").performed += Jump;
        m_Input.FindActionMap("Movement").FindAction("LightHit").performed += LightHit;
        m_Input.FindActionMap("Movement").FindAction("HeavyHit").performed += HeavyHit;
        m_Input.FindActionMap("Movement").Enable();

        m_Animator = GetComponent<Animator>();
        m_Rb = GetComponent<Rigidbody2D>();
        m_ColliderBottom = Vector3.up * GetComponent<BoxCollider2D>().size.y / 2; //LA PART DE ABAIX DEl COLLIDER
    }

    private void Start()
    {
        InitState(States.IDLE);
        OnHpChange.Raise(m_Hp);
    }
    void Update()
    {
        UpdateState();
    }

    public void ReturnToIdle()
    {
        ChangeState(States.IDLE);
    }

    public void OpenLightCombo()
    {
        canLightCombo = true;
    }
    public void CloseLightCombo()
    {
        canLightCombo = false;
    }
    public void ReciveDamage(int damage)
    {
        Debug.Log("He rebut: " + damage + " de mal");
        m_Hp -= damage;
        OnHpChange.Raise(m_Hp);

        if (m_Hp == 0 || m_Hp < 0)
        {
            //ME MUERO Y PASAN COSAS
            Destroy(this.gameObject);
        }
    }

    private void SendDamage(int dmg)
    {
        m_Hitbox.GetComponent<HitBoxController>().LoadDamage(dmg);
    }
    private void HeavyHit(InputAction.CallbackContext actionContext)
    {
        if (m_CurrentState != States.JUMP || m_CurrentState != States.LIGHTHIT || m_CurrentState != States.HEAVYHIT)
        {
            ChangeState(States.HEAVYHIT);
        }
    }
    private void LightHit(InputAction.CallbackContext actionContext)
    {
        if (m_CurrentState != States.JUMP || m_CurrentState != States.LIGHTHIT || m_CurrentState != States.HEAVYHIT)
        {
            ChangeState(States.LIGHTHIT);
        }
    }
    private void Jump(InputAction.CallbackContext actionContext)
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
            //Debug.Log("JUMP RESET");
            isJumping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyHitBox")
        {
            int dmg = collision.gameObject.GetComponent<HitBoxController>().m_Damage;
            ReciveDamage(dmg);
        }
        if (collision.gameObject.tag == "EnemyBullet")
        {
            int dmg = collision.gameObject.GetComponent<BulletController>().m_Damage;
            ReciveDamage(dmg);
        }
    }
    private void OnDestroy()
    {
        m_Input.FindActionMap("Movement").FindAction("LightHit").performed -= LightHit;
        m_Input.FindActionMap("Movement").FindAction("HeavyHit").performed -= HeavyHit;
        m_Input.FindActionMap("Movement").FindAction("Jump").performed -= Jump;
        m_Input.FindActionMap("Movement").Disable();
    }
}

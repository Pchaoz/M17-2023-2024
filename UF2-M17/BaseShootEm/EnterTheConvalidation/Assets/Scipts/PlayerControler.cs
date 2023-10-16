using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("ahora estoy " + newState);
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
                m_Rb.velocity = Vector2.right * m_MovementAction.ReadValue<Vector2>() * m_Ms; //LEER EL INPUT Y MOVERSE HACIA LA DIRECCION DEL INPUT
                if (m_Rb.velocity == Vector2.zero)
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
    [SerializeField]
    private bool isJumping;

    //RIGIDBODY AND STUFF
    private Rigidbody2D m_Rb;


    private void Awake()
    {
        Assert.IsNotNull(m_InputAsset);
        m_Input = Instantiate(m_InputAsset);
        m_MovementAction = m_Input.FindActionMap("Movement").FindAction("Walk");
        m_Input.FindActionMap("Movement").Enable();

        m_Rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InitState(States.IDLE);
    }
    void Update()
    {
        UpdateState();
    }

    private void OnDestroy()
    {
        //m_Input.FindActionMap("Standard").FindAction("Attack").performed -= AttackAction; //SUBSTITUIR POR EL MIO CUANDO PEGUE
        m_Input.FindActionMap("Standard").Disable();
    }


}

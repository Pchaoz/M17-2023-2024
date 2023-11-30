using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public enum CharacterState
    {
        IDLE,
        WALK,
        JUMP
    }
    [SerializeField]
    private CharacterState m_CurrentState;
    private void ChangeState(CharacterState newState)
    {
        //Debug.Log(newState);

        if (newState == m_CurrentState)
            return;

        ExitState();
        Debug.Log(newState);
        InitState(newState);
    }
    private void InitState(CharacterState currentState)
    {
        m_CurrentState = currentState;

        switch (m_CurrentState)
        {
            case CharacterState.IDLE:
                break;
            case CharacterState.WALK:
                break;
            case CharacterState.JUMP:
                break;
        }
    }

    private void UpdateState()
    {
        switch (m_CurrentState)
        {
            case CharacterState.IDLE:
                if (m_BodyMovemnt.ReadValue<Vector2>() != Vector2.zero)
                    ChangeState(CharacterState.WALK);
                Look(); //PUEDE MIRAR ALREDEDOR
                break;
            case CharacterState.WALK:
                if (m_BodyMovemnt.ReadValue<Vector2>() == Vector2.zero)
                    ChangeState(CharacterState.IDLE);
                Look(); //PUEDE MIRAR ALREDEDOR
                Movement(); //PUEDE MOVERSE
                break;
            case CharacterState.JUMP:
                Look(); //PUEDE MIRAR ALREDEDOR
                Movement(); //PUEDE MOVERSE
                break;
        }
    }
   
    private void ExitState()
    {
        switch (m_CurrentState)
        {
            case CharacterState.IDLE:
                break;
            case CharacterState.WALK:
                break;
            case CharacterState.JUMP:
                break;
        }
    }

    [SerializeField]
    private InputActionAsset m_InputAsset;
    private InputActionAsset m_Input;
    private InputAction m_BodyMovemnt;
    private InputAction m_HeadMovement;

    [Header("Player Stats")]
    [SerializeField]
    private int m_MaxHp;
    [SerializeField]
    private int m_Hp;
    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private float m_SensX;
    [SerializeField]
    private float m_SensY;
    [SerializeField]
    private float m_JumpForce;
    private bool OnGround;

    [SerializeField]
    private LayerMask m_Layer;
    private Rigidbody m_Rb;


    [SerializeField]
    private GameObject m_FPCamera;


    private void Awake()
    {
        OnGround = true;
        m_Rb = GetComponent<Rigidbody>();

        Assert.IsNotNull(m_InputAsset);
        m_Input = Instantiate(m_InputAsset);
        m_BodyMovemnt = m_Input.FindActionMap("Ground").FindAction("Walk"); //MOVIMIENTO WASD
        m_HeadMovement = m_Input.FindActionMap("Ground").FindAction("Look"); //DELTA DEL RATON
        m_Input.FindActionMap("Ground").FindAction("Jump").performed += Jump;

        m_Input.Enable();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InitState(CharacterState.IDLE);
    }

    private void Movement()
    {
        Vector2 bodyMovement = m_BodyMovemnt.ReadValue<Vector2>(); //LEES EL INPUT DE WASD

        m_Rb.MovePosition(transform.position + bodyMovement.x * transform.right * m_Speed * Time.deltaTime 
            + bodyMovement.y * transform.forward * m_Speed * Time.deltaTime); //TE MUEVES CON EL RIGIDBODY
    }

    private void Look()
    {
        Vector2 mouseMovement = m_HeadMovement.ReadValue<Vector2>(); //LEES EL INPUT DEL DELTA MOUSE
        float angle = (m_FPCamera.transform.localEulerAngles.x - mouseMovement.y * m_SensY * Time.deltaTime + 360) % 360; //ASEGURARSE DE QUE EL ANGULO ESTE EN 360 GRADOS
        if (angle > 180) //CON ESTO TE ASEGURAS DE QUE EL ANGULO ESTE EN EL RANGO DEL CLAMP Y NO SE VUELVA LOCO
            angle -= 360;

        angle = Mathf.Clamp(angle, -70, 70); //CLAMP PARA LIMITAR LO QUE PUEDES MOVER LA CAMARA VERTICALMENTE

        m_FPCamera.transform.localEulerAngles = Vector3.right * angle; //MUEVES LA CAMARA VERTICALMENTE
        transform.Rotate(Vector3.up * mouseMovement.x * m_SensX * Time.deltaTime); //MOVER EL PERSONAJE CON LA CAMARA HORIZONTALMENTE
    }

    private void Jump(InputAction.CallbackContext actionContext)
    {
        
        if (OnGround)
        {
            
            Vector3 bottomCol = Vector3.up * GetComponent<CapsuleCollider>().radius / 2;
            RaycastHit2D hit = Physics2D.Raycast(bottomCol, Vector2.down, 0.5f, m_Layer);
            Debug.DrawLine(bottomCol, hit.point, Color.red, 100);
            Debug.Log(hit.collider);
            if (hit.collider != null) //SI EL RAYCAST CHOCA CONTRA ALGO
            {
                Debug.Log("EL RAYCAST HA TOCAO EL SUELO");
                m_Rb.velocity = new Vector2(m_Rb.velocity.x, m_JumpForce); //AÑADO LA FUERZA PARA SALTAR
                OnGround = false; //ME PONGO A SALTAR POR LO QUE PONGO LA VARIABLE A TRUE
                ChangeState(CharacterState.JUMP);
            }
        }
            
       
    }
    private void FixedUpdate()
    {
        UpdateState();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == m_Layer)
        {
            Debug.Log("Choco contra el suelo");
            OnGround = true;
            ChangeState(CharacterState.IDLE);
        }
    }
}

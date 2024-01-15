using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour
{
    public enum PlayerStates
    {
        IDLE,
        WALK,
        JUMP
    }
    [SerializeField]
    private PlayerStates m_CurrentState;
    private void ChangeState(PlayerStates newState)
    {
        if (newState == m_CurrentState)
            return;

        ExitState();
        //Debug.Log(newState);
        InitState(newState);
    }
    private void InitState(PlayerStates currentState)
    {
        m_CurrentState = currentState;

        switch (m_CurrentState)
        {
            case PlayerStates.IDLE:
                break;
            case PlayerStates.WALK:
                break;
            case PlayerStates.JUMP:
                break;
        }
    }

    private void UpdateState()
    {
        switch (m_CurrentState)
        {
            case PlayerStates.IDLE:
                if (m_BodyMovement.ReadValue<Vector2>() != Vector2.zero)
                    ChangeState(PlayerStates.WALK);
                Look(); //POTS MIRAR
                break;
            case PlayerStates.WALK:
                if (m_BodyMovement.ReadValue<Vector2>() == Vector2.zero)
                    ChangeState(PlayerStates.IDLE);
                Walk(); //POTS CAMINAR
                Look(); //POTS MIRAR
                break;
            case PlayerStates.JUMP:
                Walk(); //POTS CAMINAR
                Look(); //POTS MIRAR
                break;
        }
    }

    private void ExitState()
    {
        switch (m_CurrentState)
        {
            case PlayerStates.IDLE:
                break;
            case PlayerStates.WALK:
                break;
            case PlayerStates.JUMP:
                break;
        }
    }

    [SerializeField]
    private InputActionAsset m_InputAsset;
    private InputActionAsset m_Input;
    private InputAction m_BodyMovement;
    private InputAction m_MouseMovement;

    private Rigidbody m_Rb;

    [SerializeField]
    private GameObject m_ThirdPersonCamera;

    [Header("PLAYER STATS")]
    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private float m_JumpForce;
    [SerializeField]
    private float m_SensX;
    [SerializeField]
    private float m_SensY;
    private bool OnGround;

    [SerializeField]
    private LayerMask m_LayerMask;


    private void Awake()
    {
        //CHARACTER SCRIPTS
        m_Rb = GetComponent<Rigidbody>();

        //INPUT SYSTEM
        Assert.IsNotNull(m_InputAsset);
        m_Input = Instantiate(m_InputAsset);
        m_BodyMovement = m_Input.FindActionMap("Ground").FindAction("Walk");
        m_MouseMovement = m_Input.FindActionMap("Ground").FindAction("Camera");
        m_Input.FindActionMap("Ground").FindAction("Jump").performed += Jump; //INPUT DEL ESPACIO
        m_Input.FindActionMap("Ground").Enable();
        
        //STATE MACHINE
        InitState(PlayerStates.IDLE);

        //CHARACTER STATS
        OnGround = true;
    }

    private void Update()
    {
        UpdateState();
    }

    private void Walk()
    {
        Vector2 bodyMovement = m_BodyMovement.ReadValue<Vector2>(); //LEES EL INPUT DE WASD

        m_Rb.MovePosition(transform.position + bodyMovement.x * transform.right * m_Speed * Time.deltaTime
            + bodyMovement.y * transform.forward * m_Speed * Time.deltaTime); //TE MUEVES CON EL RIGIDBODY
    }

    private void Jump(InputAction.CallbackContext actionContext)
    {
        if (OnGround)
        {
            Vector3 bottomCol = Vector3.up * GetComponent<CapsuleCollider>().radius / 2;
            RaycastHit hit;
            Debug.Log("QUIERO SALTAR, VARIABLE DE SALTO: " + OnGround);

            if (Physics.Raycast(bottomCol, Vector2.down, out hit, Mathf.Infinity, m_LayerMask))
            {
                Debug.DrawLine(bottomCol, hit.point, Color.black, 6f);
                if (hit.collider.gameObject.CompareTag("JumpReset"))
                {
                    ChangeState(PlayerStates.JUMP);
                    Debug.Log($"He tocat {hit.collider.gameObject.tag} a la posicio {hit.point} amb normal {hit.normal}");
                    m_Rb.AddForce(0, m_JumpForce, 0, ForceMode.Impulse);
                    OnGround = false;
                }

            }
        }
    }

    private void Look()
    {
        Vector2 mouseMovement = m_MouseMovement.ReadValue<Vector2>(); //LEES EL INPUT DEL DELTA MOUSE
        transform.Rotate(Vector3.up * mouseMovement.x * m_SensX * Time.deltaTime); //MOVER EL PERSONAJE CON LA CAMARA HORIZONTALMENTE
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("JumpReset"))
        {
            OnGround = true;
        }
    }
}

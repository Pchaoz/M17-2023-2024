using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

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

    [Header("Prefab de los portales")]
    [SerializeField]
    private GameObject prefabBlue;
    [SerializeField]
    private GameObject prefabOrange;

    //LOS PORTALES YA INICIALZADOS
    private GameObject m_BluePortal;
    private GameObject m_OrangePortal;

    [SerializeField]
    private LayerMask m_Layer;
    private Rigidbody m_Rb;


    [SerializeField]
    private GameObject m_FPCamera;


    private void Awake()
    {
        OnGround = true;
        m_Rb = GetComponent<Rigidbody>();

        //INSTANCIAR LOS PORTALES Y GUARDARLOS DESHABILITADOS
        m_BluePortal = Instantiate(prefabBlue);
        m_OrangePortal = Instantiate(prefabOrange);
        m_BluePortal.SetActive(false);
        m_OrangePortal.SetActive(false);

        Assert.IsNotNull(m_InputAsset);
        m_Input = Instantiate(m_InputAsset);
        m_BodyMovemnt = m_Input.FindActionMap("Ground").FindAction("Walk"); //MOVIMIENTO WASD
        m_HeadMovement = m_Input.FindActionMap("Ground").FindAction("Look"); //DELTA DEL RATON
        m_Input.FindActionMap("Ground").FindAction("Jump").performed += Jump; //INPUT DEL ESPACIO
        m_Input.FindActionMap("Ground").FindAction("ShootOrange").performed += ShootPortal;
        m_Input.FindActionMap("Ground").FindAction("ShootBlue").performed += ShootPortal;

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

        angle = Mathf.Clamp(angle, -85, 85); //CLAMP PARA LIMITAR LO QUE PUEDES MOVER LA CAMARA VERTICALMENTE

        m_FPCamera.transform.localEulerAngles = Vector3.right * angle; //MUEVES LA CAMARA VERTICALMENTE
        transform.Rotate(Vector3.up * mouseMovement.x * m_SensX * Time.deltaTime); //MOVER EL PERSONAJE CON LA CAMARA HORIZONTALMENTE
    }

    private void Jump(InputAction.CallbackContext actionContext)
    { 
        if (OnGround)
        {
            Vector3 bottomCol = Vector3.up * GetComponent<CapsuleCollider>().radius / 2;
            RaycastHit hit;
            Debug.Log("QUIERO SALTAR, VARIABLE DE SALTO: " + OnGround);

            if (Physics.Raycast(bottomCol, Vector2.down, out hit, 0.5f, m_Layer))
            {
                if(hit.collider.gameObject.CompareTag("Jumpable"))
                {
                    Debug.Log($"He tocat {hit.collider.gameObject.tag} a la posicio {hit.point} amb normal {hit.normal}");
                    Debug.DrawLine(bottomCol, hit.point, Color.green, 5f);
                    m_Rb.AddForce(0, m_JumpForce, 0, ForceMode.Impulse);
                    OnGround = false;
                }
               
            }         
        }
    }

    private void ShootPortal(InputAction.CallbackContext actionContext)
    {
        RaycastHit hit;
        Debug.Log(actionContext.action.name); // LITERALMENTE EL NOMBRE DE LA ACCION
        Vector3 direction = m_FPCamera.transform.forward;
        if (Physics.Raycast(m_FPCamera.transform.position, m_FPCamera.transform.forward, out hit, Mathf.Infinity, m_Layer))
        {
            Debug.DrawLine(m_FPCamera.transform.position, hit.point, Color.magenta, 5f);
            if (actionContext.action.name.Equals("ShootOrange"))
            {
                m_OrangePortal.transform.position = hit.point;
                m_OrangePortal.transform.forward = hit.normal;
                if (!m_OrangePortal.activeSelf)
                    m_OrangePortal.SetActive(true);

            }else if (actionContext.action.name.Equals("ShootBlue"))
            {
                m_BluePortal.transform.position = hit.point;
                m_BluePortal.transform.forward = hit.normal;
                if (!m_BluePortal.activeSelf)
                    m_BluePortal.SetActive(true);
            }
        }
    } 

    private void FixedUpdate()
    {
        UpdateState();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Jumpable"))
        {
            OnGround = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BluePortal"))
        {
            if (m_OrangePortal.activeSelf)
            {
                m_OrangePortal.GetComponent<Portal>().NoCol();
                transform.position = m_OrangePortal.transform.position;
                ImpulseOnPortal(m_OrangePortal.transform);
            }               
        }
        else if (other.gameObject.CompareTag("OrangePortal"))
        {
            if (m_BluePortal.activeSelf)
            {
                m_BluePortal.GetComponent<Portal>().NoCol();
                transform.position = m_BluePortal.transform.position;
                ImpulseOnPortal(m_BluePortal.transform);
            }           
        }
    }

    private void ImpulseOnPortal(Transform otherPortal)
    {
        m_Rb.velocity = otherPortal.transform.forward * m_Rb.velocity.magnitude;
    }
}

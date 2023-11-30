using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class CharacterController : MonoBehaviour
{
    //COSAS DEL RAUL
    public enum m_Attackpositions { BASICO, DISPERSION, RAFAGA };
    public m_Attackpositions m_position;

    public delegate void ActualitzarPostura(string m_position);
    public event ActualitzarPostura OnActualitzarPostura; 

    private int m_MaxShuricken = 7;
    private int m_Shuricken = 0;

    public delegate void ActualitzarMunicio(int m_Shuricken);
    public event ActualitzarMunicio OnActualitzarMunicio;

    private bool recargando = false;

    //COSAS YA NO TAN DEL RAUL
    private Rigidbody m_rb;
    private PlayerInputs playerinputs;

    [SerializeField]
    LayerMask m_LayerMask;

    //COSAS POL uwu
    [SerializeField]
    private float m_speed = 4f;
    private float m_actualSpeed;

    [SerializeField]
    private float m_Hp = 50;

    [SerializeField]
    private float m_jumpForce = 6f;

    //CAMARA owO
    [SerializeField]
    private GameObject m_FPC;

    //SENS
    [SerializeField]
    private float m_Sensitivity = 5;

    //COORUTINA GUARDADA PARA DESPUES PODERLA CANCELAR
    [SerializeField]
    Coroutine m_ActualBoost;
    float m_BoostTime = 6f;

    //COSAS DEL ATAQUE MELEE
    [SerializeField]
    private GameObject m_MeleeHit;
    private float m_MeleeCD = 2.5f;



    private void Awake()
    {
        playerinputs = new PlayerInputs();
        playerinputs.Player_Walking.Enable();
        playerinputs.Player_Walking.Jump.performed += Jump;
        playerinputs.Player_Walking.Look.started += DetectInput;
        playerinputs.Player_Walking.Recharge.performed += Recharge;
        playerinputs.Player_Walking.ChangePosition.performed += PChange;
        playerinputs.Player_Walking.Shoot.performed += Shoot;
        playerinputs.Player_Walking.Melee.performed += Melee;
        m_position = m_Attackpositions.BASICO;
        m_actualSpeed = m_speed;


    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        m_rb = GetComponent<Rigidbody>();
        m_Shuricken = m_MaxShuricken;
    }


    private void Update()
    {
        //esto deberia ser la obtencion de la info del mouse
        Vector2 lookInput = playerinputs.Player_Walking.Look.ReadValue<Vector2>();

        //mover camara en horizontal
        transform.Rotate(transform.up * lookInput.x * m_Sensitivity * Time.deltaTime);

        //mover camara en vertical
        float angle = m_FPC.transform.localEulerAngles.x + -1*lookInput.y * m_Sensitivity * Time.deltaTime;
        //Debug.Log(angle);
        m_FPC.transform.localRotation = Quaternion.Euler(angle, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 input = playerinputs.Player_Walking.Movement.ReadValue<Vector2>();
        Vector3 movement = transform.forward * input.y + transform.right * input.x;

        m_rb.MovePosition(transform.position + movement.normalized * m_actualSpeed * Time.fixedDeltaTime);
    }
    void Jump(InputAction.CallbackContext context)
    {
        if (CheckGround())
        {
            m_rb.AddForce(transform.up * m_jumpForce, ForceMode.Impulse);
        }
 
    }
    bool CheckGround()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, -transform.up, Color.cyan, 5f);
        if (Physics.Raycast(transform.position, -transform.up, out hit, GetComponent<CapsuleCollider>().height * 0.55f, m_LayerMask))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                return true;
            }
        }
        return false;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (m_Shuricken > 0)
        {
            switch (m_position)
            {
                case m_Attackpositions.BASICO:
                    BasicoShoot();
                    break;
                case m_Attackpositions.DISPERSION:
                    DispersionShoot();
                    break;
                case m_Attackpositions.RAFAGA:
                    StartCoroutine(RafagaShoot());
                    break;
            }
        }

        if (m_Shuricken <= 0)
        {
            Debug.Log("Recargando Shurickens por quedarme sin.");
            StartCoroutine(recarga());
        }
        Debug.Log("Me quedan " + m_Shuricken + " Shurickens.");
    }


    private void PChange(InputAction.CallbackContext context)
    {
        switch (m_position)
        {
            case m_Attackpositions.BASICO:
                m_position = m_Attackpositions.DISPERSION;
                OnActualitzarPostura("Postura de Dispersión");
                break;
            case m_Attackpositions.DISPERSION:
                m_position = m_Attackpositions.RAFAGA;
                OnActualitzarPostura("Postura de Ráfaga");
                break;
            case m_Attackpositions.RAFAGA:
                m_position = m_Attackpositions.BASICO;
                OnActualitzarPostura("Postura Básica");
                break;
        }


    }

    public void BasicoShoot()
    {
        RaycastHit hit;
        Vector3 direction = m_FPC.transform.forward;
        Physics.Raycast(m_FPC.transform.position, direction, out hit, Mathf.Infinity);
        Debug.DrawLine(m_FPC.transform.position, hit.point, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), 2f);
        Debug.Log("Disparo Shurickens de forma normal.");
        m_Shuricken--;
        OnActualitzarMunicio.Invoke(m_Shuricken);
    }

    public void DispersionShoot()
    {
        RaycastHit hit;
        for (int i = 0; i < m_Shuricken; i++)
        {
            float rightDispersion = Random.Range(-0.5f, .5f);
            Vector3 direction = m_FPC.transform.forward + rightDispersion * m_FPC.transform.right;
            Physics.Raycast(m_FPC.transform.position, direction, out hit, Mathf.Infinity);
            Debug.DrawLine(m_FPC.transform.position, hit.point, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), 2f);

        }

        m_Shuricken -= m_Shuricken;
        OnActualitzarMunicio.Invoke(m_Shuricken);
    }

    IEnumerator RafagaShoot()
    {
        playerinputs.Player_Walking.Shoot.performed -= Shoot;
        RaycastHit hit;
        int m_disparados = m_Shuricken;
        if (m_Shuricken < 3)
        {
            for (int i = 0; i < m_disparados; i++)
            {
                float rightDispersion = Random.Range(-0.01f, .01f);
                float upDispersion = Random.Range(-0.01f, .01f);
                Vector3 direction = m_FPC.transform.forward + rightDispersion * m_FPC.transform.right + upDispersion * m_FPC.transform.up;
                Physics.Raycast(m_FPC.transform.position, direction, out hit, Mathf.Infinity);
                Debug.DrawLine(m_FPC.transform.position, hit.point, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), 2f);
                m_Shuricken -= 1;
                OnActualitzarMunicio.Invoke(m_Shuricken);
                yield return new WaitForSeconds(0.3f);

            }

        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                float rightDispersion = Random.Range(-0.01f, .01f);
                float upDispersion = Random.Range(-0.01f, .01f);
                Vector3 direction = m_FPC.transform.forward + rightDispersion * m_FPC.transform.right + upDispersion * m_FPC.transform.up;
                Physics.Raycast(m_FPC.transform.position, direction, out hit, Mathf.Infinity);
                Debug.DrawLine(m_FPC.transform.position, hit.point, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)), 2f);
                m_Shuricken -= 1;
                OnActualitzarMunicio.Invoke(m_Shuricken);
                yield return new WaitForSeconds(0.3f);

            }

        }
        playerinputs.Player_Walking.Shoot.performed += Shoot;

    }

    private void Recharge(InputAction.CallbackContext context)
    {
        if (recargando == false)
            Debug.Log("Recargando Shurickens por recarga.");
        StartCoroutine(recarga());
    }

    IEnumerator recarga()
    {
        recargando = true;
        playerinputs.Player_Walking.Shoot.performed -= Shoot;
        yield return new WaitForSeconds(3f);
        Debug.Log("Recarga realizada.");
        m_Shuricken = m_MaxShuricken;
        OnActualitzarMunicio.Invoke(m_Shuricken);
        playerinputs.Player_Walking.Shoot.performed += Shoot;
        Debug.Log("Me quedan " + m_Shuricken + " Shurickens después de recargar.");
        recargando = false;
    }


    private void DetectInput(InputAction.CallbackContext context)
    {
        if (context.action.activeControl.device.name != "Mouse")
        {
            //Debug.Log("Esto no es un raton");
            m_Sensitivity = 120f;
        }
        else
        {
            //Debug.Log("Esto es un raton");
            m_Sensitivity = 6.5f;
        }
    }
    private void Melee(InputAction.CallbackContext context)
    {
        Debug.Log("ATAQUE MELEE, ESTA ACTIU? " + m_MeleeHit.GetComponent<BoxCollider>().enabled);
        

        if (!m_MeleeHit.GetComponent<BoxCollider>().enabled)
        {
            m_MeleeHit.GetComponent<BoxCollider>().enabled = true;
            StartCoroutine(MeleeCoorutine());
        }
       

    }
    public void Boost(float extraSpeed)
    {
        //CORRUTINA QUE CUANDO ACABA TE DEVUELVE A TU VELOCIDAD BASE

        if (m_actualSpeed >= 13)
        {
            m_actualSpeed = m_speed;
            StopCoroutine(m_ActualBoost);
        }

        m_actualSpeed += extraSpeed;
        Debug.Log("HE RECIBIDO " + extraSpeed + " VELOCIDAD EXTRA DURANTE UN TIEMPO Y MI VELOCIDAD ES " + (m_actualSpeed + extraSpeed));
        m_ActualBoost = StartCoroutine(SpeedBoost());
    }

    private IEnumerator SpeedBoost()
    {
        yield return new WaitForSeconds(m_BoostTime);
        m_actualSpeed = m_speed;

    }
    private IEnumerator MeleeCoorutine()
    {
        yield return new WaitForSeconds(m_MeleeCD);
        m_MeleeHit.GetComponent<BoxCollider>().enabled = false;

    }


}

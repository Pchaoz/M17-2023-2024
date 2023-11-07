using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerControler : MonoBehaviour
{
    //STATE MACHINE STUFF

    //STATES
    private enum States { NONE, IDLE, RUN, JUMP, LIGHTHIT, HEAVYHIT, HITCMB };
    private States m_CurrentState;
    private bool canCombo;
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
                SendDamage(m_ComboDamage);
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
            case States.HITCMB:
                m_Rb.velocity = Vector2.zero; //PEGA QUIETO
                //REPRODUCIR ANIMACION GOLPE 2
                m_Animator.Play("Player_Combo");
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

    private bool onHit;
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
        //SUBSCRIPCION AL INPUT SYSTEM
        Assert.IsNotNull(m_InputAsset);
        m_Input = Instantiate(m_InputAsset);
        m_MovementAction = m_Input.FindActionMap("Movement").FindAction("Walk");
        m_Input.FindActionMap("Movement").FindAction("Jump").performed += Jump;
        m_Input.FindActionMap("Movement").FindAction("LightHit").performed += LightHit;
        m_Input.FindActionMap("Movement").FindAction("HeavyHit").performed += HeavyHit;
        m_Input.FindActionMap("Movement").Enable();

        m_Animator = GetComponent<Animator>(); //ME GUARDO MI ANIMATOR
        m_Rb = GetComponent<Rigidbody2D>(); //ME GUARDO MI RIGIDBODY
        m_ColliderBottom = Vector3.up * GetComponent<BoxCollider2D>().size.y / 2; //ESTE CALCULO ES LA PARTE DE ABAJO DE MI COLLIDER (LA POSICION BOTTOM DEL COLLIDER)
        onHit = false; //NO ESTOY PEGANDO POR DEFECTO
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
        onHit = false;
        ChangeState(States.IDLE);
    }

    public void OpenComboWindow()
    {
        canCombo = true;
    }
    public void CloseComboWindow()
    {
        canCombo = false;
    }
    public void ReciveDamage(int damage)
    {
        //Debug.Log("He rebut: " + damage + " de mal");
        m_Hp -= damage; //RECIBO DAÑO
        OnHpChange.Raise(m_Hp); //LE COMUNICO A LA UI EL NUEVO VALOR DE MI NUEVA VIDA

        if (m_Hp == 0 || m_Hp < 0)
        {
            SceneManager.LoadScene(1); //CAMBIO DE ESCENA
            Destroy(this.gameObject); //MUERO
        }
    }

    private void SendDamage(int dmg) 
    {
        m_Hitbox.GetComponent<HitBoxController>().LoadDamage(dmg); //ENVIA EL DAÑO A LA HITBOX
    }
    private void HeavyHit(InputAction.CallbackContext actionContext)
    {
        if (!isJumping && !onHit)
        {
            ChangeState(States.HEAVYHIT);
            onHit = true;
        }
        if (canCombo)
        {
            ChangeState(States.HITCMB);

            canCombo = false;
        }
    }
    private void LightHit(InputAction.CallbackContext actionContext)
    {
        if (!isJumping && !onHit)
        {
            ChangeState(States.LIGHTHIT);
            onHit = true;
        }
        if (canCombo)
        {
            ChangeState(States.HITCMB);
            canCombo = false;
        }
    }
    private void Jump(InputAction.CallbackContext actionContext)
    {
        if (!isJumping && !onHit) //SI NO ESTA YA EN EL AIRE NI ESTA PEGANDO
        {
            Vector3 initialPos = transform.position - m_ColliderBottom; //PILO LA POSICION DESDE LA CUAL DISPARARE EL RAYCAST
            RaycastHit2D hit = Physics2D.Raycast(initialPos, Vector2.down, 0.2f, LayerMask.GetMask("Ground")); // CASTEO EL RAYCAST Y HAGO QUE SOLO PUEDA CHOACAR CONTRA EL LAYER "GROUND"
            if (hit.collider != null) //SI EL RAYCAST CHOCA CONTRA ALGO
            {
                m_Rb.velocity = new Vector2(m_Rb.velocity.x, m_JumpForce); //AÑADO LA FUERZA PARA SALTAR
                isJumping = true; //ME PONGO A SALTAR POR LO QUE PONGO LA VARIABLE A TRUE
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("He tocado algo");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))  //SI TOCO EL SUELO
        {
            //Debug.Log("JUMP RESET");
            isJumping = false; //RESETEO EL SALTO
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyHitBox"))
        {
            int dmg = collision.gameObject.GetComponent<HitBoxController>().m_Damage; //PILLO EL DAÑO DE LO QUE SEA ENEMIGO QUE ME HAYA PEGADO
            ReciveDamage(dmg); //RECIBO EL DAÑO
        }
    }
    private void OnDestroy()
    {
        //ME DESSUBSCRIBO DE LAS COSAS AL MORIR

        m_Input.FindActionMap("Movement").FindAction("LightHit").performed -= LightHit;
        m_Input.FindActionMap("Movement").FindAction("HeavyHit").performed -= HeavyHit;
        m_Input.FindActionMap("Movement").FindAction("Jump").performed -= Jump;
        m_Input.FindActionMap("Movement").Disable();
    }
}

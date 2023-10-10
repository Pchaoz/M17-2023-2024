using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.Windows;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private List<TurretInfo> m_TurretList;

    [SerializeField]
    private TextMeshProUGUI m_HpText;

    [SerializeField]
    private TextMeshProUGUI m_MoneyText;

    [SerializeField]
    private TextMeshProUGUI m_RoundsText;

    [SerializeField]
    private GameObject m_TurretPrefab;

    [SerializeField]
    private GameObject m_ShopBtn;

    [SerializeField]
    private GameObject m_ShopUI;

    [SerializeField]
    private GameInfo m_Info;

    [SerializeField]
    private bool m_BuildTurret;

    [SerializeField]
    private InputActionAsset m_PlayerActions;
    private InputActionAsset m_Actions;
    private InputAction m_TouchPosition;

    private Vector3 m_BuildSpot;

    [SerializeField]
    private Tilemap m_tilemap;

    private int selectedT;

    private void Awake()
    {
        m_Actions = Instantiate(m_PlayerActions);
       
    }
    private void OnDisable()
    {
        m_Actions.FindActionMap("Android").FindAction("TapPosition").started -= HandleTouch;
        m_Actions.FindActionMap("Android").FindAction("TapPosition").canceled -= HandleTouch;
        m_Actions.FindActionMap("Android").Disable();
    }
    private void OnEnable()
    {
        m_BuildTurret = false;
        m_TouchPosition = m_Actions.FindActionMap("Android").FindAction("TapPosition");
        m_Actions.FindActionMap("Android").FindAction("TapPosition").started += HandleTouch;
        m_Actions.FindActionMap("Android").FindAction("TapPosition").canceled += HandleTouch;
        m_Actions.FindActionMap("Android").Enable();
        ResetStats();
    }

    private void ResetStats()
    {
        m_Info.hp = 100;
        m_Info.money = 100;
        m_Info.rounds = 0;
    }

    private void Start()
    {
        m_HpText.text = "Vida: " + m_Info.hp.ToString();
        m_MoneyText.text = "Dinero: " + m_Info.money.ToString();
        m_RoundsText.text = "Ronda: " + m_Info.rounds.ToString();
    }

    private void Update()
    {
        if (m_BuildTurret)
        {
            Vector2 pointerPosition = m_TouchPosition.ReadValue<Vector2>();
            Vector3 positionPreview = m_tilemap.GetCellCenterWorld(m_tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(pointerPosition)));
        }
    }
    public void IsRoundOver(bool isOver)
    {
        if (isOver)
        {
            m_ShopBtn.SetActive(true);
            RoundEnded(isOver);
        }
        else
        {
            //COMPROVAR CERRAR TIENDA SI ACABA EL TIEMPO DE COMPRA NO SOLO DESACTIVAR EL BOTON XD
            CloseShop();
            m_ShopBtn.SetActive(false); 
        }
    }

    public void RoundEnded(bool ended)
    {
        if (ended)
        {
            m_Info.rounds++; //ACTUALIZO LA VARIABLE DE LAS RONDAS
            m_RoundsText.text = "Ronda: " + m_Info.rounds.ToString();
        }
    }

    public void EnemyKilled(int gold)
    {
        m_Info.money += gold; //ACTUALIZO LA VARIABLE DEL DINERO
        m_MoneyText.text = "Dinero: " + m_Info.money.ToString(); //ACTUALIZO EL DINERO

    }
    public void EnemyEscaped(int dmg)
    {
        m_Info.hp -= dmg; //ACTUALIZO LA VARIABLE DE LA VIDA
        m_HpText.text = "Vida: " + m_Info.hp.ToString();

        if (m_Info.hp < 1)
        {
            //ME MUERO Y CAMBIO DE ESCENA
            SceneManager.LoadScene(2);
        }
    }

    public void OpenShop()
    {
        m_ShopUI.SetActive(true); //ACTIVO LA GUI DE LA TIENDA 
        m_ShopBtn.SetActive(false); //OCULTO EL BOTON PARA ABRIRLA
    }

    public void CloseShop()
    {
        m_ShopUI.SetActive(false); //OCULTO LA GUI DE LA TIENDA
        m_ShopBtn.SetActive(true); //MUESTRO EL BOTON PARA ABRIR LA TIENDA
    }

    public void BuyTurret(int turret)
    {
        if (m_Info.money >= m_TurretList[turret].cost)
        {
            m_Info.money -= m_TurretList[turret].cost; //GASTO EL DINERO
            m_MoneyText.text = "Dinero: " + m_Info.money.ToString(); //LO ACTUALIZO A LO CUTRE ( LO SIENTO HECTOR )

            selectedT = turret;


            CloseShop(); //CIERRO LA INTERFAZ DE COMPRA PARA QUE PUEDA COLOCAR LA TORRETA LIBREMENTE

            m_BuildTurret = true;
        }
        else
        {
            //PODRIA HACERLO CON UN TEXTO Y UNA CORRUTINA, OPCIONAL
            Debug.Log("NO ENOGHT MONEY");
        }
    }

    private void HandleTouch (InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (m_BuildTurret)
            {
                Vector2 pointerPosition = m_TouchPosition.ReadValue<Vector2>();
                m_BuildSpot = m_tilemap.GetCellCenterWorld(m_tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(pointerPosition)));
            }

        }
        else if (context.canceled)
        {
            if (m_BuildTurret)
            {
                GameObject turret = Instantiate(m_TurretPrefab);
                turret.GetComponent<Turret>().LoadInfo(m_TurretList[selectedT]);
               
                turret.transform.position = m_BuildSpot;

                m_BuildTurret = false;
            }
        }
    }
}

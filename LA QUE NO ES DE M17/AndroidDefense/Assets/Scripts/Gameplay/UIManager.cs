using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_HpText;

    [SerializeField]
    private TextMeshProUGUI m_MoneyText;

    [SerializeField]
    private TextMeshProUGUI m_RoundsText;

    [SerializeField]
    private GameObject m_ShopBtn;

    [SerializeField]
    private GameObject m_ShopUI;

    [SerializeField]
    private GameInfo m_Info;

    private void Start()
    {
        m_HpText.text = "Vida: " + m_Info.hp.ToString();
        m_MoneyText.text = "Dinero: " + m_Info.money.ToString();
        m_RoundsText.text = "Ronda: " + m_Info.rounds.ToString();
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
}

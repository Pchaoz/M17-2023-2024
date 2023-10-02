using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //STATS PLAYER
    [SerializeField]
    private int m_Hp;

    [SerializeField]
    private int m_Money;

    [SerializeField]
    private int m_Rounds;


    //ESTO NO DEBERIA ESTAR EN EL GAME MANAGER PORQUE CREO QUE EXPLOTARA AL CAMBIO DE ESCENAS
    [SerializeField]
    private TextMeshProUGUI m_HpText;
    [SerializeField]
    private TextMeshProUGUI m_MoneyText;
    [SerializeField]
    private TextMeshProUGUI m_RoundText;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Reset();
    }
    private void Update()
    {
        //Hola soy una linea de codigo que no hace na
    }
    public void RoundEnded(int plus)
    {
        Debug.Log("SIGUENTE RONDA CRACK");
        m_Rounds += plus; //ACTUALIZO LA VARIABLE DE LAS RONDAS
        m_RoundText.text = "Ronda: " + m_Rounds; //ACTUALIZO EL TEXTO
    }

    public void EnemyKilled(int gold)
    {
        m_Money += gold; //ACTUALIZO LA VARIABLE DEL DINERO
        m_MoneyText.text = "Diners: " + m_Money; //ACTUALIZO EL TEXTO
    }
    public void EnemyEscaped(int dmg)
    {
        m_Hp -= dmg; //ACTUALIZO LA VARIABLE DE LA VIDA
        m_HpText.text = "Vida: " + m_Hp; //ACTUALIZO LA VARIABLE DE LA VIDA
    }
    private void Reset()
    {
        //COSAS INCIALIZADAS POR DEFECTO
        m_Hp = 100;
        m_Money = 100;
        m_Rounds = 1;
    }
}

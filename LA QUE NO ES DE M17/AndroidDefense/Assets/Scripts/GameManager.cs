using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //STATS PLAYER
    private int m_Hp;
    private int m_Money;
    private int m_Rounds;

    [SerializeField]
    private TextMeshProUGUI m_HpText;
    [SerializeField]
    private TextMeshProUGUI m_MoneyText;

    private void Awake()
    {

        //COSAS INCIALIZADAS POR DEFECTO
        m_Hp = 100;
        m_Money = 100;
        m_Rounds = 1;
    }
    private void Update()
    {
        //Hola soy una linea de codigo que no hace na
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
}

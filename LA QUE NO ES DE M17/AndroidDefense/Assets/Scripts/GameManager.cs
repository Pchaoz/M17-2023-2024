using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //STATS PLAYER
    private int m_Hp;
    private int m_Money;
    private int m_Rounds;

    private void Awake()
    {
        m_Hp = 10;
        m_Money = 100;
        m_Rounds = 1;
    }
    private void Update()
    {
        
    }
}

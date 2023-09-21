using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Transform[] path;

    private SpriteRenderer m_Sprite;

    //STATS ENEMIGOS
    [SerializeField]
    private int m_Hp;
    [SerializeField]
    private int m_Speed;
    [SerializeField]
    private int m_Gold;
    [SerializeField]
    private int m_Dmg;


    private void Awake()
    {
        m_Sprite = GetComponent<SpriteRenderer>();
    }

    public void LoadInfo (EnemyInfo info)
    {
        m_Hp = info.hp;
        m_Speed = info.velocity;
        m_Gold = info.gold;
        m_Dmg = info.damage;
        m_Sprite.color = info.color;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    private int m_Attackspeed;
    [SerializeField]
    private int m_Damage;


    private SpriteRenderer m_Sprite;
    private void Awake()
    {
        m_Sprite = GetComponent<SpriteRenderer>();
    }
    public void LoadInfo(TurretInfo info)
    {
        m_Attackspeed = info.attackspeed;
        m_Damage = info.damage;
        m_Sprite.color = info.color;
    }
    void Update()
    {
        
    }
}

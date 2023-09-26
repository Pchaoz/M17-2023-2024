using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //COSAS DE LOS CHECKPOINTS
    [SerializeField]
    private List<Transform> m_Path;
    private float changeDistance = 0.2f;
    private int waypointPos = 0;

    //EVENTOS
    [SerializeField]
    private GameEventInteger m_Death;
    [SerializeField]
    private GameEventInteger m_Escaped;

    //EL SPRITE PARA CAMBIARLO DE COLOR
    private SpriteRenderer m_Sprite;

    //STATS ENEMIGOS
    [SerializeField]
    private int m_Hp;
    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private int m_Gold;
    [SerializeField]
    private int m_Dmg;


    private void Awake()
    {
        m_Sprite = GetComponent<SpriteRenderer>();
    }

    public void LoadInfo (EnemyInfo info, List<Transform> path)
    {
        m_Hp = info.hp;
        m_Speed = info.velocity;
        m_Gold = info.gold;
        m_Dmg = info.damage;
        m_Sprite.color = info.color;
        m_Path = path;
    }

    private void Update()
    {
        if (m_Hp < 1)
        {
            m_Death.Raise(m_Gold); //AVISO DE QUE HE MUERTO :(
            Destroy(gameObject); //MUERTE
        }

        transform.position = Vector3.MoveTowards(transform.position, m_Path[waypointPos].transform.position, m_Speed * Time.deltaTime);
        if (!(waypointPos == m_Path.Count - 1))
        {

            if (Vector3.Distance(transform.position, m_Path[waypointPos].transform.position) < changeDistance)
            {
                waypointPos++;
            }
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Finish")
        { 
            m_Escaped.Raise(m_Dmg); //AVISO DE QUE ME HE "ESCAPADO" Y TE HAGO DAÑO
            Destroy(gameObject); //PENSABAS QUE HABIAS ESCAPADO DE LA MUERTE?
        }
    }
}

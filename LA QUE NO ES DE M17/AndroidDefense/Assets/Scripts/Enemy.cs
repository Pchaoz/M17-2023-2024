using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private List<Transform> m_Path;

    private float changeDistance = 0.2f;
    private int waypointPos = 0;

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
        transform.position = Vector3.MoveTowards(transform.position, m_Path[waypointPos].transform.position, m_Speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, m_Path[waypointPos].transform.position) < changeDistance)
        {
            waypointPos++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HE TRIGEREADO ALGO");

        if (collision.gameObject.tag == "Finish")
        {
            Destroy(gameObject);
        }
    }
}

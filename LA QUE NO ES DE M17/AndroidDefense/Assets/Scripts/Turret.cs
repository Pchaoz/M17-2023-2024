using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField]
    private int m_Attackspeed;
    [SerializeField]
    private int m_Damage;

    [SerializeField]
    private float m_BulletSpeed;

    [SerializeField]
    private GameObject m_BalaPrefab;

    //LLISTA DE ENEMICS DETECTATS
    List<GameObject> m_EnemyList = new List<GameObject>();

    private IEnumerator m_Shoot;


    private SpriteRenderer m_Sprite;
    private void Awake()
    {
        m_Sprite = GetComponent<SpriteRenderer>();
        m_Shoot = Shoot();
    }
    public void LoadInfo(TurretInfo info)
    {
        m_Attackspeed = info.attackspeed;
        m_Damage = info.damage;
        m_Sprite.color = info.color;
        m_BulletSpeed = 1f;
    }
    void Update()
    {
        if (m_EnemyList.Count > 0)
        {
            StartCoroutine(m_Shoot);
        }else
        {
            StopCoroutine(m_Shoot);
        }
    }
    IEnumerator Shoot()
    {
        while(m_EnemyList.Count > 0)
        {
            GameObject shoot = Instantiate(m_BalaPrefab);
            shoot.GetComponent<Bullet>().LoadInfo(m_Damage, m_BulletSpeed, m_EnemyList[0]);
            yield return new WaitForSeconds(m_Attackspeed);
        }
       
    }

        public void EnemyToList (GameObject enemy)
    {
        m_EnemyList.Add(enemy);
        Debug.Log("ENEMIES ON THE LIST: " + m_EnemyList.Count);
    }
    public void RemoveEnemy(GameObject enemy)
    {
        m_EnemyList.Remove(enemy); //FUNCIONA??
        Debug.Log("ENEMIES ON THE LIST: " + m_EnemyList.Count);
    }
}

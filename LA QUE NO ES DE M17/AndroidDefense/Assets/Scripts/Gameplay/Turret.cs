using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Turret : MonoBehaviour
{
    private AudioSource m_Source;

    [SerializeField]
    private int m_Attackspeed;
    [SerializeField]
    private int m_Damage;

    [SerializeField]
    private float m_BulletSpeed;

    [SerializeField]
    private GameObject m_BalaPrefab;

    bool isShooting = false;

    //LLISTA DE ENEMICS DETECTATS
    List<GameObject> m_EnemyList = new List<GameObject>();

    private IEnumerator m_Shoot;


    private SpriteRenderer m_Sprite;
    private void Awake()
    {
        m_Source = GetComponent<AudioSource>();
        m_Sprite = GetComponent<SpriteRenderer>();
        m_Shoot = Shoot();
    }
    public void LoadInfo(TurretInfo info)
    {
        m_Attackspeed = info.attackspeed;
        m_Damage = info.damage;
        m_Sprite.color = info.color;
        m_BulletSpeed = 25f;
    }
    void Update()
    {
        if (m_EnemyList.Count > 0)
        {
            if (!isShooting)
            {
                StartCoroutine(m_Shoot);
            }
        }else
        {
            StopCoroutine(m_Shoot);
            isShooting = false;


        }
    }
    IEnumerator Shoot()
    {
        while(m_EnemyList.Count > 0)
        {
            m_Source.Play();
            GameObject shoot = Instantiate(m_BalaPrefab);
            shoot.GetComponent<Bullet>().LoadInfo(m_Damage, m_BulletSpeed, m_EnemyList[0]);
            shoot.transform.position = transform.position;
            isShooting = true;
            yield return new WaitForSeconds(m_Attackspeed);
            isShooting = false;
        }
        isShooting = false;


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

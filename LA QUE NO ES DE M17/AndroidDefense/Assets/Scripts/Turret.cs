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
    private GameObject m_BalaPrefab;

    //LLISTA DE ENEMICS DETECTATS
    List<GameObject> m_EnemyList = new List<GameObject>();


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
        if (m_EnemyList.Count > 0)
        {
            //NECESITO EL PREFAB DE LA BALA PA SEGUIR
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

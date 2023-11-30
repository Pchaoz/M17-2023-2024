using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    float m_SpawnTime;
    [SerializeField]
    GameObject m_EnemyPrefab1;
    [SerializeField]
    GameObject m_EnemyPrefab2;
    //Por si hacemos varios enemigos
    [SerializeField]
    List<GameObject> m_SpawnList;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            Instantiate(m_EnemyPrefab1, transform);
            yield return new WaitForSeconds(m_SpawnTime);
        }
    }
}

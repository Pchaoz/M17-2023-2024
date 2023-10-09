using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private List<Transform> path;
    [SerializeField]
    private GameObject enemyToSpawn;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private EnemyInfo[] enemyInfos;

    private IEnumerator m_SpawnCorutine;

    [SerializeField]
    private int max_spawned; //SE LO HA DE ENVIAR EL GAME MANAGER (LA CANTIDAD DE ENEMIGOS POR OLEADA) ADEMAS A CADA RONDA SE HAN DE DUPLICAR

    [SerializeField]
    private float cooldown; //TIEMPO ENTRE OLEADAS

    [SerializeField]
    private int spawned; //LOS QUE LLEVA SPAWNEADOS ESA RONDA

    [SerializeField]
    private float spawnrate; //EL CD DEL SPAWN

    [SerializeField]
    private GameObject m_Warningtxt;

    [SerializeField]
    private GameEventBoolean m_RoundOver;



    private void Awake()
    {
        spawnrate = 5f;
        spawned = 0;
        max_spawned = 10;
    }

    private void Start()
    {
        StartCoroutine(SpawnNextWave());
    }

    IEnumerator SpawnWave()
    {
        spawned = 0;
        //Debug.Log("HE ENTRADO");
        while (spawned < max_spawned)
        {
            int infoToSel = Random.Range(0, enemyInfos.Length); //ESCOJO LA INFORMACION DEL ENEMIGO QUE QUIERO SPAWNERAR ALEATORIAMENTE

            GameObject newEnemy = Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity); //SPAWN TACTICO DEL ENEMIGO
            newEnemy.GetComponent<Enemy>().LoadInfo(enemyInfos[infoToSel], path); //LE PASO LA INFORMACION

            spawned++; //SUMO EL CONTADOR DE LA OLEADA
            yield return new WaitForSeconds(spawnrate); //ESPERO AL SIGUIENTE
        }
        StartCoroutine(SpawnNextWave());
    }

    IEnumerator SpawnNextWave()
    {
        m_RoundOver.Raise(true);
        m_Warningtxt.SetActive(true);
        yield return new WaitForSeconds(cooldown);
        max_spawned += 5;
        m_Warningtxt.SetActive(false);
        m_RoundOver.Raise(false);
        StartCoroutine(SpawnWave());
    }
}

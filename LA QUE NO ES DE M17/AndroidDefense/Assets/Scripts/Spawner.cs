using System.Collections;
using System.Collections.Generic;
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

    private int max_spawned; //SE LO HA DE ENVIAR EL GAME MANAGER (LA CANTIDAD DE ENEMIGOS POR OLEADA) ADEMAS A CADA RONDA SE HAN DE DUPLICAR
    private int spawned; //LOS QUE LLEVA SPAWNEADOS ESA RONDA
    private float spawnrate; //EL CD DEL SPAWN
    private bool isSpawning; //BOOLEAN PARA SABER SI ESTA SPAWNEANDO O NO

    private void Awake()
    {
        spawnrate = 5f;
        spawned = 0;
        max_spawned = 10;
        isSpawning = false;
    }

    private void Start()
    {
       if (!isSpawning)
       {
            StartCoroutine(SpawnWave());
       }
    }

    IEnumerator SpawnWave()
    {
        //Debug.Log("HE ENTRADO");
        while (spawned < max_spawned)
        {
            int infoToSel = Random.Range(0, enemyInfos.Length); //ESCOJO LA INFORMACION DEL ENEMIGO QUE QUIERO SPAWNERAR ALEATORIAMENTE

            GameObject newEnemy = Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity); //SPAWN TACTICO DEL ENEMIGO
            newEnemy.GetComponent<Enemy>().LoadInfo(enemyInfos[infoToSel], path); //LE PASO LA INFORMACION

            spawned++; //SUMO EL CONTADOR DE LA OLEADA
            yield return new WaitForSeconds(spawnrate); //ESPERO AL SIGUIENTE
        }
        //LLAMO AL METODO QUE PARA LA WAVE Y ENVIA EL INVOKE PARA AVISAR QUE HAS PASADO DE RONDA
    }
}

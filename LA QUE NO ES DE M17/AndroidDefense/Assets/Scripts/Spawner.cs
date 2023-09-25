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

    private int max_wave;
    private int wave;
    private float spawnrate;
    private bool isSpawning;

    private void Awake()
    {
        spawnrate = 5f;
        wave = 0;
        max_wave = 10;
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
        while (wave < max_wave)
        {
            int infoToSel = Random.Range(0, enemyInfos.Length); //ESCOJO LA INFORMACION DEL ENEMIGO QUE QUIERO SPAWNERAR ALEATORIAMENTE

            GameObject newEnemy = Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);
            newEnemy.GetComponent<Enemy>().LoadInfo(enemyInfos[0], path); //LE PASO LA INFORMACION

            wave++; //SUMO EL CONTADOR DE LA OLEADA
            yield return new WaitForSeconds(spawnrate); //ESPERO AL SIGUIENTE
        }
        //LLAMO AL METODO QUE PARA LA WAVE Y ENVIA EL INVOKE PARA AVISAR QUE HAS PASADO DE RONDA
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
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
        spawnrate = 1f;
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
            Debug.Log("GENERO A UN ENEMIGO");
            GameObject newEnemy = Instantiate(enemyToSpawn, spawnPoint.position, Quaternion.identity);
            wave++;
            newEnemy.GetComponent<Enemy>().LoadInfo(enemyInfos[0]);
            //ESPERO AL SIGUIENTE
            yield return new WaitForSeconds(spawnrate);
        }
    }
}

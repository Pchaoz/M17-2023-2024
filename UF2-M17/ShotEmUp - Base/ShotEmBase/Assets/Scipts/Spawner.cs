using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_SpawnType; //DOS TIPOS DE ENEMIGOS A SPAWNEAR

   
    [SerializeField]
    private int m_SpawnCD; //COOLDOWN ENTRE GENERACION DE ENEMIGOS
    private int m_WaveSize; //TAMAÑO MAXIMO OLEADA
    private int m_ToKill; //CONTADOR DE ENEMIGOS ANTES DE PASAR DE RONDA
    private int m_SpawnedEnemies;
    [SerializeField]
    private RoundsInfo m_Round;
    [SerializeField]
    private float m_RoundsCD;
    [SerializeField]
    private GameEvent1Int onRoundChange;

    private void Start()
    {
        m_SpawnedEnemies = 0; //CONTADOR DE ENEMIGOS SPAWNEADOS A 0
        m_Round.round = 1; //RONDA INICIAL PARTIDA
        m_WaveSize = 2; //TAMAÑO MAXIMO OLEADA INICIAL
        m_ToKill = m_WaveSize;
        StartCoroutine(SpawnWave()); //EMPIEZO A GENERAR ENEMIGOS
        onRoundChange.Raise(m_Round.round); //EVENTO QUE MANDA QUE RONDA ES A LA UI

    }

    //EL SPAWNER QUE GENERA A LOS ENEMIGOS
    private IEnumerator SpawnWave()
    {

        //Debug.Log("HAS DE MATAR: " + m_ToKill + " enemigos");
        while (m_SpawnedEnemies < m_WaveSize) 
        {
            GameObject Enemy;
            if (m_Round.round > 1) //SOLO CONTROLO LA PRIMERA RONDA PORQUE ES LA QUE SOLO SPAWNEA MELEE
            {
                Enemy = Instantiate(m_SpawnType[Random.Range(0, m_SpawnType.Count)]); //SPAWNEO UN ENEMIGO ALEATORIO
                Enemy.transform.position = transform.position; //LE ASIGNO LA POSICION DEL SPAWNER
            }
            else
            {
                Enemy = Instantiate(m_SpawnType[0]); //SPAWNEO EL ENEMIGO MELEE
                Enemy.transform.position = transform.position; //LE ASIGNO LA POSICION DEL SPAWNER
            }
            Enemy.GetComponent<CanDie>().DeathEvent += OnEnemyDie; //ME SUBSCRIBO AL EVENTO
            m_SpawnedEnemies++; //AÑADO UN ENEMIGO SPAWNEADO A LA LISTA
            yield return new WaitForSeconds(m_SpawnCD); //ESPERO PARA SPAWNEAR AL SIGUIENTE
        }
        
    }

    //HACE UNA PAUSA ENTRE OLEADA Y OLEADA PARA QUE AL JUGADOR LE DE TIEMPO A RESPIRAR UN POCO
    IEnumerator WaitNewWave()
    {
        Debug.Log("HA ACABADO LA RONDA, PREPARATE PARA LA SIGUENTE"); //TEST DEBUG
                                                                      //AVISO QUE LA RONDA HA ACABADO CON UN TEXTO O ALGO (HACE EVENTO)
        yield return new WaitForSeconds(m_RoundsCD); // 30 SEGUNDOS HASTA LA PROXIMA RONDA
        m_WaveSize += 2; //SPAWNEO 2 MAS LA PROXIMA RONDA
        m_ToKill = m_WaveSize;
        m_Round.round++; //INCREMENTA EL NUMERO DE RONDAS QUE HAS SOBREVIVIDO
        m_SpawnedEnemies = 0; //REINICIO LA CANTIDAD DE ENEMIGOS QUE HE SPAWNEADO
        onRoundChange.Raise(m_Round.round); //ME FALTA UN EVENTO QUE AVISE A LA GUI DE QUE HA CAMBIADO LA RONDA
        StartCoroutine(SpawnWave()); //ACTIVO LA COORUTINA DE SPAWN DE OLEADA
    }

    //EL METODO QUE ELIMINA A LOS ENEMIGOS AL MORIR
    //RECIBE EL ENEMIGO A ELMINIAR DE LA LISTA
    void OnEnemyDie(GameObject enemyToRemove)
    {
        enemyToRemove.GetComponent<CanDie>().DeathEvent -= OnEnemyDie; //AL MORIR EL ENEMIGO ME DEJO DE ESTAR SUBSCRITO
        m_ToKill--;
        //Debug.Log("QUEDAN POR MATAR: " + m_ToKill + " enemigos");
        if (m_ToKill == 0) //PUES ESO, QUE SI NO QUEDAN ENEMIGOS QUE MATAR CAMBIAS DE RONDA
            StartCoroutine(WaitNewWave()); //ACTIVO LA COORUTINA DE ESPERA ENTRE RONDAS

    }
}

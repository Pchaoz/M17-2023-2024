using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_EnemiesList; //DOS TIPOS DE ENEMIGOS A SPAWNEAR

    [SerializeField]
    private int m_Cooldown;
    [SerializeField]
    private int m_WaveSize;
}

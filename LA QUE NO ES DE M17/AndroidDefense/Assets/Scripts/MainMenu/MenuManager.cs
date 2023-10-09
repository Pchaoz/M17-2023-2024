using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField]
    private GameInfo m_Info;

    public void LoadGame()
    {

        //PONGO TODOS LOS STATS A EL VALOR POR DEFECTO
        m_Info.hp = 100;
        m_Info.money = 20;
        m_Info.rounds = 0;

        
        SceneManager.LoadScene(1); //CARGO LA ESCENA DE LA PARTIDA
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private GameInfo m_Info; //SCRIPTABLE CON TODA LA INFO

    [SerializeField]
    private TextMeshProUGUI m_LoseText; //TEXTO DERROTA


    void Start()
    {
        int rounds = m_Info.rounds - 1; //HAGO EL CALCULO A PARTE PORQUE SI NO AL STRING LE MOLESTA
        m_LoseText.text = "HAS SOBREVIVIDO " + rounds + " RONDAS"; //PONGO EL TEXTO DE DERROTA
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0); //CARGO EL MENU PRINCIPAL
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //VARIABLES
    [SerializeField]
    private TextMeshProUGUI txt_score;
    private float currentScore;
    private float clickScore;
    private float scorePerSecond;
    private float score;
 
    void Start()
    {
        scorePerSecond = 0;
        currentScore = 0;
        clickScore = 1;
    }

    void Update()
    {
        scorePerSecond = scorePerSecond * Time.deltaTime;
        currentScore = currentScore + scorePerSecond;
        txt_score.text = "Convalidaciones: " + currentScore;
    }
}

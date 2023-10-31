using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_RoundsText;

    [SerializeField]
    private RoundsInfo info;

    private void Awake()
    {
        info.round -= 1;
        m_RoundsText.text = "Has sobrevivido " + info.round + " rondas!";
    }
    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }
}

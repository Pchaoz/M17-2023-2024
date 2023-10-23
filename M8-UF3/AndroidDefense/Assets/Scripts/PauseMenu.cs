using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    GameObject m_PauseMenu;

    public void Pause()
    {
        m_PauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        m_PauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Home(int sceneID)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(sceneID);
    }
}

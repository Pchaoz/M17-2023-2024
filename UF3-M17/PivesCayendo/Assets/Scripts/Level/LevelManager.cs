using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_StartWallList = new List<GameObject>();
    [SerializeField]
    private float m_StartCooldown;

    private void Start()
    {
        //ACTIVAR CUENTA ATRAS
        StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown()
    {

        yield return new WaitForSeconds(m_StartCooldown);
        foreach (GameObject wall in m_StartWallList)
        {
            wall.SetActive(false);
        }
    }
}

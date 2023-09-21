using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform[] path;
    private int hp;
    private int speed;
    private int gold;
    private int dmg;

   
    private void Start()
    {
    
    }

    public void LoadInfo (EnemyInfo info)
    {
        Debug.Log(info);
    }
}

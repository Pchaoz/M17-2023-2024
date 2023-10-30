using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public Action<bool> HitPlayerEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("ESTOY EN AREA DE PEGAR AL PLAYER");
            HitPlayerEvent?.Invoke(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            HitPlayerEvent?.Invoke(false);
        }
    }
}

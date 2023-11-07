using System;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public Action<bool> HitPlayerEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("ESTOY EN AREA DE PEGAR AL PLAYER");
            HitPlayerEvent?.Invoke(true);
        }
    }
}

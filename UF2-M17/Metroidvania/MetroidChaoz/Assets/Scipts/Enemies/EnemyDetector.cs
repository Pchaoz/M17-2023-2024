using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public Action<GameObject> FollowPlayerEvent;
    public Action<bool> UnfollowPlayerEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("HE DETECTADO A UN JUGADOR");
            FollowPlayerEvent?.Invoke(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            UnfollowPlayerEvent?.Invoke(true);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public Action<GameObject> FollowPlayerEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FollowPlayerEvent?.Invoke(collision.gameObject);
        }
    }
}

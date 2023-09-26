using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretArea : MonoBehaviour
{
    [SerializeField]
    GameObject m_Turret;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("ENEMY COLLIDED, ADDING TO THE LIST");
            m_Turret.GetComponent<Turret>().EnemyToList(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("THE ENEMY DID RUN AWAY, REMOVE HIM");
            m_Turret.GetComponent<Turret>().RemoveEnemy(collision.gameObject);
        }
    }
}

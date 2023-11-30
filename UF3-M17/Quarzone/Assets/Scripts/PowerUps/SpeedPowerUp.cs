using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    float m_Speed = 30f;
    float m_SpeedBoost = 10f;

    [SerializeField]
    GameEvent1Float m_Event;
    private void Update()
    {
        transform.Rotate(Vector3.up * m_Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Boost de velocidad");
            m_Event.Raise(m_SpeedBoost);
            Destroy(this);
        }
        
    }

}

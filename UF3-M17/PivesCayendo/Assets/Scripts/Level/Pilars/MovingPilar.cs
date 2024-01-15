using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPilar : MonoBehaviour
{
    [SerializeField]
    private int m_Direction;
    private float m_Speed = 20f;
    private Rigidbody m_Rb;
    [SerializeField]
    private float m_Force;


    void Start()
    {
        m_Rb = GetComponent<Rigidbody>();

        m_Direction = Random.Range(-1, 1);
       
        if (m_Direction == 0)
            m_Direction = 1;
    }

    void FixedUpdate()
    {
        m_Rb.MovePosition(transform.position + m_Direction * transform.right * m_Speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.rigidbody.AddForce(new Vector3(0, m_Force, m_Force), ForceMode.Force);
        }
            //
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("MapBorder"))
        {
            m_Direction *= -1;
        }
    }
}

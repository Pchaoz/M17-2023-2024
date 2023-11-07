using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    public float m_Speed;
    private Rigidbody2D m_Rb;
    [SerializeField]
    private float m_TimeToDie;
    private void Awake()
    {
        m_Rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        StartCoroutine(Autodestruction());
    }
    public void LoadShot(Vector3 position)
    {
        Vector3 dir = (position - transform.position).normalized;
        m_Rb.velocity = dir * m_Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
    }
    private IEnumerator Autodestruction()
    {
        yield return new WaitForSeconds(m_TimeToDie);
        Destroy(this.gameObject);
    }
}

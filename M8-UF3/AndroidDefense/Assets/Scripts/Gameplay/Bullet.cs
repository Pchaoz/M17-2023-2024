using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField]
    private int m_Dmg; //EL DA�O QUE HACE A LOS ENEMIGOS

    [SerializeField]
    private float m_Speed; //LA VELOCIDAD DE LA BALA

    private GameObject m_Target; //CONTRA QUIEN SE HA DE ESTAMPAR

    private void Awake()
    {
        
    }
    void Update()
    {
        if (m_Target == null)
        {
            Destroy(this.gameObject);
        }else
        {
            transform.position = Vector3.MoveTowards(transform.position, m_Target.transform.position, m_Speed * Time.deltaTime);
        }
    }

    public void LoadInfo(int dmg, float speed, GameObject target)
    {
       
        m_Dmg = dmg;
        m_Speed = speed;
        m_Target = target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //ME PEGA
            collision.gameObject.GetComponent<Enemy>().ReciveDamage(m_Dmg);
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField]
    private int m_Dmg; //EL DAÑO QUE HACE A LOS ENEMIGOS

    [SerializeField]
    private float m_Speed; //LA VELOCIDAD DE LA BALA

    private GameObject m_Target; //CONTRA QUIEN SE HA DE ESTAMPAR

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_Target.transform.position, m_Speed * Time.deltaTime);
    }

    public void LoadInfo(int dmg, float speed, GameObject target)
    {
        m_Dmg = dmg;
        m_Speed = speed;
        m_Target = target;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("SOY LA BALA Y HE CHOCADO CONRTA: " + collision.gameObject.tag);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour, CanDie
{
    public event Action<GameObject> DeathEvent; //DELEGADO DE MUERTE DEL ENEMIGO

    //EVENTO O DELEGADO EN EL QUE AVISARA QUE HA MUERTO
    void OnDeath()
    {
        DeathEvent?.Invoke(this.gameObject); //ME MUERO ASI QUE AVISO PARA BORRARME DE LA LISTA
        Destroy(this.gameObject); //ME MUERO :(
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //CODIGO TEMPORAL PARA HACER PRUEBAS CON LAS OLEADAS
            OnDeath();
        }
    }
}

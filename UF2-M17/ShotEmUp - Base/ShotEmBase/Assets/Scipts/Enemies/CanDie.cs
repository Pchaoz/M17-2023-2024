using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//INTEFACE QUE TIENE UN EVENTO GENERICO PARA LOS DOS TIPOS DE ENEMIGOS
public interface CanDie
{
    event Action<GameObject> DeathEvent; //EVENTO GENERICO PARA LOS DOS ENEMIGOS AL MORIR
}

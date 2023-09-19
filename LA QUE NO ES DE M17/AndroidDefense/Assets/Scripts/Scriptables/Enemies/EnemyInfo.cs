using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInfo", menuName = "Scriptables/EnemyInfo")]
public class EnemyInfo : ScriptableObject
{
    public int hp;
    public int damage;
    public int velocity;
    public Color color; //MAS ADELANTE ALOMEJOR INTENTO CAMBIARLO POR UN SPRITE ANIMADO
}

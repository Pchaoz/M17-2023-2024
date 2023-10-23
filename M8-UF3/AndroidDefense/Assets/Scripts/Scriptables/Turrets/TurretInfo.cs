using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TurretInfo", menuName = "Scriptables/TurretInfo")]
public class TurretInfo : ScriptableObject
{
    public int attackspeed;
    public int cost;
    public int damage;
    public Color color; //LO MIMSO SI ME DA TIEMPO ALOMEJOR INTENTO CAMBIARLO A UN SPRITE
}

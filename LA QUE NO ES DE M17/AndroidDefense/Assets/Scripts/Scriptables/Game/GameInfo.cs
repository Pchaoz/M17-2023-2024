using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInfo", menuName = "Scriptables/GameInfo")]
public class GameInfo : ScriptableObject
{
   public int hp;
   public int rounds;
   public int money;
}

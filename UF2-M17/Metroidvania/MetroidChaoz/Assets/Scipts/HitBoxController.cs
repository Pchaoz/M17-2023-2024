using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxController : MonoBehaviour
{
    public int m_Damage;
    public void LoadDamage(int dmg)
    {
        m_Damage = dmg;
    }
}

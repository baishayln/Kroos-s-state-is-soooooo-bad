using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCounter : MonoBehaviour
{
    private float damage = 0;
    public void CauseDamage(float dmg)
    {
        damage += dmg;
    }
    public float GetDamageCount()
    {
        return damage;
    }
    public void ResetDamage()
    {
        damage = 0;
    }
}

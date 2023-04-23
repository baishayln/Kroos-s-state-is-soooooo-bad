using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobleDamageCounter : MonoBehaviour
{
    private static GlobleDamageCounter instance;
    public static GlobleDamageCounter Instance
    {
        get
        {
            if (instance == null)
                instance = Transform.FindObjectOfType<GlobleDamageCounter>();
            return instance;
        }
    }
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }

        }
    }
    private float damage = 0;
    public void CauseDamage(float dmg)
    {
        damage += dmg;
    }
    public float GetDamage()
    {
        return damage;
    }
    public void ResetDamage()
    {
        damage = 0;
    }
    
}

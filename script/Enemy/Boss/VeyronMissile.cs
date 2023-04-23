using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeyronMissile : MissileFather
{
    [SerializeField]private string targetLayerMaskName = "Player";
    
    
    override public void FixedUpdate()
    {
        base.FixedUpdate();
    }
    
    override public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Ground") || other.CompareTag("Player"))
        {
            ShootGround(explosionPrefab , targetLayerMaskName);
        }
    }
}

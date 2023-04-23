using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface OnHit
{
    
    bool OnHit(float damage , float Dir);

    bool OnHit(float damage , Vector2 speed);
    
    bool OnHit(float damage);
    float GetHealth();
    float GetHealthUplimit();

    // void SetDate();
}
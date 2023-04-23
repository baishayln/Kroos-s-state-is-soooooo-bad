using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MissileLauncher
{
    void LaunchMissile(Vector3 targetpoint , Vector3 dir , float startdsts , float speed);
    void LaunchMissile(Vector3 targetpoint , Vector3 dir , float startdsts , float speed , GameObject areaObj);
    void HitPlayer(Collider2D player , float hitDir);
    void LockPlayer(Transform player);
    void ExplosionSound();

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShootPointToTarget
{
    static public void MoveFirePoint(Transform gun , Transform target , Transform shootPoint , Transform gunCenterPoint , float gunLenght)
    {
        shootPoint.position = (target.position - gunCenterPoint.position).normalized * gunLenght + gunCenterPoint.position;
        gun.position = (shootPoint.position - gunCenterPoint.position).normalized * gunLenght * 0.5f + gunCenterPoint.position;
        gun.right = shootPoint.position - gun.position;
    }
    static public void MoveFirePoint(Transform gun , Vector3 dir , Transform shootPoint , Transform gunCenterPoint , float gunLenght)
    {
        shootPoint.position = dir.normalized * gunLenght + gunCenterPoint.position;
        gun.position = (shootPoint.position - gunCenterPoint.position).normalized * gunLenght * 0.5f + gunCenterPoint.position;
        gun.right = shootPoint.position - gun.position;
    }
    static public void MoveFireGun(Transform gun , Transform shootPoint , Transform gunCenterPoint , float gunLenght)
    {
        gun.position = (shootPoint.position - gunCenterPoint.position).normalized * gunLenght * 0.5f + gunCenterPoint.position;
        gun.right = shootPoint.position - gun.position;
    }
}

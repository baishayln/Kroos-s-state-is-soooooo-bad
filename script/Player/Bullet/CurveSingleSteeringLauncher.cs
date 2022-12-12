using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveSingleSteeringLauncher : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject target;
    private Vector3 nowRotateAngle;
    private Vector3 randomRotateAngle;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent;
    }

    // Update is called once per frame
    // void Update()
    // {
        // if (Input.GetKeyDown(KeyCode.L))
        // {
            // GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
            // bullet.transform.position = transform.position;
            // bullet.GetComponent<CurveSingleSteeringProjectile>().setAngle(Vector2.up);
        //     bullet.GetComponent<CurveSingleSteeringProjectile>().setTarget(target);
        // }
    // }
    public void LaunchRocket(float rocketCount , float damage)
    {
        nowRotateAngle.z = 360f/rocketCount;
        randomRotateAngle.z = Random.Range(1 , nowRotateAngle.z);
        transform.Rotate(randomRotateAngle);
        while (transform.localEulerAngles.z % nowRotateAngle.z == 0)
        {
            randomRotateAngle.z = Random.Range(1 , nowRotateAngle.z);
            transform.Rotate(randomRotateAngle);
        }
        for (int i = 0; i < rocketCount; i++)
        {
            GameObject rocket = ObjectPool.Instance.GetObject(bulletPrefab);
            rocket.transform.position = player.position;
            transform.Rotate(nowRotateAngle);
            rocket.GetComponent<CurveSingleSteeringProjectile>().setAngle(transform.up);
            rocket.GetComponent<CurveSingleSteeringProjectile>().setDamage(damage);
        }
    }
    private void SetRocket()
    {

    }
}

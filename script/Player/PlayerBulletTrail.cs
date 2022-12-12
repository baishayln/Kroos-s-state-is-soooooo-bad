using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletTrail : MonoBehaviour
{
    private float deadTimer;
    [SerializeField]public TrailRenderer trailRenderer;
    // Update is called once per frame
    void Update()
    {
        if (deadTimer > 0 && deadTimer <= Time.deltaTime)
        {
            trailRenderer.enabled = false;
            ObjectPool.Instance.PushObject(gameObject);
        }
        deadTimer -= Time.deltaTime;
    }

    public void StopTrail(float deadtime)
    {
        transform.SetParent(null);
        deadTimer = deadtime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeyronCGMissileAimArea : MonoBehaviour
{
    [SerializeField]private float aliveTime = 0.75f;
    private float aliveTimer;
    private MissileLauncher launcher;
    private float launcherSpeed;
    private float areaWidth;
    private Vector3 area = Vector3.one;
    private Vector3 dir;
    private Color color;
    // Start is called before the first frame update
    void OnEnable()
    {
        transform.GetComponent<SpriteRenderer>().color = Vector4.one;
        color = transform.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (aliveTimer > 0)
        {
            aliveTimer -= Time.deltaTime;
            if (aliveTimer <= 0)
            {
                launcher.LaunchMissile(transform.position , transform.up * -1 , (CameraBehaviour.Instance.ReturnCameraY() * 0.6f + (CameraBehaviour.Instance.ReturnBornPosition().y - transform.position.y)) , launcherSpeed , gameObject);
                // ObjectPool.Instance.PushObject(gameObject);
            }
        }
    }
    public void SetData(MissileLauncher launcher , float missileSpeed , Vector3 scale)
    {
        this.launcher = launcher;
        launcherSpeed = missileSpeed;
        aliveTimer = aliveTime;
        areaWidth = scale.y;
        area = scale;
    }
    public void SetData(MissileLauncher launcher , float missileSpeed , float Y , float alivetime)
    {
        this.launcher = launcher;
        launcherSpeed = missileSpeed;
        aliveTimer = alivetime;
        areaWidth = Y;
        area.y *= Y;
    }
    public void Push()
    {
        StartCoroutine(PushSelf());
    }
    IEnumerator PushSelf()
    {
        while(color.a > 0.05f)
        {
            yield return new WaitForFixedUpdate();
            color.a -= Time.deltaTime * 2;
            transform.GetComponent<SpriteRenderer>().color = color;
        }
        ObjectPool.Instance.PushObject(gameObject);
    }
}

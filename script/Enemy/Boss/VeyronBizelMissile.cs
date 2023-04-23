using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeyronBizelMissile : MissileFather
{
    [SerializeField]protected string targetLayerMaskName = "Player";
    protected Vector3 startPos; 
    protected Vector3 midPos;
    protected Vector3 targetPos;
    protected float percentSpeed;
    protected float percent = 0;
    protected Vector2 lastPos = Vector2.zero;
    override protected void OnEnable()
    {
        base.OnEnable();
        percent = 0;
    }
    override public void FixedUpdate()
    {
        // base.FixedUpdate();
        percent += percentSpeed * Time.deltaTime;
        if (percent > 1)
        {
            percent = 1;
        }
        transform.position = Bezier(percent , startPos , midPos , targetPos);
        Rotate();
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            // ObjectPool.Instance.PushObject(areaObj);
            // areaObj = null;
            // ObjectPool.Instance.PushObject(gameObject);
            ShootGround(explosionPrefab , "Player");
        }
    }
    
    override public void SetData(MissileLauncher launcher , float speed , Vector3 dir)
    {
        base.SetData(launcher , speed , dir);
    }
    override public void SetData(MissileLauncher launcher , float speed , Vector3 dir , GameObject areaObj)
    {
        base.SetData(launcher , speed , dir , areaObj);
    }
    override public void SetData(MissileLauncher launcher , float speed , Vector3 dir , GameObject areaObj , Vector3 targetpoint)
    {
        base.SetData(launcher , speed , dir , areaObj , targetpoint);
        
        targetPos = targetpoint;
        startPos = transform.position;
        midPos = GetMiddlePosition(transform.position , targetPos);
        percentSpeed = speed / (targetPos - startPos).magnitude;
    }
    protected void Rotate()
    {
        if (lastPos != null)
        {
            transform.up = -(lastPos - (Vector2)transform.position);
        }
        lastPos = transform.position;
    }
    protected Vector2 GetMiddlePosition(Vector2 a , Vector2 b)
    {
        Vector2 m = Vector2.Lerp(a , b , 0.1f);
        Vector2 normal = Vector2.Perpendicular(a - b).normalized;
        float rd = Random.Range(-2f , 2f);
        float curveRatio = 0.3f;
        return m + (a - b).magnitude * curveRatio * rd * normal;
    }
    override public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Ground") || other.CompareTag("Player"))
        {
            ShootGround(explosionPrefab , targetLayerMaskName);
        }
    }
    protected Vector2 Bezier(float t , Vector2 a , Vector2 b , Vector2 c)
    {
        var ab = Vector2.Lerp(a , b , t);
        var bc = Vector2.Lerp(b , c , t);
        return Vector2.Lerp(ab , bc , t);
    }
    override public void ShootGround(GameObject explosionFirePre , string layerMaskName)
    {
        if (!isExplosion)
        {
            // rig.velocity = Vector2.zero;
            Collider2D player =  Physics2D.OverlapCircle(transform.position , 2 , LayerMask.GetMask(layerMaskName));
            if (player)
            {
                launcher.HitPlayer(player , player.transform.position.x - transform.position.x);
            }
            // ObjectPool.Instance.PushObject(areaObj);
            if (areaObj)
            {
                areaObj.transform.GetComponent<VeyronCGMissileAimArea>().Push();
            }
            areaObj = null;
            launcher.ExplosionSound();
            GameObject exp = ObjectPool.Instance.GetObject(explosionPrefab);
            exp.transform.position = explosionPoint.position;
            
            StartCoroutine(DestoryGameObject(0.5f));
        }
    }
}
 
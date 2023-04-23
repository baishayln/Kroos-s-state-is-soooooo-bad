using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileFather : MonoBehaviour
{
    protected float speed;
    protected Rigidbody2D rig;
    protected MissileLauncher launcher;
    protected string layerMaskName = "Ground";
    protected float timer;
    protected float lifeTime = 10;
    protected GameObject areaObj;
    [SerializeField]protected GameObject explosionPrefab;
    [SerializeField]protected Transform explosionPoint;
    protected ParticleSystem smoke;
    protected bool isExplosion;

    // Start is called before the first frame update
    virtual public void Awake()
    {
        // smoke = transform.GetChild(0).GetComponent<ParticleSystem>();
    }
    virtual public void Start()
    {
        
    }
    virtual protected void OnEnable()
    {
        timer = lifeTime;
        transform.GetComponent<SpriteRenderer>().color = Vector4.one;
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        isExplosion = false;
    }

    // Update is called once per frame
    virtual public void FixedUpdate()
    {
        if (rig)
        {
            rig.velocity = transform.up * speed;
        }
        if (!rig)
        {
            rig = transform.GetComponent<Rigidbody2D>();
        }
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            // ObjectPool.Instance.PushObject(areaObj);
            // areaObj = null;
            // ObjectPool.Instance.PushObject(gameObject);
            ShootGround(explosionPrefab , "Player");
        }
    }
    
    virtual public void ShootGround(GameObject explosionFirePre , string layerMaskName)
    {
        if (!isExplosion)
        {
            isExplosion = true;
            //命中地面时生成一个范围给所有单位造成伤害
            rig.velocity = Vector2.zero;
            Collider2D player =  Physics2D.OverlapCircle(transform.position , 2 , LayerMask.GetMask(layerMaskName));
            if (player)
            {
                launcher.HitPlayer(player , player.transform.position.x - transform.position.x);
            }
            if (areaObj)
            {
                ObjectPool.Instance.PushObject(areaObj);
                areaObj = null;
            }
            // ObjectPool.Instance.PushObject(areaObj);
            // areaObj = null;
            launcher.ExplosionSound();
            GameObject exp = ObjectPool.Instance.GetObject(explosionPrefab);
            exp.transform.position = explosionPoint.position;

            // GameObject explosionFire = ObjectPool.Instance.GetObject(explosionFirePre);
            // explosionFire.transform.position = transform.position;

            // GameObject particle = ObjectPool.Instance.GetObject(particlePrefab);
            // particle.GetComponent<ParticleEffect>().SetData(transform.position + Vector3.down * 0.5f * transform.localScale.x);

            // CameraBehaviour.Instance.CameraShake(0.3f , 0.35f);

            // ObjectPool.Instance.PushObject(gameObject);
            StartCoroutine(DestoryGameObject(0.5f));
        }
    }
    protected IEnumerator DestoryGameObject(float time)
    {
        transform.GetComponent<SpriteRenderer>().color = Vector4.zero;
        transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(time);
        ObjectPool.Instance.PushObject(gameObject);
    }
    virtual public void SetData(MissileLauncher launcher , float speed , Vector3 dir)
    {
        this.launcher = launcher;
        this.speed = speed;
        transform.up = dir;
        if (!rig)
        {
            rig = transform.GetComponent<Rigidbody2D>();
        }
        timer = lifeTime;
    }
    virtual public void SetData(MissileLauncher launcher , float speed , Vector3 dir , GameObject areaObj)
    {
        this.launcher = launcher;
        this.speed = speed;
        transform.up = dir;
        if (!rig)
        {
            rig = transform.GetComponent<Rigidbody2D>();
        }
        timer = lifeTime;
        this.areaObj = areaObj;
    }
    virtual public void SetData(MissileLauncher launcher , float speed , Vector3 dir , GameObject areaObj , Vector3 targetpoint)
    {
        this.launcher = launcher;
        this.speed = speed;
        transform.up = dir;
        if (!rig)
        {
            rig = transform.GetComponent<Rigidbody2D>();
        }
        timer = lifeTime;
        this.areaObj = areaObj;
    }
    
    
    virtual public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Ground") || other.CompareTag("Player"))
        {
            ShootGround(explosionPrefab , "Player");
        }
    }
}

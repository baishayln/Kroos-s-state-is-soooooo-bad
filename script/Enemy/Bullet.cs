using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 dirction;
    private float damage;
    private float lifeTimer = 0;
    private Rigidbody2D rig;
    private GameObject target;
    private Color startColor;
    private Vector3 selfScale;
    private Vector3 originScale;
    private Vector4 color;
    private SpriteRenderer sprtRenderer;
    private bool isHiting = false;
    private Transform shooter;
    private float originSpeedDistance;
    private Vector2 speedOfFar;
    [SerializeField]public float bulletAcceleration;
    [SerializeField]public float minxSpeedX;
    [SerializeField]private Vector3 scale = new Vector3(0.6f , 0.6f , 1);
    [SerializeField]private GameObject tarilPrefab;
    private GameObject taril;
    private GameObject cameraPoint;
    [SerializeField]public AudioClip[] hitEffect;
    [SerializeField]private string attackTag = "PlayerBody";
    [SerializeField]private bool isGrivaty = false;
    [SerializeField]private bool isFollowTarget = false;
    [SerializeField]private float pushTime = 0.2f;

    //      如果碰撞到地面会存在一段时间然后逐渐消失
    void Awake()
    {
        rig = gameObject.GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        cameraPoint = GameObject.Find("Main Camera");
    }
    void OnEnable()
    {
        sprtRenderer = transform.GetComponent<SpriteRenderer>();
        if(isFollowTarget && startColor == new Color(0 , 0 , 0 , 0))
        {
            startColor = sprtRenderer.color;
            sprtRenderer.color = startColor;
            color = startColor;
        }
        rig = gameObject.GetComponent<Rigidbody2D>();
        target = null;
        isHiting = false;
        if (isGrivaty)
        {
            rig.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            rig.bodyType = RigidbodyType2D.Kinematic;
        }
        lifeTimer = 0;          //子弹每次被使用时的生命计时
        speedOfFar = Vector2.zero;
        transform.localScale = scale;
        originScale = transform.localScale;
    }
    void FixedUpdate()
    {
        if(isHiting && isFollowTarget)
        {
            if(color.w > 0)
            {
                color.w = color.w - 0.03f;
                sprtRenderer.color = color;
            }
            else
            {
                transform.localScale = originScale;
                ObjectPool.Instance.PushObject(gameObject);
            }
        }
        lifeTimer += Time.deltaTime;
        // if(lifeTimer > 15)
        // {
        //     StartCoroutine(PushSelf(pushTime));
        // }
        if (Vector3.Distance(transform.position , cameraPoint.transform.position) > 50)
        {
            if (tarilPrefab)
            {
                PushRrail();
            }
            StartCoroutine(PushSelf(pushTime));
        }
        
        if(!isHiting && isGrivaty)
            RotateSelf();
    }

    virtual protected void RotateSelf()
    {
        dirction.x = rig.velocity.x;
        dirction.y = rig.velocity.y;
        transform.right = dirction;
    }
    virtual public void SetBullet(float dmg , float speed , Vector3 dirction , Vector3 position)
    {
        damage = dmg;
        transform.right = dirction;
        rig.velocity = dirction * speed;
        speedOfFar.x = rig.velocity.x;
        if (tarilPrefab)
        {
            SetRrail();
        }
        transform.position = position;
        transform.GetChild(0).GetComponent<TrailRenderer>().Clear();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        OnTrigger(other);
    }
    virtual protected void OnTrigger(Collider2D other)
    {
        if(other.CompareTag(attackTag) && !isHiting)
        {
            if (tarilPrefab)
            {
                PushRrail();
            }
            Hit(other.transform);
        }
        if(other.CompareTag("Ground") && !isHiting)
        {
            if (tarilPrefab)
            {
                PushRrail();
            }
            Hit();
        }
    }
    virtual protected void PlayAudio()
    {
        if (hitEffect.Length > 0)
        {
            SoundManager.Instance.PlayVoiceSound(hitEffect[Random.Range(0 , hitEffect.Length)]);
        }
    }
    virtual protected void Hit(Transform hitTarget)
    {
        if (isFollowTarget)
        {
            SetTargetToParend(hitTarget);
        }
        
        isHiting = true;
        rig.velocity = Vector2.zero;
        rig.bodyType = RigidbodyType2D.Kinematic;
        hitTarget.GetComponentInParent<OnHit>().OnHit(damage , transform.right.x);

        PlayAudio();
        StartCoroutine(PushSelf(pushTime));
    }
    virtual protected void SetTargetToParend(Transform hitTarget)
    {
        //此函数是当命中物体后需要产生将自身设为子物体跟随其移动和旋转的效果时调用的
        target = hitTarget.gameObject;
        transform.SetParent(hitTarget);

        selfScale.x = originScale.x/hitTarget.transform.localScale.x;       //这三行在击中缩放不为111的物体、并将自身置为其子物体后会根据自身缩放和命中物体缩放进行计算，保证自身外观不会变化
        selfScale.y = originScale.y/hitTarget.transform.localScale.y;
        selfScale.z = originScale.z/hitTarget.transform.localScale.z;
        
        if(selfScale.z !> 0)
        {
            selfScale.z = 1;
        }

        transform.localScale = selfScale;
    }
    virtual protected void Hit()
    {
        isHiting = true;
        rig.velocity = Vector2.zero;
        rig.bodyType = RigidbodyType2D.Kinematic;
        PlayAudio();
        StartCoroutine(PushSelf(pushTime));
    }
    
    virtual public void SetRrail()
    {
        taril = ObjectPool.Instance.GetObject(tarilPrefab);
        taril.transform.SetParent(transform);
        taril.transform.position = transform.position - transform.right * 0.5f * 0.3f;
        taril.GetComponent<PlayerBulletTrail>().trailRenderer.enabled = true;
    }
    virtual public void PushRrail()
    {
        taril.GetComponent<PlayerBulletTrail>().StopTrail(0.2f);
        taril = null;
    }
    
    protected IEnumerator PushSelf(float time)
    {
        yield return new WaitForSeconds(time);
        ObjectPool.Instance.PushObject(gameObject);
    }
}

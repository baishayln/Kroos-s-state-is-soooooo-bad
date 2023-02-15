using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Vector2 dirction;
    private float decelerationThreshold;
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
    private Vector3 rotateOfHiting;
    private Transform shooter;
    private float originSpeedDistance;
    private Vector2 speedOfFar;
    [SerializeField]public float bulletAcceleration;
    [SerializeField]public float minxSpeedX;
    [SerializeField]private Vector3 scale = new Vector3(0.6f , 0.6f , 1);
    [SerializeField]private GameObject tarilPrefab;
    private GameObject taril;
    private GameObject cameraPoint;
    [SerializeField]public AudioClip hitEffect1;
    [SerializeField]public AudioClip hitEffect2;
    [SerializeField]public AudioClip hitEffect3;
    [SerializeField]public AudioClip hitEffect4;

    //      如果碰撞到地面会存在一段时间然后逐渐消失
    void Start()
    {
        cameraPoint = GameObject.Find("Main Camera");
    }
    void OnEnable()
    {
        sprtRenderer = transform.GetComponent<SpriteRenderer>();
        if(startColor == new Color(0 , 0 , 0 , 0))
            startColor = sprtRenderer.color;
        rig = gameObject.GetComponent<Rigidbody2D>();
        target = null;
        isHiting = false;
        rig.bodyType = RigidbodyType2D.Dynamic;
        sprtRenderer.color = startColor;
        color = startColor;
        lifeTimer = 0;          //子弹每次被使用时的生命计时
        speedOfFar = Vector2.zero;
        transform.localScale = scale;
        originScale = transform.localScale;
    }
    void FixedUpdate()
    {
        if(isHiting)
        {
            if(color.w > 0)
            {
                color.w = color.w - 0.03f;
                sprtRenderer.color = color;
            }
            else
            {
                transform.localScale = originScale;
                // Destroy(gameObject);
                ObjectPool.Instance.PushObject(gameObject);
            }
            // FollowTarget();
        }
        XSpeedDown();
        lifeTimer += Time.deltaTime;
        if(lifeTimer > 15)
        {
            Destroy(gameObject);            //当此次被使用时在游戏中的存在时间大于15秒则被视为射击出界自行销毁或归于对象池
        }
    }

    void Update()
    {
        // if(isHiting)
            // FollowTarget();
        if(!isHiting)
            RotateSelf();

        if (Vector3.Distance(transform.position , cameraPoint.transform.position) > 50)
        {
            PushRrail();
            ObjectPool.Instance.PushObject(gameObject);
        }
        // if(startxHisTr)   
        // {
        //     if((xHistroy > 0 && rig.velocity.x < 0) || (xHistroy < 0 && rig.velocity.x > 0))
        //     {
        //         Debug.Log("反向了");
        //     }
        //     if(rig.velocity.x > 0)
        // {xHistroy = 1;}
        // else
        // {
        //     xHistroy = -1;
        // }
        // startxHisTr = true;}
        
        // XSpeedDown();   //按照时间开始减速还是按照已经飞行的距离？或者当前距离玩家的位置？（重力会影响第三种）
    }
    private void RotateSelf()
    {
        dirction.x = rig.velocity.x;
        dirction.y = rig.velocity.y;
        transform.right = dirction;
    }
    private void XSpeedDown()
    {
        // if(Mathf.Abs((transform.position.x - shooter.position.x) * (transform.position.x - shooter.position.x) + (transform.position.y - shooter.position.y) * (transform.position.y - shooter.position.y)) > originSpeedDistance)
        if(Vector2.Distance(shooter.position , transform.position) > originSpeedDistance && !isHiting)
        {
            if(rig.velocity.x > 0)
            {
                if(rig.velocity.x - minxSpeedX > 0 && speedOfFar.y <= speedOfFar.x)
                    speedOfFar.x = rig.velocity.x - bulletAcceleration;
                else
                {
                    speedOfFar.x = rig.velocity.x;
                }
                speedOfFar.y = rig.velocity.y - bulletAcceleration;
            }
            else
            {
                if(rig.velocity.x + minxSpeedX < 0 && speedOfFar.y <= -speedOfFar.x)
                    speedOfFar.x = rig.velocity.x + bulletAcceleration;
                else
                {
                    speedOfFar.x = rig.velocity.x;
                }
                speedOfFar.y = rig.velocity.y - bulletAcceleration;
            }
            rig.velocity = speedOfFar;
        }
    }
    // private void XSpeedDown()
    // {
    //     // if(Mathf.Abs((transform.position.x - shooter.position.x) * (transform.position.x - shooter.position.x) + (transform.position.y - shooter.position.y) * (transform.position.y - shooter.position.y)) > originSpeedDistance)
    //     if(Vector2.Distance(shooter.position , transform.position) > originSpeedDistance && rig.velocity.x > 0)
    //     {
    //         speedOfFar.x = rig.velocity.x - 3f;
    //         speedOfFar.y = rig.velocity.y;
    //         Debug.Log(speedOfFar);
    //         rig.velocity = speedOfFar;
    //     }
    // }
    // private void XSpeedDown()
    // {
    //     // if(Mathf.Abs((transform.position.x - shooter.position.x) * (transform.position.x - shooter.position.x) + (transform.position.y - shooter.position.y) * (transform.position.y - shooter.position.y)) > originSpeedDistance)
    //     if(Vector2.Distance(shooter.position , transform.position) > originSpeedDistance && rig.velocity.x - 1.5f > 0)
    //     {
    //         speedOfFar.x = rig.velocity.x - 1.5f;
    //         if(rig.velocity.y - 1.5f > 0)
    //             speedOfFar.y = rig.velocity.y - 1.5f;
    //         Debug.Log(speedOfFar);
    //         rig.velocity = speedOfFar;
    //     }
    // }
    //      也许可以模仿某种模仿自然下雪的制雪机（某些剧情和游戏情节可用）
    public void SetBullet(float dmg , float speed , Vector3 dirction , Transform shooter , float originSpeedDistance , float bulletAcceleration , float minxSpeedX)
    {
        damage = dmg;
        transform.right = dirction;
        rig.velocity = dirction * speed;
        speedOfFar.x = rig.velocity.x;
        this.shooter = shooter;
        this.originSpeedDistance = originSpeedDistance;
        this.bulletAcceleration = bulletAcceleration;
        this.minxSpeedX = minxSpeedX;
        
        SetRrail();
        // SetRrailTrue();
        // if(rig.velocity.x > 0)
        // {xHistroy = 1;}
        // else
        // {
        //     xHistroy = -1;
        // }
        // startxHisTr = true;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && !isHiting)
        {
            // SetRrailFalse();
            PushRrail();
            Hit(other.transform);
        }
        if(other.CompareTag("Ground") && !isHiting)
        {
            // SetRrailFalse();
            PushRrail();
            Hit();
        }
        // startxHisTr = false;
    }
    private void PlayAudio()
    {
        switch(Random.Range(0 , 4))
        {
            case 0:
                SoundManager.Instance.PlayEffectSound(hitEffect1);
                break;
            case 1:
                SoundManager.Instance.PlayEffectSound(hitEffect2);
                break;
            case 2:
                SoundManager.Instance.PlayEffectSound(hitEffect3);
                break;
            case 3:
                SoundManager.Instance.PlayEffectSound(hitEffect4);
                break;
            default:
                SoundManager.Instance.PlayEffectSound(hitEffect4);
                break;
        }
    }
    private void Hit(Transform hitTarget)
    {
        // rotateOfHiting = transform.rotation.eulerAngles;
        // rotateOfHiting.x = transform.rotation.x;
        // rotateOfHiting.y = transform.rotation.y;
        // rotateOfHiting.z = transform.rotation.z;

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
        
        isHiting = true;
        rig.velocity = Vector2.zero;
        rig.bodyType = RigidbodyType2D.Kinematic;
        if(hitTarget.CompareTag("Enemy"))
            hitTarget.GetComponent<EnemyBehavior>().OnHit(damage , transform.right.x);

        PlayAudio();
    }
    private void Hit()
    {
        isHiting = true;
        rig.velocity = Vector2.zero;
        rig.bodyType = RigidbodyType2D.Kinematic;
        PlayAudio();
    }
    
    public void SetRrail()
    {
        taril = ObjectPool.Instance.GetObject(tarilPrefab);
        taril.transform.SetParent(transform);
        taril.transform.position = transform.position - transform.right * 0.5f * 0.3f;
        taril.GetComponent<PlayerBulletTrail>().trailRenderer.enabled = true;
    }
    public void PushRrail()
    {
        taril.GetComponent<PlayerBulletTrail>().StopTrail(0.2f);
        taril = null;
    }
    // public void SetRrailTrue()
    // {
    //     transform.GetChild(0).gameObject.SetActive(true);
    // }
    // public void SetRrailFalse()
    // {
    //     transform.GetChild(0).gameObject.SetActive(false);
    // }
    // private void FollowTarget()
    // {
    //     transform.rotation = target.transform.rotation;
    //     transform.Rotate(rotateOfHiting , 1);
    // }
}

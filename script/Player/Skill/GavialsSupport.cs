using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GavialsSupport : MonoBehaviour
{
    private bool isTreat;
    private bool isTreatPlayer;
    private bool isAttack;
    private Rigidbody2D rig;
    [SerializeField]public float flySpeed = 18;
    [SerializeField]public float supportSpeed = 30;
    [SerializeField]public Vector3 wRotateSpeed = new Vector3(0 , 0 , 360);
    private GameObject player;
    private Vector3 goBackDir;
    [SerializeField]public float destroyDsts = 1;
    [SerializeField]public float restoreHP = 5;
    private bool isHitingGround;
    private SpriteRenderer sprtRenderer;
    private Color startColor;
    private Vector4 color;
    private float lifeTimer = 0;
    [SerializeField]private float damage = 150;
    [SerializeField]public GameObject bangPrefab;
    private Color bangColor;
    [SerializeField]private GameObject particlePrefab;
    [SerializeField]private AudioClip BO;
    // Start is called before the first frame update
    void Start()
    {
        rig = transform.GetComponent<Rigidbody2D>();
    }
    void OnEnable()
    {
        isTreat = false;
        isTreatPlayer = false;
        isAttack = false;
        sprtRenderer = transform.GetComponent<SpriteRenderer>();
        if(startColor == new Color(0 , 0 , 0 , 0))
            startColor = sprtRenderer.color;
        rig = gameObject.GetComponent<Rigidbody2D>();
        isHitingGround = false;
        rig.bodyType = RigidbodyType2D.Dynamic;
        sprtRenderer.color = startColor;
        color = startColor;
        transform.localScale = Vector3.one;
        lifeTimer = 0;          //子弹每次被使用时的生命计时
    }
    // Update is called once per frame
    void Update()
    {
        if(isHitingGround)
        {
            if(color.w > 0)
            {
                color.w = color.w - 0.03f;
                sprtRenderer.color = color;
            }
            else
            {
                // Destroy(gameObject);
                ObjectPool.Instance.PushObject(gameObject);
            }
            // FollowTarget();
        }

        lifeTimer += Time.deltaTime;
        if(lifeTimer > 5)
        {
            ObjectPool.Instance.PushObject(gameObject);          //当此次被使用时在游戏中的存在时间大于15秒则被视为射击出界自行销毁或归于对象池
        }

        if(isAttack)
        {
            transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * 1.5f, transform.localScale.y + Time.deltaTime * 1.5f, transform.localScale.z + Time.deltaTime * 0.3f);
            transform.Rotate(wRotateSpeed * Time.deltaTime);
            //每一帧逐渐变大一点点，并且增加亿点点碰撞体积和命中时的溅射范围
        }

        if(isTreat)
        {
            if(isTreatPlayer)
            {
                transform.position = Vector3.MoveTowards(transform.position , player.transform.position + goBackDir * 1.5f , supportSpeed * Time.deltaTime);
                transform.Rotate(wRotateSpeed * Time.deltaTime);
                if(Vector3.Distance(player.transform.position + goBackDir * 1.5f , transform.position) < destroyDsts)
                {
                    ObjectPool.Instance.PushObject(gameObject);
                }
                //奶到玩家后远离
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position , player.transform.position + (Vector3.up * 0.5f) , supportSpeed * Time.deltaTime);
                transform.Rotate(wRotateSpeed * Time.deltaTime);
                //没有奶到玩家时直着朝玩家飞行
            }
        }
    }
    public void SetData(Vector3 ROfenemyBorn , Vector3 cameraPosition , bool isFullHP , Vector2 mousePosition , GameObject player)
    {
        transform.position = new Vector3(cameraPosition.x + Random.Range(-ROfenemyBorn.x * 0.5f , ROfenemyBorn.x * 0.5f) , cameraPosition.y + ROfenemyBorn.y , 0);
        //玩家不满血时直接进行治疗，是使用动画机动画进行治疗还是程序行为动画？接近头部时产生一个梆字然后淡出，换一下字体和边框，略大，会产生治疗的绿色十字架特效
        if(!isFullHP)
        {
            isTreat = true;
            treatPlayer();
        }
        //玩家满血时，发射一条射线向下检测地面，如果未检测到就根据玩家所在的Y轴和鼠标的X轴拼合为坐标？
        else
        {
            isAttack = true;
            Attack(mousePosition);
            // Physics2D.
        }
        this.player = player;
        goBackDir = (player.transform.position + (Vector3.up * 0.5f)) - transform.position;
        goBackDir.y = -goBackDir.y;
        if (mousePosition.x > transform.position.x)
        {
            wRotateSpeed.z = Mathf.Abs(wRotateSpeed.z) * -1;
        }
        else
        {
            wRotateSpeed.z = Mathf.Abs(wRotateSpeed.z);
        }
    }
    private void Attack(Vector2 mousePosition)
    {
        rig = transform.GetComponent<Rigidbody2D>();
        rig.velocity = new Vector2(mousePosition.x - transform.position.x , mousePosition.y - transform.position.y).normalized * flySpeed;
    }
    private void ShootGround()
    {
        //命中地面时生成一个范围给所有单位造成伤害
        isHitingGround = true;
        rig.velocity = Vector2.zero;
        rig.bodyType = RigidbodyType2D.Kinematic;
        isTreat = false;
        isAttack = false;
        Collider2D[] enemyWhoWasShot =  Physics2D.OverlapBoxAll(transform.position , transform.localScale * 7 , 0 , LayerMask.GetMask("Enemy"));
        for(int i = enemyWhoWasShot.Length ; i > 0 ; i --)
        {
            float Xspeed = Random.Range(0f , 40f);
            enemyWhoWasShot[i - 1].GetComponent<EnemyBehavior>().OnHit(damage , new Vector2(Xspeed , 40f - Xspeed));
            // if(enemyWhoWasShot[i - 1].GetComponent<EnemyBehavior>().OnHit(damage))
            // {
            //     float Xspeed = Random.Range(0f , 40f);
            //     enemyWhoWasShot[i - 1].GetComponent<EnemyBehavior>().DeadFly(new Vector2(Xspeed , 40f - Xspeed));
            // }
        }
        GameObject particle = ObjectPool.Instance.GetObject(particlePrefab);
        particle.GetComponent<ParticleEffect>().SetData(transform.position + Vector3.down * 0.5f * transform.localScale.x);

        CameraBehaviour.Instance.CameraShake(0.3f , 0.35f);
    }
    private void PlayAudio()
    {
        SoundManager.Instance.PlayEffectSound(BO);
    }
    private void treatPlayer()
    {
        
    }
    private void RestoreHP()
    {
        //会生成一个梆字
    }
    private void Bang()
    {
        GameObject bang = ObjectPool.Instance.GetObject(bangPrefab);
        RandomColor();
        bang.GetComponent<Bang>().SetData(player.transform.position + Vector3.up * Random.Range(0f , 2f) + Vector3.right * Random.Range(-2f , 2f) , bangColor , Random.Range(-30f , 30f));
    }
    private void RandomColor()
    {
        bangColor.r = Random.Range(0f , 1f);
        bangColor.g = Random.Range(0f , 1f);
        bangColor.b = Random.Range(0f , 1f);
        bangColor.a = 1;
        switch(Random.Range(0 , 3))
        {
            case 1:
                bangColor.r = 1;
                break;
            case 2:
                bangColor.g = 1;
                break;
            case 3:
                bangColor.b = 1;
                break;
            default:
                bangColor.g = 1;
                break;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(isAttack && other.CompareTag("Ground"))
        {
            ShootGround();
        }
        if(isAttack && other.CompareTag("Enemy"))
        {
            Vector2 dir = (other.transform.position - transform.position).normalized;
            other.GetComponent<EnemyBehavior>().OnHit(damage , dir * flySpeed * 1.3f);
        }
        if(isTreat && other.CompareTag("Player"))
        {
            // treatPlayer();
            Bang();
            PlayAudio();
            isTreatPlayer = true;
            player.GetComponent<PlayerMove>().RestoreHP(restoreHP);
        }
    }
    // void OnTriggerEnter2D(Collision2D other)
    // {
    //     if(isAttack && other.collider.CompareTag("Ground"))
    //     {
    //         ShootGround();
    //     }
    //     if(isAttack && other.collider.CompareTag("Enemy"))
    //     {
    //         Vector2 dir = (other.transform.position - transform.position).normalized;
    //         other.collider.GetComponent<EnemyBehavior>().OnHit(damage , dir * flySpeed * 1.3f);
    //     }
    //     if(isTreat && other.collider.CompareTag("Player"))
    //     {
    //         // treatPlayer();
    //         Bang();
    //         isTreatPlayer = true;
    //         player.GetComponent<PlayerMove>().RestoreHP(restoreHP);
    //     }
    // }
}

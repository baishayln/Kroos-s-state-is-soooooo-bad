using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour , OnHit
{
    [SerializeField]public float healthUpLimit = 30;
    protected float nowHealthUpLimit = 30;
    [SerializeField]protected float healthIncrease = 5;
    [SerializeField]public float health;
    [SerializeField]public float shootInterval = 3;
    protected float shootTimer;
    [SerializeField]public float minInclusive = 1;
    [SerializeField]public float maxInclusive = 3;
    [SerializeField]public float speed = 3;
    protected Rigidbody2D rig;
    protected GameObject player;
    public GameObject playerGeneratePrefab;
    [SerializeField]public float healthConstant;
    public float rewardProbabilty = 20;
    [SerializeField]public float bornEnhancementProbability = 20;
    [SerializeField]public GameObject Enhancement1;
    [SerializeField]public GameObject Enhancement2;
    [SerializeField]public GameObject Enhancement3;
    [SerializeField]public GameObject Enhancement4;
    [SerializeField]public GameObject Enhancement5;
    [SerializeField]public GameObject Enhancement6;
    [SerializeField]public GameObject Enhancement7;
    protected GameObject fightUI;
    protected GameObject target;
    protected float hitDir;
    [SerializeField]protected bool isDead = false;
    protected float deadTimer = 0;
    protected bool isShoot = false;
    [SerializeField]public Vector3 wRotateSpeed = new Vector3(0 , 0 , 720);
    protected Transform cameraPoint;
    protected Collider2D[] objs;
    protected Transform nearestEnemy;
    protected float nearsetEnemyDsts;
    protected bool isFight;
    protected bool isEscape;
    protected float escapeTimer = 0;
    private float hitedMove = 1.5f;
    protected Vector2 moveSpeed = new Vector2(0 , 0);
    protected Vector3 leftScale = new Vector3(-1 , 1 , 1);
    //      添加一个全局变量控制怪物波数
    // Start is called before the first frame update
    virtual protected void Start()
    {
        fightUI = GameObject.Find("FightUI");
        // rig = gameObject.GetComponent<Rigidbody2D>();
    }
    void OnEnable()
    {
        player = GameObject.Find("Player");
        setHealth();
        isDead = false;
        deadTimer = 0;
        transform.localScale = Vector3.one;
        transform.GetComponent<BoxCollider2D>().enabled = true;
        transform.rotation = Quaternion.identity;
        health = healthUpLimit;
        shootTimer = shootInterval;
    }

    // Update is called once per frame
    void Update()
    {
        IsTooLongOfCamera();
        
        if(!isDead && !isShoot)
        {
            Move();
        }
        else if(isDead)
        {
            transform.Rotate(wRotateSpeed * Time.deltaTime);
            deadTimer += Time.deltaTime;
            if(deadTimer >= 2f)
            {
                SetChildToPool();
                ObjectPool.Instance.PushObject(gameObject);
            }
        }
        if(shootTimer < 0 && !isShoot)
        {
            ShootStart();
            SetTimer();
        }
        else if(isShoot)
        {
            Shoot();
        }
        else
        {
            if (!isDead)
            {
                shootTimer -= Time.deltaTime;
            }
        }
        if(isDead && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime * 2 , transform.localScale.y - Time.deltaTime * 2 , transform.localScale.z - Time.deltaTime * 2);
        }
        Direction();
    }
    virtual protected void Direction()
    {
        if(rig.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
        }
        else if(rig.velocity.x < 0)
        {
            transform.localScale = leftScale;
        }
    }
    virtual protected void Move()
    {
        if(player.transform.position.x > transform.position.x)
        {
            rig.velocity = Vector2.right * speed;
        }
        else if(player.transform.position.x < transform.position.x)
        {
            rig.velocity = Vector2.left * speed;
        }
        else
        {
            rig.velocity = Vector2.zero * speed;
        }
    }
    virtual protected void Shoot()
    {

    }
    virtual protected void ShootStart()
    {

    }
    virtual protected void ShootEnd()
    {

    }

    virtual protected void SetTimer()
    {
        // timer = shootInterval + Random.Range(minInclusive , maxInclusive);
    }

    protected void Die()
    {
        EnemyCountDown();
        //      命中时向后击退的幅度变大有可能会飞起一定高度，在一段时间后销毁（闪烁消失？）
        if(Random.Range(0 , 100) < bornEnhancementProbability)
        {
            if (Random.Range(0 , 100) < 90)
            {
                switch (Random.Range(0 , 4))
                {
                    case 0:
                        GenerateRewards(Enhancement1);
                        break;
                    case 1:
                        GenerateRewards(Enhancement2);
                        break;
                    case 2:
                        GenerateRewards(Enhancement3);
                        break;
                    case 3:
                        GenerateRewards(Enhancement4);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (Random.Range(4 , 7))
                {
                    case 4:
                        GenerateRewards(Enhancement5);
                        break;
                    case 5:
                        GenerateRewards(Enhancement6);
                        break;
                    case 6:
                        GenerateRewards(Enhancement7);
                        break;
                    default:
                        break;
                }
            }
        }
        isDead = true;
    }
    private void GenerateRewards(GameObject Enhancement)        //掉落物体并销毁自身
    {
        GameObject playerGenerate = GameObject.Instantiate(Enhancement , transform.position , Quaternion.identity);
        playerGenerate.GetComponent<PlayerEnhancement>().StartMove();
    }
    protected void SetChildToPool()
    {
        for(int i = transform.childCount ; i > 0 ; i--)
        {
            if(transform.GetChild(i - 1).name.IndexOf("Bullet") >= 0)
            {
                ObjectPool.Instance.PushObject(transform.GetChild(i - 1).gameObject);
            }
        }
    }
    virtual public bool OnHit(float damage , float Dir)         //当单位受伤死亡后，返回true
    {
        if(Dir >= 0)
        {
            hitDir = 1;
        }
        else
        {
            hitDir = -1;
        }
        
        moveSpeed.x += hitDir * hitedMove;
        if (!rig)
        {
            rig = transform.GetComponent<Rigidbody2D>();
        }
        rig.velocity = moveSpeed;
        isFight = true;
        isEscape = false;
            //  命中时会向后击退，角度有一定的变化
            //  或者就是短暂后退
        health -= damage;
        DamageCount(damage);
        if(health <= 0)
        {
            Die();
            DeadFly(hitDir);
        }
        escapeTimer = 0;
        return isDead;
    }
    virtual public bool OnHit(float damage , Vector2 speed)         //当单位受伤死亡后，返回true
    {
        if(speed.x >= 0)
        {
            hitDir = 1;
        }
        else
        {
            hitDir = -1;
        }
        
        moveSpeed.x += hitDir * hitedMove;
        rig.velocity = moveSpeed;
        isFight = true;
        isEscape = false;
            //  命中时会向后击退，角度有一定的变化
        health -= damage;
        DamageCount(damage);
        if(health <= 0)
        {
            Die();
            DeadFly(speed);
        }
        escapeTimer = 0;
        return isDead;
    }
    virtual public bool OnHit(float damage)         //当单位受伤死亡后，返回true
    {
        health -= damage;
        DamageCount(damage);
        if(health <= 0)
        {
            Die();
            // DeadFly(speed);
        }
        return isDead;
    }
    public float GetHealthUplimit()
    {
        return healthUpLimit;
    }
    virtual public void DamageCount(float damage)
    {
        GlobleDamageCounter.Instance.CauseDamage(damage);
    }
    protected void DeadFly(float hitDir)
    {
        transform.GetComponent<BoxCollider2D>().enabled = false;
        rig = gameObject.GetComponent<Rigidbody2D>();
        rig.velocity = new Vector2(hitDir * 35 , Random.Range(3f , 28f));
    }
    protected void DeadFly(Vector2 speed)
    {
        transform.GetComponent<BoxCollider2D>().enabled = false;
        rig.velocity = speed;
    }
    protected void EnemyCountDown()
    {
        if (!fightUI)
        {
            fightUI = GameObject.Find("FightUI");
        }
        // fightUI.GetComponent<EnemyBornController>().EnemyDeath();
        fightUI.GetComponent<FightUIController>().EnemyDeath();
    }
    protected void setHealth()
    {
        // health = health + 波数 * 一个波数常量;
    }
    virtual public bool SetBornPosition(Vector3 ROfenemyBorn)
    {
        //地面敌人需要检测地面位置
        return true;
    }
    virtual public void IsTooLongOfCamera()
    {
        //地面敌人需要检测地面位置
    }
    virtual public float GetHealth()
    {
        return health;
    }
    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("EnemyExistenceRange"))
        {
            cameraPoint = other.transform;
        }
    }
    virtual protected void OnTriggerStay2D(Collider2D other)
    {
        
    }
    
    public bool IsDead()
    {
        return isDead;
    }
    virtual public Transform GetNearestEnemy(float r)         //当单位受伤死亡后，返回true
    {
        objs = Physics2D.OverlapCircleAll(transform.position , r , LayerMask.GetMask("Enemy"));
        if (objs.Length == 0)
        {
            return null;
        }
        if (objs[0].transform == transform)
        {
            if (objs.Length == 1)
            {
                return null;
            }
            else
            {
                nearsetEnemyDsts = Vector2.Distance(transform.position , objs[1].transform.position);
                nearestEnemy = objs[1].transform;
            }
        }
        else
        {
            nearsetEnemyDsts = Vector2.Distance(transform.position , objs[0].transform.position);
            nearestEnemy = objs[0].transform;
        }
        for(int i = 1 ; i < objs.Length ; i ++)
        {
            if (Vector2.Distance(transform.position , objs[i].transform.position) < nearsetEnemyDsts)
            {
                if (objs[i].transform != transform && objs[i].GetComponent<OnHit>().GetHealth() > 0)
                {
                    nearsetEnemyDsts = Vector2.Distance(transform.position , objs[i].transform.position);
                    nearestEnemy = objs[i].transform;
                }
            }
        }
        return nearestEnemy;
    }

    //还需要一个激活周围所有敌人的函数，自己需要一个警戒bool值，每个单位只进入一次警戒，只发送一次警戒消息
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]private float health = 10;
    [SerializeField]public float shootInterval = 3;
    private float timer;
    [SerializeField]public float minInclusive = 2;
    [SerializeField]public float maxInclusive = 5;
    [SerializeField]public float speed = 3;
    private Rigidbody2D rig;
    private GameObject player;
    public GameObject playerGeneratePrefab;
    [SerializeField]public float healthConstant;
    public float rewardProbabilty = 20;
    [SerializeField]public float DropProbability = 15;
    [SerializeField]public GameObject Enhancement1;
    [SerializeField]public GameObject Enhancement2;
    [SerializeField]public GameObject Enhancement3;
    [SerializeField]public GameObject Enhancement4;
    [SerializeField]public GameObject Enhancement5;
    [SerializeField]public GameObject Enhancement6;
    [SerializeField]public GameObject Enhancement7;
    private GameObject fightUI;
    private GameObject target;
    private float hitDir;
    private bool isDead = false;
    private float deadTimer = 0;
    [SerializeField]public Vector3 wRotateSpeed = new Vector3(0 , 0 , 720);
    //      添加一个全局变量控制怪物波数
    // Start is called before the first frame update
    void Start()
    {
        rig = gameObject.GetComponent<Rigidbody2D>();
    }
    void OnEnable()
    {
        player = GameObject.Find("Player");
        setHealth();
        isDead = false;
        deadTimer = 0;
        transform.GetComponent<BoxCollider2D>().enabled = true;
        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead)
        {
            Move();
        }
        else
        {
            transform.Rotate(wRotateSpeed * Time.deltaTime);
            deadTimer += Time.deltaTime;
            if(deadTimer >= 2f)
            {
                SetChildToPool();
                ObjectPool.Instance.PushObject(gameObject);
            }
        }
        if(timer < 0)
        {
            Shoot();
            SetTimer();
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
    private void Shoot()
    {

    }
    private void SetTimer()
    {
        timer = shootInterval + Random.Range(minInclusive , maxInclusive);
    }

    private void Die()
    {
        //      命中时向后击退的幅度变大有可能会飞起一定高度，在一段时间后销毁（闪烁消失？）
        if(Random.Range(0 , 100) < DropProbability)
        {
            switch (Random.Range(0 , 7))
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
        // Destroy(gameObject);
        SetChildToPool();
        isDead = true;
    }
    private void GenerateRewards(GameObject Enhancement)        //掉落物体并销毁自身
    {
        GameObject playerGenerate = GameObject.Instantiate(Enhancement , transform.position , Quaternion.identity);
        playerGenerate.GetComponent<PlayerEnhancement>().StartMove();
        // playerGenerate.transform.SetParent(null);
        // Destroy(gameObject);

        // SetChildToPool();
        // ObjectPool.Instance.PushObject(gameObject);
    }
    private void SetChildToPool()
    {
        for(int i = transform.childCount ; i > 0 ; i--)
        {
            if(transform.GetChild(i - 1).name.IndexOf("Bullet") >= 0)
            {
                ObjectPool.Instance.PushObject(transform.GetChild(i - 1).gameObject);
            }
        }
    }
    public bool OnHit(float damage , float Dir)         //当单位受伤死亡后，返回true
    {
        if(Dir >= 0)
        {
            hitDir = 1;
        }
        else
        {
            hitDir = -1;
        }
            //  命中时会向后击退，角度有一定的变化
        health -= damage;
        if(health <= 0)
        {
            Die();
            DeadFly(hitDir);
        }
        return isDead;
    }
    public bool OnHit(float damage , Vector2 speed)         //当单位受伤死亡后，返回true
    {
        if(speed.x >= 0)
        {
            hitDir = 1;
        }
        else
        {
            hitDir = -1;
        }
            //  命中时会向后击退，角度有一定的变化
        health -= damage;
        if(health <= 0)
        {
            Die();
            DeadFly(speed);
        }
        return isDead;
    }
    private void DeadFly(float hitDir)
    {
        transform.GetComponent<BoxCollider2D>().enabled = false;
        rig.velocity = new Vector2(hitDir * 35 , Random.Range(3f , 28f));
    }
    private void DeadFly(Vector2 speed)
    {
        transform.GetComponent<BoxCollider2D>().enabled = false;
        rig.velocity = speed;
    }
    private void setHealth()
    {
        // health = health + 波数 * 一个波数常量;
    }
    public void SetBornPosition(Vector3 ROfenemyBorn)
    {
        //地面敌人需要检测地面位置
    }
}

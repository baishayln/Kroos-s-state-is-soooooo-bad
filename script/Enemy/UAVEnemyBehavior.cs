using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UAVEnemyBehavior : EnemyBehavior
{
    private float moveDir = 1;
    private Vector2 direction;
    private float bornDir;
    private float bornX;
    private float bornYRange;
    private Vector2 bornPoint;
    // private Vector2 bornPoint1;
    private Vector2 realBornPoint;
    new private Vector2 moveSpeed = new Vector2(1 , 0);
    [SerializeField]public float aimTime = 1;
    private float aimTimer;
    private bool isAim = false;
    [SerializeField]public float rayCastDsts = 30;
    private GameObject aimLine;
    [SerializeField]public GameObject aimLinePrefab;
    [SerializeField]public float aimDistense = 30;
    private Transform shootPoint;
    private RaycastHit2D aimResult;
    private RaycastHit2D shootResult;
    private Vector2 aimDir;
    private LineRenderer line;
    [SerializeField]public float attack = 3;
    private bool canFollowPlayer;
    private Vector2 ROfenemyCamera;
    // private Vector3 scale;
    // // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        rig = gameObject.GetComponent<Rigidbody2D>();
        // rig.bodyType = RigidbodyType2D.Static;
        shootPoint = transform.GetChild(0);
        shootTimer = shootInterval;
        cameraPoint = GameObject.Find("Main Camera").transform;
        ROfenemyCamera = CameraBehaviour.Instance.ReturnBornPosition();
        // scale = transform.localScale;
    }
    void OnEnable()
    {
        bornPoint = moveSpeed * -1000;
        player = GameObject.Find("Player");
        setHealth();
        isDead = false;
        deadTimer = 0;
        transform.localScale = Vector3.one;
        transform.GetComponent<BoxCollider2D>().enabled = true;
        transform.rotation = Quaternion.identity;
        health = healthUpLimit;
        aimTimer = 0;
        canFollowPlayer = true;
        isShoot = false;
        shootTimer = shootInterval;
        // cameraPoint = null;
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    
    override protected void Move()
    {
        moveSpeed.y = Mathf.MoveTowards(moveSpeed.y , 0 , Time.deltaTime * 3);
        if (canFollowPlayer)
        {
            if (canFollowPlayer && Mathf.Abs(player.transform.position.y - transform.position.y) > ROfenemyCamera.y * 1.5f)
            {
                if (player.transform.position.y > transform.position.y)
                {
                    moveSpeed.y = 5;
                }
                else
                {
                    moveSpeed.y = -5;
                }
                canFollowPlayer = false;
            }
        }
            
        moveSpeed.x = speed * moveDir;
        rig.velocity = moveSpeed;
        // Mathf.MoveTowards(transform.position.x , player.transform.position.x , Time.deltaTime * 10);
    }
    override protected void ShootStart()
    {
        isShoot = true;
        aimTimer = aimTime;
    }
    override protected void Shoot()
    {
        if (!isDead)
        {
            rig.velocity = Vector2.zero;
        }
        if(!isAim && !isDead)
        {
            isAim = true;
            if(target != null)
            {
                Aim(target.transform.position - shootPoint.position);
            }
            else
            {
                Aim(new Vector2(moveDir , -1));
            }
        }
        if(aimTimer < 0)
        {
            ShootEnd();
        }
        else if(aimTimer < 0.5f && Time.timeScale > 0)
        {
            if (line)
            {
                line.enabled = !line.enabled;
            }
        }
        aimTimer -= Time.deltaTime;
    }
    override protected void SetTimer()
    {
        shootTimer = shootInterval + Random.Range(minInclusive , maxInclusive);
    }
    override protected void ShootEnd()
    {
        shootResult = Physics2D.Raycast(shootPoint.position , aimDir , aimDistense , LayerMask.GetMask("PlayerBody"));
        if (shootResult.transform)
        {
            shootResult.transform.GetComponent<PlayerMove>().onHit(attack , aimDir.x);
        }
        ObjectPool.Instance.PushObject(aimLine);
        aimLine = null;     //需要吗？
        line = null;
        isShoot = false;
        isAim = false;
    }
    virtual protected void Aim(Vector2 dir)
    {
        aimDir = dir;
        aimLine = ObjectPool.Instance.GetObject(aimLinePrefab);
        line = aimLine.GetComponent<LineRenderer>();
        line.enabled = true;
        aimResult = Physics2D.Raycast(shootPoint.position , dir , aimDistense , LayerMask.GetMask("Ground"));
        line.SetPosition(0 , shootPoint.position);
        if(aimResult.point == Vector2.zero)
        {
            line.SetPosition(1 , shootPoint.position + new Vector3(dir.normalized.x * aimDistense , dir.normalized.y * aimDistense , shootPoint.position.z));
        }
        else
        {
            line.SetPosition(1 , aimResult.point);
        }
        // {
        //     line.SetPosition(1 , shootPoint.position + new Vector3(dir.normalized.x * aimDistense , dir.normalized.y * aimDistense , shootPoint.position.z));
        // }
        // else
        // {
        //     line.SetPosition(1 , new Vector3(aimResult.point.x , aimResult.point.y , shootPoint.position.z));
        //     Debug.Log(new Vector3(aimResult.point.x , aimResult.point.y , shootPoint.position.z));
        // }
    }
    override public bool SetBornPosition(Vector3 ROfenemyBorn)
    {
        //地面敌人需要检测地面位置
        if(Random.Range(0 , 2) == 0)
        {
            bornDir = 1;
            moveDir = -1;
        }
        else
        {
            bornDir = -1;
            moveDir = 1;
        }
        bornX = bornDir * Random.Range(ROfenemyBorn.x , ROfenemyBorn.x + 5) + CameraBehaviour.Instance.ReturnCameraPosition().x;
        bornPoint = Physics2D.Raycast(new Vector2(bornX , CameraBehaviour.Instance.ReturnCameraPosition().y + ROfenemyBorn.y * 0.7f) , Vector2.down , ROfenemyBorn.y , LayerMask.GetMask("Ground")).point;
        if(bornPoint != null && bornPoint != Vector2.one * -1000)
        {
            realBornPoint.x = bornX;
            realBornPoint.y = Random.Range( CameraBehaviour.Instance.ReturnCameraPosition().y + 0.5f , CameraBehaviour.Instance.ReturnCameraPosition().y + (ROfenemyBorn.y * 0.7f));
            transform.position = realBornPoint;
        }
        else if(bornPoint.y + 0.5f < CameraBehaviour.Instance.ReturnCameraPosition().y + (ROfenemyBorn.y * 0.7f))
        {
            realBornPoint.x = bornX;
            realBornPoint.y = Random.Range( bornPoint.y + 0.5f , CameraBehaviour.Instance.ReturnCameraPosition().y + (ROfenemyBorn.y * 0.7f));
            transform.position = realBornPoint;
        }
        else
        {
            ObjectPool.Instance.PushObject(gameObject);
            return false;
        }
        return true;
    }
    override public bool OnHit(float damage , float Dir)         //当单位受伤死亡后，返回true
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
            //  或者就是短暂后退
        health -= damage;
        if(health <= 0)
        {
            if(aimTimer >= 0 && line)
            {
                ObjectPool.Instance.PushObject(line.gameObject);
                aimLine = null;     //需要吗？
                line = null;
                isShoot = false;
                isAim = false;
            }
            Die();
            DeadFly(hitDir);
        }
        moveSpeed = new Vector2(Dir , 1);
        return isDead;
    }
    public void SetTarget(GameObject tgt)
    {
        target = tgt.gameObject;
    }
    public void ReMoveTarget()
    {
        target = null;
    }
    override public bool OnHit(float damage , Vector2 speed)         //当单位受伤死亡后，返回true
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
            if(aimTimer >= 0 && line)
            {
                ObjectPool.Instance.PushObject(line.gameObject);
                aimLine = null;     //需要吗？
                line = null;
                isShoot = false;
                isAim = false;
            }
            Die();
            DeadFly(speed);
        }
        moveSpeed = speed.normalized;
        return isDead;
    }
    public override void IsTooLongOfCamera()
    {
        // base.IsTooLongOfCamera();
        
        if (Vector3.Distance(transform.position , cameraPoint.transform.position) > 40)
        {
            if(aimTimer >= 0 && line)
            {
                ObjectPool.Instance.PushObject(line.gameObject);
                aimLine = null;     //需要吗？
                line = null;
                isShoot = false;
                isAim = false;
            }
            SetChildToPool();
            isDead = true;
            ObjectPool.Instance.PushObject(gameObject);
            fightUI.GetComponent<EnemyBornController>().EnemyLoss();
            fightUI.GetComponent<FightUIController>().EnemyLoss();
        }
    }
     
    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if(other.CompareTag("Player"))
    //     {
    //         target = other.gameObject;
    //     }
    // }
    // void OnTriggerExit2D(Collider2D other)
    // {
    //     if(other.CompareTag("EnemyExistenceRange"))
    //     {
    //         ObjectPool.Instance.PushObject(gameObject);
    //     }
    // }
}

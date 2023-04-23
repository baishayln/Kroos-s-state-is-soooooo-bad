using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonWormBehavior : EnemyBehavior , OnHit
{
    private Vector2 direction;
    private float bornDir;
    private float bornX;
    private Vector2 bornPoint;
    [SerializeField]public float rayCastDsts = 30;
    [SerializeField]public float normalAttack = 1;
    [SerializeField]public float dashAttack = 2;
    private float attack;
    private RaycastHit2D bornResult;
    public float moveDir = 0;
    public float escapeDir = 0;
    [SerializeField]public Vector2 checkBoxSize = new Vector2(0.5f , 2);
    private Collider2D groundInFace;

    // // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        rig = gameObject.GetComponent<Rigidbody2D>();
        isFight = true;
        isEscape = false;
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    void OnEnable()
    {
        bornPoint = Vector2.one * -1000;
        player = GameObject.Find("Player");
        setHealth();
        isDead = false;
        deadTimer = 0;
        transform.localScale = Vector3.one;
        transform.GetComponent<BoxCollider2D>().enabled = true;
        transform.rotation = Quaternion.identity;
        health = nowHealthUpLimit;
        attack = normalAttack;
        isFight = true;
        isEscape = false;
        escapeTimer = 0;
    }
    override protected void Move()
    {
        moveSpeed = rig.velocity;

        if (moveSpeed.x > 0)
        {
            moveDir = 1;
        }
        if (moveSpeed.x < 0)
        {
            moveDir = -1;
        }

        if (isFight)
        {
            if(player.transform.position.x - 0.75f > transform.position.x)
            {
                moveSpeed.x = Mathf.MoveTowards(moveSpeed.x , speed , Time.deltaTime * 10);
            }
            else if(player.transform.position.x + 0.75f < transform.position.x)
            {
                moveSpeed.x = Mathf.MoveTowards(moveSpeed.x , -speed , Time.deltaTime * 10);
            }
            else
            {
                moveSpeed.x = Mathf.MoveTowards(moveSpeed.x , 0 , Time.deltaTime * 10);
            }
            
            groundInFace = Physics2D.OverlapBox(transform.position + moveDir * Vector3.right * 1.5f , checkBoxSize , 0 , LayerMask.GetMask("Ground"));
            if (!groundInFace)
            {
                escapeDir = -moveDir;
                isEscape = true;
                isFight = false;
            }
        }
        else if (isEscape)
        {
            moveSpeed.x = Mathf.MoveTowards(moveSpeed.x , speed * escapeDir , Time.deltaTime * 10);
            
            groundInFace = Physics2D.OverlapBox(transform.position + escapeDir * Vector3.right * 1.5f , checkBoxSize , 0 , LayerMask.GetMask("Ground"));
            if (!groundInFace)
            {
                escapeDir = -escapeDir;
                isEscape = true;
                isFight = false;
            }
        }

        if (Mathf.Abs(player.transform.position.x - transform.position.x) < 0.75f && Mathf.Abs(player.transform.position.y - transform.position.y) > 4f && !isEscape)
        {
            escapeDir = moveDir;
            isEscape = true;
            isFight = false;
        }
        else if (Mathf.Abs(player.transform.position.x - transform.position.x) < 0.75f && Mathf.Abs(player.transform.position.y - transform.position.y) > 2f && !isEscape)
        {
            escapeTimer += Time.deltaTime;
            if (escapeTimer > 2)
            {
                escapeDir = moveDir;
                if (moveDir == 0)
                {
                    escapeDir = 1;
                }
                isEscape = true;
                isFight = false;
            }
        }
        else if (Mathf.Abs(player.transform.position.y - transform.position.y) <= 2f)
        {
            escapeTimer = 0;
        }

        // if (isEscape && Mathf.Abs(player.transform.position.y - transform.position.y) < 1.75f)
        // {
        //     isEscape = false;
        //     isFight = true;
        // }
        
        rig.velocity = moveSpeed;
        
        // Mathf.MoveTowards(transform.position.x , player.transform.position.x , Time.deltaTime * 10);
    }
    override protected void Direction()
    {
        if (player && isFight)
        {
            if(player.transform.position.x > transform.position.x)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                transform.localScale = leftScale;
            }
        }
        else
        {
            if(rig.velocity.x > 0)
            {
                transform.localScale = Vector2.one;
            }
            else if(rig.velocity.x < 0)
            {
                transform.localScale = leftScale;
            }
        }
    }
    override public bool SetBornPosition(Vector3 ROfenemyBorn)
    {
        SetHealth();
        //地面敌人需要检测地面位置
        if(Random.Range(0 , 2) == 0)
        {
            bornDir = 1;
        }
        else
        {
            bornDir = -1;
        }
        bornX = bornDir * Random.Range(ROfenemyBorn.x , ROfenemyBorn.x + 5) + CameraBehaviour.Instance.ReturnCameraPosition().x;
        // bornPoint = Physics2D.Raycast(new Vector2(bornX , Random.Range(CameraBehaviour.Instance.ReturnCameraPosition().y + ROfenemyBorn.y * 0.5f , CameraBehaviour.Instance.ReturnCameraPosition().y + ROfenemyBorn.y)) , Vector2.down , rayCastDsts , LayerMask.GetMask("Ground")).point;
        //      新的检测逻辑使用了屏幕的高作为检测长度
        bornResult = Physics2D.Raycast(new Vector2(bornX , Random.Range(CameraBehaviour.Instance.ReturnCameraPosition().y , CameraBehaviour.Instance.ReturnCameraPosition().y + ROfenemyBorn.y)) , Vector2.down , ROfenemyBorn.y * 1.5f , LayerMask.GetMask("Ground"));
        if(bornResult.transform != null)
        {
            bornPoint.y = bornResult.point.y + 0.5f;
            bornPoint.x = bornResult.point.x;
            transform.position = bornPoint;
        }
        else
        {
            // SetBornPosition(ROfenemyBorn);
            // Debug.Log("检测地面失败");
            ObjectPool.Instance.PushObject(gameObject);
            return false;
        }
        return true;
    }
    public void SetHealth()
    {
        if (!fightUI)
        {
            fightUI = GameObject.Find("FightUI");
        }
        nowHealthUpLimit = healthUpLimit + healthIncrease * fightUI.GetComponent<FightUIController>().GetScoreGet() * fightUI.GetComponent<FightUIController>().GetWave();
        health = nowHealthUpLimit;
    }
    override protected void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBody"))
        {
            other.transform.parent.GetComponent<OnHit>().OnHit(attack , other.transform.position.x - transform.position.x);
        }
    }

    public override void IsTooLongOfCamera()
    {
        // base.IsTooLongOfCamera();
        if (cameraPoint)
        {
            if (Vector3.Distance(transform.position , cameraPoint.transform.position) > 25)
            {
                SetChildToPool();
                isDead = true;
                ObjectPool.Instance.PushObject(gameObject);
                // fightUI.GetComponent<EnemyBornController>().EnemyLoss();
                fightUI.GetComponent<FightUIController>().EnemyLoss();
            }
        }
        // if (Vector3.Distance(transform.position , cameraPoint.transform.position) > 40)
        // {
        //     SetChildToPool();
        //     isDead = true;
        //     ObjectPool.Instance.PushObject(gameObject);
        //     fightUI.GetComponent<EnemyBornController>().EnemyLoss();
        //     fightUI.GetComponent<FightUIController>().EnemyLoss();
        // }
    }
}

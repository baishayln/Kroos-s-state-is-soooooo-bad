using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerEnhancementType
{
    AttackEnhancement = 0 , IntervalEnhancement = 1 , RangeEnhancement = 2 , skill1 = 3 , skill2 = 4 , skill3 = 5 , ScatterEnhancement = 7
}
public class PlayerEnhancement : MonoBehaviour
{
    private Rigidbody2D rig;
    private Vector2 direction;
    private Vector2 speed;
    public PlayerEnhancementType type;
    private bool isStartMove = false;
    private bool isMoveToPlayer = false;
    private GameObject target;
    
    // Start is called before the first frame update
    void Start()
    {
        rig = transform.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(isStartMove)
        {
            speed.x = Mathf.MoveTowards(speed.x , 0 , Time.deltaTime * 3);
            speed.y = Mathf.MoveTowards(speed.y , 0 , Time.deltaTime * 3);
            if(speed.x <= 0 && speed.y <= 0)
            {
                isStartMove = false;
            speed.x = 0;
            speed.y = 0;
            }
        }
        if(isMoveToPlayer)
        {
            direction = (target.transform.position - transform.position).normalized;
            speed.x = Mathf.MoveTowards(speed.x , direction.normalized.x * 25 , Mathf.Abs(direction.x) * 42 * Time.deltaTime);
            speed.y = Mathf.MoveTowards(speed.y , direction.normalized.y * 25 , Mathf.Abs(direction.y) * 42 * Time.deltaTime);
            if(Vector2.Distance(transform.position , target.transform.position) <= 1f)
                TouchPlayer(type , target);
        }
        rig.velocity = speed;
    }

    // Update is called once per frame
    void Update()
    {
        //需要一个枚举值来随机当前的奖励是攻击、攻速、射程还是技能的奖励，不同的奖励类型根据枚举触发不同的强化函数，并且设置外观（或者干脆换一个物体）
        //需要一个函数来向玩家加速
        //需要一个collider来检测玩家，当玩家进入一定范围后会将目标指向玩家向其加速移动，用星星弹的加速弹道？和玩家碰撞后会调用玩家的函数
        //不同的强化有一个循环淡入淡出的光晕？
    }
    // public void StartMove(PlayerEnhancementType type)
    // {
    //     this.type = type;
    //     speedX = Random.Range(0f , 2f);
    //     speedY = Random.Range(0f , 2f);
    //     rig.velocity = new Vector2(speedX , speedY);
    // }
    public void StartMove()
    {
        rig = transform.GetComponent<Rigidbody2D>();
        speed.x = Random.Range(-3f , 3f);
        speed.y = Random.Range(-3f , 3f);
        rig.velocity = speed;
        isStartMove = true;
    }
    public void TouchPlayer(PlayerEnhancementType selfType , GameObject player)
    {
        if(selfType == PlayerEnhancementType.AttackEnhancement)     //玩家获得攻击力增益
        {
            player.GetComponent<PlayerShoot>().GetAttackEnhancement();
        }
        if(selfType == PlayerEnhancementType.IntervalEnhancement)   //玩家获得射速增益
        {
            player.GetComponent<PlayerShoot>().GetIntervalEnhancement();
        }
        if(selfType == PlayerEnhancementType.RangeEnhancement)      //玩家获得射程增益
        {
            player.GetComponent<PlayerShoot>().GetRangeEnhancementt();
        }
        if(selfType == PlayerEnhancementType.skill1)                //玩家获得技能一
        {
            player.GetComponent<PlayerShoot>().Getskill1();
        }
        if(selfType == PlayerEnhancementType.skill2)                //玩家获得技能二
        {
            player.GetComponent<PlayerShoot>().Getskill2();
        }
        if(selfType == PlayerEnhancementType.skill3)                //玩家获得技能三
        {
            player.GetComponent<PlayerShoot>().Getskill3();
        }
        if(selfType == PlayerEnhancementType.ScatterEnhancement)    //玩家获取散射增益，减少散射角度
        {
            player.GetComponent<PlayerShoot>().GetScatterEnhancement();
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && target != other.gameObject)
        {
            target = other.gameObject;
            isStartMove = false;
            direction = (other.transform.position - transform.position).normalized;
            rig.velocity = -4 * direction;
            speed = rig.velocity;
            isMoveToPlayer = true;
        }
    }
}

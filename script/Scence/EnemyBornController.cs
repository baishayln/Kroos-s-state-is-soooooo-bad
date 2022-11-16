using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBornController : MonoBehaviour
{
    private Vector3 ROfenemyBorn;
    [SerializeField]public GameObject enemy1;
    [SerializeField]public GameObject enemy2;
    private int enemyCount = 0;
    private float enemyBornTimer = 0;
    [SerializeField]public float enemyBornColdTime = 0.25f;
    private int enemyBornMaxIntervalCount = 4;
    private int enemytIsNotBornIntervalCount = 0;
    private int enemyBornMinIntervalCount = 1;
    private int enemyBornIntervalCount = 0;
    [SerializeField]public int enemyBornProbability = 50;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        // IsEnemyBorn();
        enemyBornTimer += Time.deltaTime;
        if(enemyBornTimer >= enemyBornColdTime)
        {
            enemyBornTimer = 0;
            IsEnemyBorn();
        }
    }
    //需要一个函数用来控制当前每一帧的生成几率
    //需要一个函数来判断当前是否继续生成怪物
    //波次结束有两种方式，一种是时间归零，一种是杀够数量（很多？300？）
    //需要一个boss脚本
    public void BornEnemy()             //生成敌人时会调用此函数，并且在UI上显示当前敌人数量？此函数暴露给敌人生成器脚本或者敌人使用
    {
        enemyCount++;                     //敌人生成脚本应该动态平衡，敌人少时增多，敌人多时减少生成，有一个生成概率每帧有一定概率生成
        // SetEnemy();
    }
    public void EnemyDeath()            //敌人死亡或离开屏幕时调用，当前场景内敌人计数减一
    {
        enemyCount -- ;
    }
    private void IsEnemyBorn()          //此函数每帧或每fixedupdate调用，根据当前场上怪物数量计算这一帧有多少概率生成怪物
    {
        // int x = Random.Range(-1000 , 21);
        // if(x > enemyCount)        //需要修改一下生成判定算法
        // {
        //     BornEnemy();
        //     Debug.Log("随机数为：" + x + "敌人数量：" + enemyCount);
        // }
        
        
        if((Random.Range(0 , 100) > 50 || enemytIsNotBornIntervalCount >= enemyBornMaxIntervalCount) && enemyBornIntervalCount <= 0)        //需要修改一下生成判定算法
        {
            BornEnemy();
            if(enemytIsNotBornIntervalCount >= enemyBornMaxIntervalCount)
                {enemytIsNotBornIntervalCount = 0;}
            enemyBornIntervalCount = enemyBornMinIntervalCount;
            enemytIsNotBornIntervalCount = 0;
            Debug.Log(enemyCount);
        }
        else
        {
            enemytIsNotBornIntervalCount ++;
            enemyBornIntervalCount --;
        }
    }
    private void SetEnemy()
    {
        GameObject newEnemy = ObjectPool.Instance.GetObject(RandomEnemy());
        newEnemy.GetComponent<EnemyBehavior>().SetBornPosition(CameraBehaviour.Instance.ReturnBornPosition());
    }
    private GameObject RandomEnemy()
    {
        switch (Random.Range(0 , 2))
        {
            case 0:
                return enemy1;
            case 1:
                return enemy2;
            default:
                return enemy1;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBornController : MonoBehaviour
{
    private Vector3 ROfenemyBorn;
    [SerializeField]public GameObject CommonWorm;
    [SerializeField]public GameObject UAV;
    [SerializeField]public GameObject enemy3;
    [SerializeField]public GameObject Varon;
    private int enemyCount = 0;
    private float enemyBornTimer = 0;
    [SerializeField]public float enemyBornColdTime = 0.25f;
    private int enemyBornMaxIntervalCount = 4;
    private int enemytIsNotBornIntervalCount = 0;
    private int enemyBornMinIntervalCount = 1;
    private int enemyBornIntervalCount = 0;
    [SerializeField]public int basicEnemyBornProbability = 50;
    private float extraEnemyBornProbability = 0;
    private bool isStopBornEnemy = true;
    [SerializeField]public float relaxTimeBetweenWaves = 10;
    [SerializeField]public float relaxTimer = 0;
    private int nowWave;
    private bool isEndFight = false;
    [SerializeField]public float normalEnemyCount = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        nowWave = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        // IsEnemyBorn();
        if (!isStopBornEnemy)
        {
            enemyBornTimer += Time.deltaTime;
            if(enemyBornTimer >= enemyBornColdTime)
            {
                enemyBornTimer = 0;
                IsEnemyBorn();
            }
        }
        if (relaxTimer > 0 && !isEndFight)
        {
            if (relaxTimer <= Time.deltaTime)
            {
                nowWave ++ ;
                transform.GetComponent<FightUIController>().WaveStart(nowWave);
                // Debug.Log(nowWave);
            }
            relaxTimer -= Time.deltaTime;
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
        if (enemyCount < 0)
        {
            enemyCount = 0;
        }
    }
    public void EnemyLoss()
    {
        enemyCount -- ;
        if (enemyCount < 0)
        {
            enemyCount = 0;
        }
    }
    private void IsEnemyBorn()          //此函数每帧或每fixedupdate调用，根据当前场上怪物数量计算这一帧有多少概率生成怪物
    {
        // int x = Random.Range(-1000 , 21);
        // if(x > enemyCount)        //需要修改一下生成判定算法
        // {
        //     BornEnemy();
        //     Debug.Log("随机数为：" + x + "敌人数量：" + enemyCount);
        // }
        if(normalEnemyCount - enemyCount >= 0)
        {
            extraEnemyBornProbability = 100/normalEnemyCount * (normalEnemyCount - enemyCount);
        }
        if((Random.Range(0 , 100) < basicEnemyBornProbability + extraEnemyBornProbability || enemytIsNotBornIntervalCount >= enemyBornMaxIntervalCount) && enemyBornIntervalCount <= 0)        //需要修改一下生成判定算法
        {
            BornEnemy();
            SetEnemy();
            if(enemytIsNotBornIntervalCount >= enemyBornMaxIntervalCount)
                {enemytIsNotBornIntervalCount = 0;}
            enemyBornIntervalCount = enemyBornMinIntervalCount;
            enemytIsNotBornIntervalCount = 0;
        }
        else
        {
            // enemytIsNotBornIntervalCount ++; 
            enemyBornIntervalCount --;
        }
    }
    private void SetEnemy()
    {
        transform.GetComponent<FightUIController>().BornEnemy();
        GameObject newEnemy = ObjectPool.Instance.GetObject(RandomEnemy());
        newEnemy.transform.SetParent(null);
        if(!newEnemy.GetComponent<EnemyBehavior>().SetBornPosition(CameraBehaviour.Instance.ReturnBornPosition()))
        {
            GameObject newEnemy1 = ObjectPool.Instance.GetObject(UAV);
            newEnemy1.transform.SetParent(null);
            newEnemy1.GetComponent<EnemyBehavior>().SetBornPosition(CameraBehaviour.Instance.ReturnBornPosition());
        }
    }
    private GameObject RandomEnemy()
    {
        switch (Random.Range(0 , 2))
        {
            case 0:
                return CommonWorm;
            case 1:
                return UAV;
            case 2:
                return enemy3;
            case 999:
                return Varon;
            default:
                return CommonWorm;
        }
    }
    public void PauseBornEnemy()
    {
        isStopBornEnemy = true;
    }
    public void ContinueBornEnemy()
    {
        isStopBornEnemy = false;
    }
    public void StopBornEnemy()
    {
        enemyCount = 0;
        isStopBornEnemy = true;
        relaxTimer = relaxTimeBetweenWaves;
    }
    public void StartBornEnemy()
    {
        isStopBornEnemy = false;
    }
    public void EndFight()
    {
        isEndFight = true;
    }
}

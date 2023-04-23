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
    [SerializeField]public GameObject sponsoredUVA;
    [SerializeField]public GameObject[] enemys;
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
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        // ContinuouslyGenerateEnemy();
    }
    //需要一个函数用来控制当前每一帧的生成几率
    //需要一个函数来判断当前是否继续生成怪物
    //波次结束有两种方式，一种是时间归零，一种是杀够数量（很多？300？）
    //需要一个boss脚本

    public void BornAllEnemy(float time)
    {
        StartCoroutine(BornAllEnemyIE(time));
    }
    IEnumerator BornAllEnemyIE(float time)
    {
        if (GameController.Instance.GetSsponsoredUVACount() > 0)
        {
            yield return StartCoroutine(GenerateEnemiesIE(sponsoredUVA , GameController.Instance.GetSsponsoredUVACount() , 1.5f));
            yield return new WaitForSeconds(time);
        }
        yield return StartCoroutine(GenerateBossAfterTimeIE(Varon , 3));
        GameController.Instance.ResetSponsoredUVACount();
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
    private void SetEnemy(GameObject obj)
    {
        transform.GetComponent<FightUIController>().BornEnemy();
        GameObject newEnemy = ObjectPool.Instance.GetObject(obj);
        newEnemy.transform.SetParent(null);
        if(!newEnemy.GetComponent<EnemyBehavior>().SetBornPosition(CameraBehaviour.Instance.ReturnBornPosition()))
        {
            GameObject newEnemy1 = ObjectPool.Instance.GetObject(UAV);
            newEnemy1.transform.SetParent(null);
            newEnemy1.GetComponent<EnemyBehavior>().SetBornPosition(CameraBehaviour.Instance.ReturnBornPosition());
        }
    }
    private void SetBoss(GameObject obj)
    {
        transform.GetComponent<FightUIController>().BornEnemy();
        GameObject newEnemy = ObjectPool.Instance.GetObject(obj);
        newEnemy.transform.SetParent(null);
        GameController.Instance.SetBossInGame(newEnemy);
        newEnemy.GetComponent<BossBorn>().SetBornPosition(CameraBehaviour.Instance.ReturnBornPosition());
        newEnemy.GetComponent<BossBorn>().SetBossHealthBar(transform.GetComponent<FightUIController>().GetHealthBar());
        // transform.GetComponent<FightUIController>().HealthBarBorn();
    }
    private GameObject RandomEnemy()
    {
       return ObjectPool.Instance.GetObject(enemys[Random.Range(0 , enemys.Length)]);
    }
    public void EnemyBorn(GameObject obj)
    {
        ObjectPool.Instance.GetObject(obj);
    }
    public void PauseBornEnemy()
    {
        isStopBornEnemy = true;
    }
    public void ContinueBornEnemy()
    {
        isStopBornEnemy = false;
    }
    public void StartBornEnemy()
    {
        isStopBornEnemy = false;
    }
    public void EndFight()
    {
        isEndFight = true;
    }
    public void GenerateEnemies(GameObject enemy , int enemynum , float interval)
    {
        StartCoroutine(GenerateEnemiesIE(enemy , enemynum , interval));
    }
    public void GenerateEnemyAfterTime(GameObject enemy , float waitTime)
    {
        StartCoroutine(GenerateEnemyAfterTimeIE(enemy , waitTime));
    }
    public void GenerateBossAfterTime(GameObject enemy , float waitTime)
    {
        StartCoroutine(GenerateBossAfterTimeIE(enemy , waitTime));
    }
    private void ContinuouslyGenerateEnemy()
    {
        if (!isStopBornEnemy)
        {
            enemyBornTimer += Time.deltaTime;
            if(enemyBornTimer >= enemyBornColdTime)
            {
                enemyBornTimer = 0;
                // IsEnemyBorn();
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
    IEnumerator GenerateEnemiesIE(GameObject enemy , int enemynum , float interval)
    {
        for (int i = 0; i < enemynum; i++)
        {
            SetEnemy(enemy);
            yield return new WaitForSeconds(interval);
        }
    }
    IEnumerator GenerateEnemyAfterTimeIE(GameObject enemy , float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SetEnemy(enemy);
    }
    IEnumerator GenerateBossAfterTimeIE(GameObject enemy , float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SetBoss(enemy);
    }
}

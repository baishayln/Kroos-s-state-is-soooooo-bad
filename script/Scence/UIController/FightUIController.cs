using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FightUIController : MonoBehaviour
{
    private Image Skill1Image;
    private Image Skill2Image;
    private Image SkillCold1;
    private Image SkillCold2;
    private Text SkillColdText1;
    private Text SkillColdText2;
    private Sprite emptySkillImage;
    private Animator animator;
    private float skillColdTimer;
    private float skillColdThisTime;
    private int numOfRewardOfNowWave = 0;
    private Dictionary<StartDeBuff , string> startDebuffTypeDir = new Dictionary<StartDeBuff, string>();
    private Dictionary<StartDeBuff , string> startDebuffDescribeDir = new Dictionary<StartDeBuff, string>();
    private Dictionary<StartDeBuff , Color> startDebuffColor = new Dictionary<StartDeBuff, Color>();
    private StartDeBuff[] playersDebuffs;
    [SerializeField]public float preparationTime = 3;
    private bool isPreparationStart = false;
    [SerializeField]public float startTime = 3;
    private bool isFightStart = false;
    private float gameStartTimer = 0;
    private Text buffName;
    private Text buffDescribe;
    private AnimatorStateInfo info;
    private AnimatorStateInfo startInfo;
    private GameObject player;
    private int enemyCount;
    private int enemyHasDeath = 0;
    private float enemyBornTimer;
    [SerializeField]public float enemyBornColdTime = 0.25f;
    private int enemyBornMaxIntervalCount = 4;
    private int enemyBornIntervalCount;
    private int enemyBornMinIntervalCount = 2;
    private int enemytIsNotBornIntervalCount;
    private GameObject healthBar;
    private Text healthText;
    private float healthLimit;
    private float health;
    private int nowWave = 0;
    private int enemyDeathInNowWave = 0;
    [SerializeField]public int nowWaveEnemyKillAtLeast = 15;
    private float nowWaveTime = 0;
    [SerializeField]public float nowWaveTimeAtLeast = 30;
    private bool isInWave = true;
    private Text enemyKill;
    private int[] enemyKillInWaves = new int[3];
    private int totleScore;
    private bool isEndFight = false;
    private int totleEnemyKillCount;
    [SerializeField]public Text totleEnemyKillCountText;
    [SerializeField]public Text totleScoreText;
    private bool isPause = false;
    private GameObject setting;
    [SerializeField]private AudioClip uiEffect;
    [SerializeField]private AudioClip BGM;
    [SerializeField]private float level;
    [SerializeField]private float scoreGet = 1;
    // Start is called before the first frame update
    void Start()
    {
        Skill1Image = transform.GetChild(1).GetComponent<Image>();
        Skill2Image = transform.GetChild(0).GetComponent<Image>();
        SkillCold1 = Skill1Image.transform.GetChild(0).GetComponent<Image>();
        SkillCold2 = Skill2Image.transform.GetChild(0).GetComponent<Image>();
        SkillColdText1 = Skill1Image.transform.GetChild(1).GetComponent<Text>();
        SkillColdText2 = Skill2Image.transform.GetChild(1).GetComponent<Text>();
        buffName = transform.GetChild(2).GetChild(0).GetComponent<Text>();
        buffDescribe = transform.GetChild(2).GetChild(1).GetComponent<Text>();
        animator = transform.GetComponent<Animator>();
        startInfo = animator.GetCurrentAnimatorStateInfo(0);

        healthBar = transform.GetChild(3).gameObject;
        healthText = healthBar.transform.GetChild(3).gameObject.GetComponent<Text>();

        enemyKill = transform.GetChild(5).gameObject.GetComponent<Text>();
        enemyKill.text = "?????????????????????" + enemyDeathInNowWave;

        setting = transform.GetChild(6).gameObject;

        player = GameObject.Find("Player");

        Skill1Image.sprite = player.GetComponent<PlayerShoot>().GetSkillSprite(0);
        Skill2Image.sprite = player.GetComponent<PlayerShoot>().GetSkillSprite(1);

        startDebuffTypeDir.Add(StartDeBuff.AttackDown , "???????????????");
        startDebuffDescribeDir.Add(StartDeBuff.AttackDown , "???????????????????????????????????????????????????????????????????????????????????????????????????????????????1???????????????????????????????????????/n????????????????????????????????????????????????");
        startDebuffColor.Add(StartDeBuff.AttackDown , new Color(196f/255 , 39f/255 , 212f/255 , 255f/255));//??????

        startDebuffTypeDir.Add(StartDeBuff.IntervalDown , "???????????????");
        startDebuffDescribeDir.Add(StartDeBuff.IntervalDown , "??????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????/n???????????????????????????????????????/n?????????????????????");
        startDebuffColor.Add(StartDeBuff.IntervalDown , new Color(154f/255 , 46f/255 , 81f/255 , 255f/255));//??????

        startDebuffTypeDir.Add(StartDeBuff.ScatterDown , "?????????????????????");
        startDebuffDescribeDir.Add(StartDeBuff.ScatterDown , "????????????????????????I like?????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????/n????????????????????????????????????????????????/n?????????????????????");
        startDebuffColor.Add(StartDeBuff.ScatterDown , Color.black);//??????

        startDebuffTypeDir.Add(StartDeBuff.RangeDown , "?????????????????????");
        startDebuffDescribeDir.Add(StartDeBuff.RangeDown , "????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????/n????????????????????????????????????~?????????????????????");
        startDebuffColor.Add(StartDeBuff.RangeDown , Color.yellow);//??????

        startDebuffTypeDir.Add(StartDeBuff.CantStopShoot , "???????????????????????????");
        startDebuffDescribeDir.Add(StartDeBuff.CantStopShoot , "?????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????");
        startDebuffColor.Add(StartDeBuff.CantStopShoot , new Color(255/255 , 139f/255 , 117f/255 , 255f/255));//??????

        startDebuffTypeDir.Add(StartDeBuff.DoubleGun , "?????????");
        startDebuffDescribeDir.Add(StartDeBuff.DoubleGun , "??????????????????????????????????????????");
        startDebuffColor.Add(StartDeBuff.DoubleGun , new Color(150/255 , 150/255 , 150/255 , 255f/255));//??????????????????

        startDebuffTypeDir.Add(StartDeBuff.kaminohikari , "?????????????????????");
        startDebuffDescribeDir.Add(StartDeBuff.kaminohikari , "???????????????????????????");
        startDebuffColor.Add(StartDeBuff.kaminohikari , Color.yellow);//??????

        startDebuffTypeDir.Add(StartDeBuff.tnnd , "??????????????????");
        startDebuffDescribeDir.Add(StartDeBuff.tnnd , "?????????????????????????????????");
        startDebuffColor.Add(StartDeBuff.tnnd , Color.green);//??????
        //???????????????????????????
        //?????????????????????????????????????????????????????????
        //??????????????????????????????????????????
        //??????????????????????????????????????????????????????????????????????????????
        //?????????????????????        
        //?????????????????????        //?????????????????????
        //???~??????????????????????????????   //????????????????????????????????????
        //????????????????????????????????????????????????  //
        //??????????????????????????????              //???????????????
        //?????????????????????                    //???????????????
        //                                  //????????????????????????????????????
        //???????????????????????????????????????????????????????????????????????????    //?????????????????????
        // startDebuffType.Add(StartDeBuff , "");
        // startDebuffDescribe.Add(StartDeBuff);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && CanChangeSkill())
        {
            ChangeSkill();
        }
        if(!isPreparationStart)
        {
            gameStartTimer += Time.deltaTime;
            if(gameStartTimer > preparationTime)
            {
                isPreparationStart = true;
                gameStartTimer = 0;
                StartCoroutine(GameStart());
            }
        }
        // if(isPreparationStart && !isFightStart)
        // {
        //     gameStartTimer += Time.deltaTime;
        //     if(gameStartTimer > startTime)
        //     {
        //         isFightStart = true;
        //         gameStartTimer = 0;
        //         // FightStart();
        //     }
        // }
        if (nowWaveTime < nowWaveTimeAtLeast + 1 && isInWave)
        {
            nowWaveTime += Time.deltaTime;
        }
        if (nowWaveTime > nowWaveTimeAtLeast + 1 && enemyDeathInNowWave >= nowWaveEnemyKillAtLeast && isInWave)
        {
            EndWaves();
            // Debug.Log("??????????????????");
        }
        // BornEnemy();
        if(!isEndFight && Input.GetKeyDown(KeyCode.Escape))
        {
            // isPause = !isPause;
            // setting.SetActive(isPause);
            if (isPause)
            {
                Time.timeScale = 1;
                isPause = !isPause;
                setting.SetActive(isPause);
            }
            else
            {
                Time.timeScale = 0;
                isPause = !isPause;
                setting.SetActive(isPause);
            }
        }
    }
    public void ReturnGame()
    {
        setting.SetActive(false);
        Time.timeScale = 1;
    }

    void FixedUpdate()
    {
        if(skillColdTimer >= 0)
        {
            SkillColdDown();
        }
    }

    //??????????????????????????????????????????????????????????????????????????????????????????????????????
    public void ChangeSkill()
    {
        animator.Play("ChangeSkill");
    }
    public void ChangeSkillImage()
    {
        emptySkillImage = Skill1Image.sprite;
        Skill1Image.sprite = Skill2Image.sprite;
        Skill2Image.sprite = emptySkillImage;
        player.GetComponent<PlayerShoot>().ChangeSkill();
    }
    public void SetSkillImage(int count , Sprite image)
    {
        if (count == 0)
        {
            Skill1Image.sprite = image;
        }
        else
        {
            Skill2Image.sprite = image;
        }
    }
    //??????????????????????????????????????????CD???????????????????????????????????????cd???????????????timer????????????????????????
    public void UseSkill(float skillCold)
    {
        skillColdThisTime = skillCold;
        skillColdTimer = skillColdThisTime;
        SkillColdText1.enabled = true;
        SkillColdText2.enabled = true;
    }
    //???????????????update??????????????????????????????CD???????????????
    private void SkillColdDown()
    {
        skillColdTimer -= Time.deltaTime;
        SkillCold1.fillAmount = skillColdTimer/skillColdThisTime;
        SkillCold2.fillAmount = skillColdTimer/skillColdThisTime;
        if(skillColdTimer > 10)
        {
            SkillColdText1.text = skillColdTimer.ToString("f0");
            SkillColdText2.text = skillColdTimer.ToString("f0");
        }
        else
        {
            SkillColdText1.text = skillColdTimer.ToString("f1");
            SkillColdText2.text = skillColdTimer.ToString("f1");
        }
        if(skillColdTimer < 0)
        {
            SkillColdText1.enabled = false;
            SkillColdText2.enabled = false;
        }
    }
    //???????????????????????????????????????????????????
    private void healthBarChange()
    {

    }
    //?????????????????????????????????????????????????????????????????????????????????????????????????????????DEBUFF
    private void PlaceDebuffSprite()
    {

    }
    IEnumerator GameStart()
    {
        transform.GetComponent<EnemyBornController>().PauseBornEnemy();
        playersDebuffs = player.GetComponent<PlayerShoot>().GiveDebuffs(2);
        if (Random.Range(0 , 100) < 34)
        {
            StartDeBuff[] sttDebuffs = playersDebuffs;
            playersDebuffs = new StartDeBuff[3];
            playersDebuffs[2] = sttDebuffs[0];
            playersDebuffs[1] = sttDebuffs[1];
            if(Random.Range(0 , 100) < 50)
            {
                playersDebuffs[0] = StartDeBuff.CantStopShoot;
            }
            else
            {
                playersDebuffs[0] = StartDeBuff.DoubleGun;
            }
        }
        if (Random.Range(0 , 100) < 34)
        {
            StartDeBuff[] sttDebuffs = playersDebuffs;
            playersDebuffs = new StartDeBuff[3];
            playersDebuffs[2] = sttDebuffs[0];
            playersDebuffs[1] = sttDebuffs[1];
            if(Random.Range(0 , 100) < 50)
            {
                playersDebuffs[0] = StartDeBuff.kaminohikari;
            }
            else
            {
                playersDebuffs[0] = StartDeBuff.tnnd;
            }
        }
        for(int i = playersDebuffs.Length ; i > 0 ; i --)
        {
            player.GetComponent<PlayerShoot>().GetDebuff(playersDebuffs[i - 1]);
            GetBuffContent(playersDebuffs[i - 1]);
            while((!info.IsName("ShowBuff") && !info.IsName("ShowBuff1")) || info.normalizedTime > 0.1f)    //????????????????????????????????????????????????buff????????????????????????????????????????????????????????????????????????????????????????????????0.1????????????????????????????????????buff??????????????????????????????????????????????????????buff????????????????????????????????????????????????????????????buff?????????????????????????????????????????????????????????buff????????????????????????????????????
            {
                info = animator.GetCurrentAnimatorStateInfo(0);
                yield return null;
            }
            yield return StartCoroutine(IsAnimatorEnd());
        }
        
        transform.GetComponent<EnemyBornController>().ContinueBornEnemy();
    }
    IEnumerator IsAnimatorEnd()
    {
        while(info.normalizedTime < 0.95f)
        {
            info = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
        animator.Play("New State");
    }
    // private void GameStart()             //?????????????????????debuff  //???????????????
    // {
    //     for(int i = playersDebuffs.Length ; i > 0 ; i --)
    //     {
    //         GetBuffContent(playersDebuffs[i]);
    //     }
    // }
    public void GetBuffContent(StartDeBuff type)
    {
        buffName.text = startDebuffTypeDir[type];
        buffDescribe.text = startDebuffDescribeDir[type];
        buffName.GetComponent<Text>().color = startDebuffColor[type];
        buffDescribe.GetComponent<Text>().color = startDebuffColor[type];
        info = animator.GetCurrentAnimatorStateInfo(0);
        if(info.IsName("ShowBuff"))
        {
            animator.Play("ShowBuff1");
        }
        else if(info.IsName("ShowBuff1"))
        {
            animator.Play("ShowBuff");
        }
        else
        {
            animator.Play("ShowBuff");
        }
    }
    // private void FightStart()           //??????????????????????????????  //???????????????
    // {
    //     transform.GetComponent<EnemyBornController>().StartBornEnemy();
    // }
    private void GameOver()              //??????????????????????????????
    {
        
    }
    private void SettlementScreen()      //??????????????????????????????????????????????????????
    {
                                        //???????????????????????????????????????????????????????????????
                                        //????????????debuff?????????
    }
    private void CalculateScore()        //????????????????????????????????????????????????
    {

    }
    private void RewardUp()
    {
        numOfRewardOfNowWave ++ ;
    }
    public void BornEnemy()             //?????????????????????????????????????????????UI???????????????????????????????????????????????????????????????????????????????????????
    {
        enemyCount++;                     //???????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????
    }
    public void EnemyDeath()            //????????????????????????????????????????????????????????????????????????
    {
        enemyCount -- ;
        enemyDeathInNowWave ++ ;
        enemyKill.text = "?????????????????????" + enemyDeathInNowWave;
    }
    public void EnemyLoss()
    {
        enemyCount -- ;
    }
    public void WaveStart(int wave)
    {
        // animator.Play("WaveStart");
        isInWave = true;
        nowWave = wave;
        if (wave > 1)
        {
            enemyKillInWaves[wave - 2] = enemyDeathInNowWave;       //?????????????????????????????????
        }
        enemyDeathInNowWave = 0;
        nowWaveTime = 0;
        enemyKill.text = "?????????????????????" + enemyDeathInNowWave;
        transform.GetComponent<EnemyBornController>().StartBornEnemy();
    }
    public int EnemyNumNow()
    {
        return enemyCount;
    }
    private bool IsLastEnemyAndNoReward()           //????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????
    {
        return true;
    }
    private void EndWaves()              //??????????????????????????????????????????????????????0??????????????????????????????????????????????????????????????????????
    {
        isInWave = false;
        numOfRewardOfNowWave = 0;
        transform.GetComponent<EnemyBornController>().StopBornEnemy();
        if (nowWave == 3)
        {
            EndFight();
            transform.GetComponent<EnemyBornController>().EndFight();
        }
    }
    public float GetSkillCold()
    {
        return skillColdTimer;
    }
    public bool CanChangeSkill()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);
        if(!info.IsName("ChangeSkill") && !(info.IsName("ShowBuff") || info.IsName("ShowBuff1")))   //???????????????????????????????????????????????????????????????????????????????????????????????????debuff????????????????????????
            {return true;}
        else
            {return false;}
    }
    public float SetHealthBar(float healthLimit , float health)
    {
        this.healthLimit = healthLimit;
        this.health = health;
        if (!healthText)
        {
            healthBar = transform.GetChild(3).gameObject;
            healthText = healthBar.transform.GetChild(3).gameObject.GetComponent<Text>();
        }
        healthText.text = health + "/" + healthLimit;
        healthBar.GetComponent<Slider>().value = health / healthLimit;
        return health/healthLimit;
    }
    private void EndFight()
    {
        enemyKillInWaves[2] = enemyDeathInNowWave;
        animator.Play("ShowScore");
        player.GetComponent<PlayerMove>().enabled = false;
        player.GetComponent<PlayerShoot>().enabled = false;
        isInWave = false;
        isEndFight = true;
        GameObject[] allObj = SceneManager.GetActiveScene().GetRootGameObjects();
        for(int i = allObj.Length ; i > 0 ; i --)
        {
            if(allObj[i - 1].CompareTag("Enemy"))
            {
                Debug.Log(allObj[i - 1]);
                ObjectPool.Instance.PushObject(allObj[i - 1]);
            }
        }
    }
    public void ShowScorePublic()
    {
        StartCoroutine(ShowScore());
    }
    public void EndShowScore()
    {
        transform.GetChild(4).gameObject.SetActive(true);
        // for(int i = transform.childCount ; i > 0 ; i --)
        // {
            
        //     if(allObj[i - 1].CompareTag("Enemy"))
        //     {
        //         Debug.Log(allObj[i - 1]);
        //         ObjectPool.Instance.PushObject(allObj[i - 1]);
        //     }
        // }
    }
    public void PlayDong()
    {
        if (uiEffect)
        {
            SoundManager.Instance.PlayEffectSound(uiEffect);
        }
    }
    public void PlayBGM()
    {
        if (BGM)
        {
            SoundManager.Instance.PlayEffectSound(BGM);
        }
    }
    public void PausePlayAnimator()
    {
        animator.speed = 0f;
    }
    public void ContinuePlayAnimator()
    {
        animator.speed = 1f;
    }
    IEnumerator ShowScore()
    {
        float timeOfRollScore = 1;
        totleEnemyKillCount = enemyKillInWaves[0] + enemyKillInWaves[1] + enemyKillInWaves[2];
        totleScore = Mathf.FloorToInt((enemyKillInWaves[0] + enemyKillInWaves[1] * 2 + enemyKillInWaves[2] * 3) * scoreGet);
        totleEnemyKillCountText.text = 0.ToString();
        totleScoreText.text = 0.ToString();
        while(timeOfRollScore > 0)
        {
            totleEnemyKillCountText.text = Random.Range(0 , 999).ToString();
            timeOfRollScore -= Time.deltaTime;
            yield return null;
        }
        totleEnemyKillCountText.text = totleEnemyKillCount.ToString();
        yield return new WaitForSeconds(1);
        timeOfRollScore = 1;
        while(timeOfRollScore > 0)
        {
            totleScoreText.text = Random.Range(0 , 999).ToString();
            timeOfRollScore -= Time.deltaTime;
            yield return null;
        }
        totleScoreText.text = totleScore.ToString();
    }
}

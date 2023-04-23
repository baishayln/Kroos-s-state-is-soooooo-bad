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
    private int longmenCoinGet;
    private bool isEndFight = false;
    private int totleEnemyKillCount;
    private float missionComplitePersent;
    [SerializeField]public Text totleEnemyKillCountText;
    [SerializeField]public Text totleScoreText;
    private bool isPause = false;
    [SerializeField]private AudioClip pauseEffect;
    private GameObject setting;
    [SerializeField]private AudioClip uiEffect;
    [SerializeField]private AudioClip BGM;
    [SerializeField]private float level;
    [SerializeField]private float scoreGet = 1;
    [SerializeField]private float neutralBuffProbablity = 34;
    [SerializeField]private float getSkillProbablity = 34;
    [SerializeField]private int debuffCount = 2;
    [SerializeField]private Text fightEndVoice;
    [SerializeField]private Image kroosFace;
    [SerializeField]private Sprite gameoverImage;
    [SerializeField]private Sprite gameEndImage;
    [SerializeField]public Slider loadSlider;
    [SerializeField]public Text loadingText;
    [SerializeField]public GameObject loadScreen;
    [SerializeField]public Sprite pauseButtonSprite;
    [SerializeField]public Sprite playButtonSprite;
    [SerializeField]public Image pauseButton;
    private float gameTime = 0;
    private bool isGameTiming = false;
    private int UVAcount;
    [SerializeField]private float BornEnemyWaitTime = 15;
    [SerializeField]public GameObject BossHealthBar;
    [SerializeField]private bool isTest = false;
    [SerializeField]public GameObject testObj;
    private bool isEndShow;
    private float scoreMut = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        GlobleDamageCounter.Instance.ResetDamage();
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
        if (!pauseButton)
        {
            pauseButton = transform.GetChild(8).gameObject.GetComponent<Image>();
        }

        healthBar = transform.GetChild(3).gameObject;
        healthText = healthBar.transform.GetChild(3).gameObject.GetComponent<Text>();

        // enemyKill = transform.GetChild(5).gameObject.GetComponent<Text>();
        // enemyKill.text = "本轮击败敌人：" + enemyDeathInNowWave;

        setting = transform.GetChild(6).gameObject;

        startDebuffTypeDir.Add(StartDeBuff.AttackDown , "血肉畸变！");
        startDebuffDescribeDir.Add(StartDeBuff.AttackDown , "克洛丝与凯尔希一起潜入深海营救沉迷烤海嗣的博士，却在行动过程中被不幸投出了1的屑博士献祭了，人神共愤！\n\n克洛丝的攻击被降低了，效果拔群！");
        startDebuffColor.Add(StartDeBuff.AttackDown , new Color(196f/255 , 39f/255 , 212f/255 , 255f/255));//紫色

        startDebuffTypeDir.Add(StartDeBuff.IntervalDown , "睡眠不足！");
        startDebuffDescribeDir.Add(StartDeBuff.IntervalDown , "昨晚克洛丝和博士被博士拖着在办公室玩游戏玩了个爽，今天早上还要参加行动，这让本就不足的睡眠雪上加霜！\n\n克洛丝，你为什么只是看着！\n攻击间隔增大！");
        startDebuffColor.Add(StartDeBuff.IntervalDown , new Color(154f/255 , 46f/255 , 81f/255 , 255f/255));//粉色

        startDebuffTypeDir.Add(StartDeBuff.ScatterDown , "物理神经损伤！");
        startDebuffDescribeDir.Add(StartDeBuff.ScatterDown , "昨晚罗德岛有凶恶博士出没压榨员工，克洛丝今天的手有点哆哆嗦嗦的！\n\n什么嘛……我射的还是蛮准的吗……\n攻击散射增大！");
        startDebuffColor.Add(StartDeBuff.ScatterDown , Color.black);//黑色

        startDebuffTypeDir.Add(StartDeBuff.RangeDown , "武器校准过度！");
        startDebuffDescribeDir.Add(StartDeBuff.RangeDown , "“我的、我的王之力啊！！！！！”——可露希尔在校准武器时由于操作不规范而大喊道。\n\n发生这种事情大家都不想的~武器射程下降！");
        startDebuffColor.Add(StartDeBuff.RangeDown , Color.yellow);//黄色

        startDebuffTypeDir.Add(StartDeBuff.CantStopShoot , "达瓦里希！伏特加！");
        startDebuffDescribeDir.Add(StartDeBuff.CantStopShoot , "攻击速度上升！但无法停止射击！是假酒！我在金酒之杯里加了假酒！");       //跟！本！停！不！下！来！
        startDebuffColor.Add(StartDeBuff.CantStopShoot , new Color(255/255 , 139f/255 , 117f/255 , 255f/255));//肉色

        startDebuffTypeDir.Add(StartDeBuff.DoubleGun , "双持！");
        startDebuffDescribeDir.Add(StartDeBuff.DoubleGun , "双枪老太婆！不是霜枪老太婆！");
        startDebuffColor.Add(StartDeBuff.DoubleGun , new Color(150/255 , 150/255 , 150/255 , 255f/255));//灰色？银色？

        startDebuffTypeDir.Add(StartDeBuff.kaminohikari , "神说，要有光。");
        startDebuffDescribeDir.Add(StartDeBuff.kaminohikari , "神是仁慈的。客门。");
        startDebuffColor.Add(StartDeBuff.kaminohikari , Color.yellow);//金色

        startDebuffTypeDir.Add(StartDeBuff.tnnd , "闪开！我来！");
        startDebuffDescribeDir.Add(StartDeBuff.tnnd , "他奶奶的，姑奶奶来了！");
        startDebuffColor.Add(StartDeBuff.tnnd , Color.green);//绿色

        startDebuffTypeDir.Add(StartDeBuff.FishRocket , "这不是很好吃吗！");
        startDebuffDescribeDir.Add(StartDeBuff.FishRocket , "博士曾经说过，夏天吃鲱鱼罐头就像是凌晨四点在厕所吃泡面，回味无穷又震慑人心。");
        startDebuffColor.Add(StartDeBuff.FishRocket , new Color(181/255 , 115/255 , 0/255 , 255f/255));//屎黄色
        //有内鬼，终止交易！
        //五年，五年！你知道我这五年怎么过的吗！
        //克洛丝！你在干什么啊克洛丝！
        //非常抱歉，本视频可能由于以下原因导致无法正常播放……
        //三年之后又三年        
        //让子弹飞一会！        //获得射程增益？
        //哟~这不显得您枪法准嘛！   //获得射击散射归零时的提示
        //你管得了我，还管得了观众爱看谁！  //
        //博士！我不做医疗了！              //嘉维尔技能
        //神说，要有光。                    //卡密的技能
        //                                  //可连续射击的提示是什么？
        //达瓦里希，伏特加！我！控！几！不！住！我！吉！几！    //无法停止射击？
        // startDebuffType.Add(StartDeBuff , "");
        // startDebuffDescribe.Add(StartDeBuff);


        if (GameController.Instance && GameController.Instance.GetCharacter())
        {
            GameObject.Instantiate(GameController.Instance.GetCharacter() , GameObject.Find("PlayerBornPoint").transform.position , Quaternion.identity).name = "Player";
            // ObjectPool.Instance.GetObject(PlayerCharacterController.Instance.GetCharacter()).transform.position = GameObject.Find("PlayerBornPoint").transform.position;
        }

        player = GameObject.Find("Player");

        // Skill1Image.sprite = player.GetComponent<PlayerShoot>().GetSkillSprite(0);
        // Skill2Image.sprite = player.GetComponent<PlayerShoot>().GetSkillSprite(1);
    }
    void OnEnable()
    {
        GlobleDamageCounter.Instance.ResetDamage();
        ObjectPool.Instance.ClearQueue();
        isEndShow = false;
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
        if (nowWaveTime < nowWaveTimeAtLeast + 1 && isInWave)
        {
            nowWaveTime += Time.deltaTime;
        }
        if (nowWaveTime > nowWaveTimeAtLeast + 1 && enemyDeathInNowWave >= nowWaveEnemyKillAtLeast && isInWave)
        {
            EndWaves();
        }
        if(!isEndFight && Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        if (isGameTiming)
        {
            gameTime += Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.L) && isTest)
        {
            ObjectPool.Instance.GetObject(testObj);
        }
    }
    void FixedUpdate()
    {
        if(skillColdTimer >= 0)
        {
            SkillColdDown();
        }
    }

    public void Pause()
    {
        if (isPause)
        {
            Time.timeScale = 1;
            isPause = !isPause;
            setting.SetActive(isPause);
            pauseButton.sprite = pauseButtonSprite;
        }
        else
        {
            Time.timeScale = 0;
            isPause = !isPause;
            setting.SetActive(isPause);
            pauseButton.sprite = playButtonSprite;
        }
        player.GetComponent<PlayerMove>().PauseGame(!isPause);
        player.GetComponent<PlayerShoot>().PauseGame(!isPause);     //当isPause为true时，玩家无法射击，游戏运行速度为0，显示暂停提示框
        SoundManager.Instance.PlayEffectSound(pauseEffect);
    }
    public void ReturnGame()
    {
        Pause();
    }
    public void StartTiming()
    {
        isGameTiming = true;
    }
    public void StopTiming()
    {
        isGameTiming = false;
    }

    
    //需要一个公开函数用来切换技能图标，播放两个技能图图标的动画和切换图标
    public void ChangeSkill()
    {
        animator.Play("ChangeSkill");
    }
    public void ChangeSkillImage()
    {
        if (player.GetComponent<PlayerShoot>().CanChangeSkillSprite())
        {
            emptySkillImage = Skill1Image.sprite;
            Skill1Image.sprite = Skill2Image.sprite;
            Skill2Image.sprite = emptySkillImage;
            player.GetComponent<PlayerShoot>().ChangeSkill();
        }
        
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
    //需要一个公开的函数用来进入转CD的状态，一个变量记录传入的cd时长，一个timer用来计算遮罩比例
    public void UseSkill(float skillCold)
    {
        skillColdThisTime = skillCold;
        skillColdTimer = skillColdThisTime;
        SkillColdText1.enabled = true;
        SkillColdText2.enabled = true;
    }
    //需要一个在update中常驻的函数用来控制CD遮罩的填充
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
    //需要一个公开函数用来访问血条的变化
    private void healthBarChange()
    {

    }
    //需要一个公开函数用来在血条下方的位置放置量个小图标以标识本次游戏获得的DEBUFF
    private void PlaceDebuffSprite()
    {

    }
    IEnumerator GameStart()
    {
        PlayBGM();
        transform.GetComponent<EnemyBornController>().PauseBornEnemy();
        // playersDebuffs = player.GetComponent<PlayerShoot>().GiveDebuffs(debuffCount);
        playersDebuffs = player.GetComponent<PlayerShoot>().GiveDebuffs(GameController.Instance.GetKroossState());
        if (Random.Range(0 , 100) < neutralBuffProbablity)
        {
            StartDeBuff[] sttDebuffs = playersDebuffs;
            playersDebuffs = new StartDeBuff[playersDebuffs.Length + 1];
            for (int i = sttDebuffs.Length; i > 0; i--)
            {
                playersDebuffs[i] = sttDebuffs[i - 1];
            }
            // playersDebuffs[2] = sttDebuffs[0];
            // playersDebuffs[1] = sttDebuffs[1];
            if(Random.Range(0 , 100) < 50)
            {
                playersDebuffs[0] = StartDeBuff.CantStopShoot;
            }
            else
            {
                playersDebuffs[0] = StartDeBuff.DoubleGun;
            }
        }
        if (Random.Range(0 , 100) < getSkillProbablity)
        {
            StartDeBuff[] sttDebuffs = playersDebuffs;
            playersDebuffs = new StartDeBuff[playersDebuffs.Length + 1];
            for (int i = sttDebuffs.Length; i > 0; i--)
            {
                playersDebuffs[i] = sttDebuffs[i - 1];
            }
            // playersDebuffs[2] = sttDebuffs[0];
            // playersDebuffs[1] = sttDebuffs[1];
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
            while((!info.IsName("ShowBuff") && !info.IsName("ShowBuff1")) || info.normalizedTime > 0.1f)    //前一个条件为如果没有播放两个显示buff动画中任何一个，即正在播放其他动画，则等待；如果当前动画进度大于0.1，则视为没有更正为另一个buff动画，则继续等待；当切换为播放另一个buff动画时，时间归零则结束等待，当从头播放非buff动画时，无法达成第一个条件，即播放两个buff动画中的一个，会继续等待
            {
                info = animator.GetCurrentAnimatorStateInfo(0);
                yield return null;
            }
            yield return StartCoroutine(IsAnimatorEnd());
        }
        
        // transform.GetComponent<EnemyBornController>().ContinueBornEnemy();
        StartTiming();
        transform.GetComponent<EnemyBornController>().BornAllEnemy(BornEnemyWaitTime);
        // transform.GetComponent<EnemyBornController>().GenerateBossAfterTime(GameController.Instance.GetBoss() , 3);
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
    // private void GameStart()             //游戏开始时显示debuff  //改成协程？
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
    // private void FightStart()           //显示倒计时后战斗开始  //改成协程？
    // {
    //     transform.GetComponent<EnemyBornController>().StartBornEnemy();
    // }
    public void GameOver()              //游戏失败时的显示画面
    {
        fightEndVoice.text = "呜....搞砸了....";
        kroosFace.sprite = gameoverImage;
        // transform.GetComponent<EnemyBornController>().StopBornEnemy();
        EndFight();
        transform.GetComponent<EnemyBornController>().EndFight();
        // ArchiveSystem.Instance.GameOver(Mathf.FloorToInt((enemyKillInWaves[0] * 1 + enemyKillInWaves[1] * 1.5f + enemyKillInWaves[2] * 2) * scoreGet * 0.5f) , Mathf.FloorToInt((enemyKillInWaves[0] * 1 + enemyKillInWaves[1] * 1.5f + enemyKillInWaves[2] * 2) * scoreGet));
        ArchiveSystem.Instance.GameOver(Mathf.FloorToInt(GlobleDamageCounter.Instance.GetDamage() * 0.5f) , Mathf.FloorToInt(GlobleDamageCounter.Instance.GetDamage()));
    }
    private void SettlementScreen()      //游戏结束时显示结算界面，开始计算分数
    {
                                        //有一个记录击败敌人总数的数，还有其他加分项
                                        //根据开局debuff数加分
    }
    private void CalculateScore()        //计算分数的画面，也许应该使用协程
    {

    }
    private void RewardUp()
    {
        numOfRewardOfNowWave ++ ;
    }
    public void BornEnemy()             //生成敌人时会调用此函数，并且在UI上显示当前敌人数量？此函数暴露给敌人生成器脚本或者敌人使用
    {
        enemyCount++;                     //敌人生成脚本应该动态平衡，敌人少时增多，敌人多时减少生成，有一个生成概率每帧有一定概率生成
    }
    public void EnemyDeath()            //敌人死亡或离开屏幕时调用，当前场景内敌人计数减一
    {
        enemyCount -- ;
        enemyDeathInNowWave ++ ;
        // enemyKill.text = "本轮击败敌人：" + enemyDeathInNowWave;
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
            enemyKillInWaves[wave - 2] = enemyDeathInNowWave;       //记录上一轮的敌人击杀数
        }
        enemyDeathInNowWave = 0;
        nowWaveTime = 0;
        // enemyKill.text = "本轮击败敌人：" + enemyDeathInNowWave;
        transform.GetComponent<EnemyBornController>().StartBornEnemy();
    }
    public int EnemyNumNow()
    {
        return enemyCount;
    }
    private bool IsLastEnemyAndNoReward()           //每个敌人在销毁前都会调用这个函数用以检测是否为最后一个敌人并且此次没有掉落过奖励
    {
        return true;
    }
    private void EndWaves()              //结束本次波次，如果此次波次掉落奖励为0则最后一个怪物强制掉落一个?清空掉落当前波次数量
    {
        isInWave = false;
        numOfRewardOfNowWave = 0;
        // transform.GetComponent<EnemyBornController>().StopBornEnemy();
        if (nowWave == 3)
        {
            fightEndVoice.text = "我觉得还可以哦！";
            kroosFace.sprite = gameEndImage;
            EndFight();
            transform.GetComponent<EnemyBornController>().EndFight();
            // ArchiveSystem.Instance.GameEnd(Mathf.FloorToInt((enemyKillInWaves[0] * 1 + enemyKillInWaves[1] * 1.5f + enemyKillInWaves[2] * 2) * scoreGet * 0.5f) , Mathf.FloorToInt((enemyKillInWaves[0] * 1 + enemyKillInWaves[1] * 1.5f + enemyKillInWaves[2] * 2) * scoreGet));
            ArchiveSystem.Instance.GameEnd(Mathf.FloorToInt(GlobleDamageCounter.Instance.GetDamage() * 0.5f + 500 * (90f/gameTime)) , Mathf.FloorToInt(GlobleDamageCounter.Instance.GetDamage() + 1000 * 180f/gameTime));
        }
    }
    public float GetSkillCold()
    {
        return skillColdTimer;
    }
    public bool CanChangeSkill()
    {
        info = animator.GetCurrentAnimatorStateInfo(0);
        if(!info.IsName("ChangeSkill") && !(info.IsName("ShowBuff") || info.IsName("ShowBuff1")))   //可以切换技能的前提是当前没有正在切换技能，并且也没有在使用两个显示debuff动画中的任意一个
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
    public void HealthBarBorn()
    {
        StartCoroutine(HealthBarBornIE());
    }
    IEnumerator HealthBarBornIE()
    {
        Color color = healthBar.transform.GetChild(3).GetComponent<Text>().color;
        color.a = 0;
        while(color.a < 1)
        {
            yield return new WaitForFixedUpdate();
            color.a += Time.deltaTime;
            healthBar.transform.GetChild(3).GetComponent<Text>().color = color;
        }
        

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
        SoundManager.Instance.PlayBGM2();
        SoundManager.Instance.ContinueMusicSound();
    }
    public void PausePlayAnimator()
    {
        animator.speed = 0f;
    }
    public void ContinuePlayAnimator()
    {
        animator.speed = 1f;
    }
    
    private void BornAllEnemy()
    {
        transform.GetComponent<EnemyBornController>().BornAllEnemy(BornEnemyWaitTime);
    }
    IEnumerator ShowScore()
    {
        
        float timeOfRollScore = 1;
        string extraString;
        string extraString2;
        if (GameController.Instance.GetKroossState() == 0)
        {
            scoreMut = 1;
        }
        else if (GameController.Instance.GetKroossState() == 1)
        {
            scoreMut = 1.3f;
        }
        else if (GameController.Instance.GetKroossState() == 2)
        {
            scoreMut = 1.5f;
        }
        else
        {
            scoreMut = 1;
        }
        // totleEnemyKillCount = enemyKillInWaves[0] + enemyKillInWaves[1] + enemyKillInWaves[2];
        // totleScore = Mathf.FloorToInt((enemyKillInWaves[0] * 1 + enemyKillInWaves[1] * 1.5f + enemyKillInWaves[2] * 2) * scoreGet);


        missionComplitePersent = 100 * (1 - (GameController.Instance.GetBossInGame().GetComponent<OnHit>().GetHealth() / GameController.Instance.GetBossInGame().GetComponent<OnHit>().GetHealthUplimit()));
        // ArchiveSystem.Instance.GameOver(Mathf.FloorToInt(GlobleDamageCounter.Instance.GetDamage() * 0.5f) , Mathf.FloorToInt(GlobleDamageCounter.Instance.GetDamage()));
        if (missionComplitePersent > 100)
        {
            missionComplitePersent = 100;
        }

        if (GameController.Instance.GetBossInGame().GetComponent<OnHit>().GetHealth() / GameController.Instance.GetBossInGame().GetComponent<OnHit>().GetHealthUplimit() > 0)
        {
            extraString = "%    任务将由专业小队收尾。";
        }
        else
        {
            extraString = "%    克洛丝的工作完成了，但博士还没有。";
        }

        if (GameController.Instance.GetBossInGame().GetComponent<OnHit>().GetHealth() / GameController.Instance.GetBossInGame().GetComponent<OnHit>().GetHealthUplimit() > 0)
        {
            totleScore = Mathf.FloorToInt(GlobleDamageCounter.Instance.GetDamage() * scoreMut);
        }
        else
        {
            totleScore = Mathf.FloorToInt(GlobleDamageCounter.Instance.GetDamage() + 500 * (90f/gameTime) * scoreMut);
        }

        if(missionComplitePersent/100 >= 1 && totleScore - GameController.Instance.GetBossInGame().GetComponent<OnHit>().GetHealthUplimit() > 500)
        {
            extraString2 = "    S！  迅捷又快速，好用又强力！这就是克洛丝给我的自信！";
        }
        else if (missionComplitePersent/100 >= 1)
        {
            extraString2 = "    A    克洛丝圆满完成了任务！";
        }
        else if(missionComplitePersent/100 > 0.75f)
        {
            extraString2 = "    B    只要到达那个地方……";
        }
        else if(missionComplitePersent/100 > 0.4f)
        {
            extraString2 = "    C    也许，克洛丝还能加加油？";
        }
        else if(missionComplitePersent/100 > 0.2f)
        {
            extraString2 = "    D    博士，又在打电动噢，休息一下好不好？";
        }
        else
        {
            extraString2 = "    D    也许半夜吃泡面真的有害于健康，博士。";
        }

        totleEnemyKillCountText.text = 0.ToString();
        totleScoreText.text = 0.ToString();
        while(timeOfRollScore > 0)
        {
            totleEnemyKillCountText.text = Random.Range(0 , 999).ToString();
            timeOfRollScore -= Time.deltaTime;
            yield return null;
        }
        // totleEnemyKillCountText.text = totleEnemyKillCount.ToString() + extraString;
        totleEnemyKillCountText.text = missionComplitePersent.ToString("F2") + extraString;
        yield return new WaitForSeconds(1);
        timeOfRollScore = 1;
        while(timeOfRollScore > 0)
        {
            totleScoreText.text = Random.Range(0 , 999).ToString();
            timeOfRollScore -= Time.deltaTime;
            yield return null;
        }
        // totleScoreText.text = totleScore.ToString();
        totleScoreText.text = totleScore.ToString() + extraString2;
        isEndShow = true;
        
    }
    public float GetScoreGet()
    {
        return scoreGet;
    }
    public float GetWave()
    {
        return nowWave;
    }
    public void ReturnMainMenu()
    {
        if (isEndShow)
        {
            StartCoroutine(LoadScene());
        }
    }
    IEnumerator LoadScene()
    {
        loadScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(0);
        operation.allowSceneActivation = false;
        SoundManager.Instance.TurnDownMusic();
        while (!operation.isDone)
        {
            loadSlider.value = operation.progress;
            if(operation.progress < 0.9f)
            {
                loadingText.text = (operation.progress * 100).ToString("f0") + "%";
            }
            else
            {
                loadSlider.value = 1;
                loadingText.text = "请按任意键继续~";
                if(Input.anyKeyDown)
                {
                    SoundManager.Instance.TurnUpMusic();
                    operation.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }
    public GameObject GetHealthBar()
    {
        return BossHealthBar;
    }
    public void End()
    {
        ArchiveSystem.Instance.GameEnd(Mathf.FloorToInt(GlobleDamageCounter.Instance.GetDamage() + 500 * (90f/gameTime) * scoreMut) , Mathf.FloorToInt(GlobleDamageCounter.Instance.GetDamage() + 500 * (90f/gameTime) * scoreMut));
        StartCoroutine(GameEnd());
    }
    IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(2);
        
        EndFight();
        transform.GetComponent<EnemyBornController>().EndFight();
    }
}

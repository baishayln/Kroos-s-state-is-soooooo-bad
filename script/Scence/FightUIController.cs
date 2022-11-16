using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private float enemyBornTimer;
    [SerializeField]public float enemyBornColdTime = 0.25f;
    private int enemyBornMaxIntervalCount = 4;
    private int enemyBornIntervalCount;
    private int enemyBornMinIntervalCount = 2;
    private int enemytIsNotBornIntervalCount;
    // Start is called before the first frame update
    void Start()
    {
        Skill1Image = transform.GetChild(0).GetComponent<Image>();
        Skill2Image = transform.GetChild(1).GetComponent<Image>();
        SkillCold1 = Skill1Image.transform.GetChild(0).GetComponent<Image>();
        SkillCold2 = Skill2Image.transform.GetChild(0).GetComponent<Image>();
        SkillColdText1 = Skill1Image.transform.GetChild(1).GetComponent<Text>();
        SkillColdText2 = Skill2Image.transform.GetChild(1).GetComponent<Text>();
        buffName = transform.GetChild(2).GetChild(0).GetComponent<Text>();
        buffDescribe = transform.GetChild(2).GetChild(1).GetComponent<Text>();
        animator = transform.GetComponent<Animator>();
        startInfo = animator.GetCurrentAnimatorStateInfo(0);

        player = GameObject.Find("Player");

        startDebuffTypeDir.Add(StartDeBuff.AttackDown , "神经退行！");
        startDebuffDescribeDir.Add(StartDeBuff.AttackDown , "克洛丝与凯尔希一起潜入深海营救沉迷烤海嗣的博士，却在行动过程中被不幸投出了1的屑博士献祭了，人神共愤！/n克洛丝的攻击被降低了，效果拔群！");
        startDebuffColor.Add(StartDeBuff.AttackDown , new Color(196f/255 , 39f/255 , 212f/255 , 255f/255));//紫色

        startDebuffTypeDir.Add(StartDeBuff.IntervalDown , "睡眠不足！");
        startDebuffDescribeDir.Add(StartDeBuff.IntervalDown , "昨晚克洛丝和博士被博士拖着在办公室玩游戏玩了个爽，今天早上还要参加行动，这让本就不足的睡眠雪上加霜！/n克洛丝，你为什么只是看着！/n攻击间隔增大！");
        startDebuffColor.Add(StartDeBuff.IntervalDown , new Color(154f/255 , 46f/255 , 81f/255 , 255f/255));//粉色

        startDebuffTypeDir.Add(StartDeBuff.ScatterDown , "神经综合征！");
        startDebuffDescribeDir.Add(StartDeBuff.ScatterDown , "“我到河北省来，I like河北省的书呆子……好棒，好棒的……！”昨晚罗德岛有凶恶博士出没压榨员工，克洛丝今天的手有点哆哆嗦嗦的！/n什么嘛……我射的还是蛮准的吗……/n攻击散射增大！");
        startDebuffColor.Add(StartDeBuff.ScatterDown , Color.black);//黑色

        startDebuffTypeDir.Add(StartDeBuff.RangeDown , "武器校准过度！");
        startDebuffDescribeDir.Add(StartDeBuff.RangeDown , "“我的、我的王之力啊！！！！！”——可露希尔在校准武器时由于操作不规范而大喊道。/n发生这种事情大家都不想的~武器射程下降！");
        startDebuffColor.Add(StartDeBuff.RangeDown , Color.yellow);//黄色
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
        if(isPreparationStart && !isFightStart)
        {
            gameStartTimer += Time.deltaTime;
            // Debug.Log(gameStartTimer);
            if(gameStartTimer > startTime)
            {
                isFightStart = true;
                gameStartTimer = 0;
                FightStart();
                // Debug.Log("Fight Start");
            }
        }
        // BornEnemy();
    }

    void FixedUpdate()
    {
        if(skillColdTimer >= 0)
        {
            SkillColdDown();
        }
    }

    //需要一个公开函数用来切换技能图标，播放两个技能图图标的动画和切换图标
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
        playersDebuffs = player.GetComponent<PlayerShoot>().GiveDebuffs(2);
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
    private void FightStart()           //显示倒计时后战斗开始  //改成协程？
    {

    }
    private void GameOver()              //游戏失败时的显示画面
    {
        
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
    }
    public int EnemyNumNow()
    {
        return enemyCount;
    }
    private bool IsLastEnemyAndNoReward()           //每个敌人在销毁前都会调用这个函数用以检测是否为最后一个敌人并且此次没有掉落过奖励
    {
        return true;
    }
    private void EndWaves()              //结束本次波次，如果此次波次掉落奖励为0则最后一个怪物强制掉落一个，清空掉落当前波次数量
    {
        enemyCount = 0;
        numOfRewardOfNowWave = 0;
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
}

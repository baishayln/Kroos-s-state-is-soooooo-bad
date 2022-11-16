using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StartDeBuff
{
    AttackDown = 0 , IntervalDown = 1 , RangeDown = 2 , ScatterDown = 3 , NoDebuff = 4
}
public class PlayerShoot : MonoBehaviour
{
    [SerializeField]public float health = 50;
    [SerializeField]public float healthUpLimit = 50;
    [SerializeField]public float attack = 10;       //玩家的攻击力
    private int numOfAttack;
    [SerializeField]public GameObject firePoint;
    private Vector2 firePointDirection;
    private Vector2 firePointPosition;
    [SerializeField]public GameObject bulletPrefab;
    private Vector3 gunDirction;
    private Rigidbody2D rig;
    [SerializeField]public float bulletSpeed;
    private float shootTimer;
    private PlayerSkill now1stSkill = null;  //改成一个技能类？和状态类类似？是不是太麻烦了？
    private PlayerSkill now2ndSkill = null;
    private float PlayerSkillColdtime;
    private float SkillColdtimer;       //玩家的技能CD根据上一个释放技能会进入不同的CD
    private Vector3 mousePoint;
    private Vector3 mousePositionOnScreen;//鼠标在屏幕的坐标
    private Vector3 mousePositionInWorld;//把鼠标的屏幕坐标转换为世界坐标
    [SerializeField]public float originSpeedDistance = 1;   //玩家的攻击范围
    [SerializeField]public float originSpeedDistanceChange = 1.5f;   //玩家的攻击范围增益变化值
    [SerializeField]public float originSpeedDistanceUpLimit = 10;   //玩家的攻击范围上限
    [SerializeField]public float bulletAcceleration = 3f;
    [SerializeField]public float minxSpeedX = 6f;
    [SerializeField]public float shootColdTime = 0.35f;     //玩家的射击间隔
    [SerializeField]public float shootDeviation = 30;     //玩家的散射角度（单位：度）
    private Dictionary<PlayerEnhancementType , PlayerSkill> SkillList = new Dictionary<PlayerEnhancementType, PlayerSkill>();
    [SerializeField]public GameObject fishRocket;
    [SerializeField]public GameObject shinbazu;
    [SerializeField]public GameObject gavialsStaff;
    [SerializeField]public GameObject skillUI1;
    [SerializeField]public GameObject skillUI2;
    private GameObject fightUI;
    [SerializeField]public GameObject Kaminobazu;               //异客支援所使用的物体
    [SerializeField]public GameObject realMedicalSupplies;      //嘉维尔支援所使用的物体

    
    
    
    
    // private UI
    //      把技能读条显示在游戏界面上
    //      背景用什么颜色？还是用图片？
    //      玩家拥有射击间隔，一般点不到这么快就行，但要有
    //      玩家可以获得强化？（通过鼠标拾取）两种？一种弹幕数量？一种是环绕物？（无人机）麦哲伦？
    //      克洛丝有万分之一的概率可以秒杀任何敌人
    //      克洛丝可以升级攻击力、射程和射速
    //      音量可调整
    //      血条显示
    //      友军系统？
    //      子弹尾迹以及素材、音效
    //      对象池
    // Start is called before the first frame update
    void Start()
    {
        rig = gameObject.GetComponent<Rigidbody2D>();
        firePoint = transform.GetChild(0).gameObject;
        SkillList.Add(PlayerEnhancementType.skill1 , new MagellansFishBullet(gameObject));
        SkillList.Add(PlayerEnhancementType.skill2 , new Kaminobazu(gameObject));
        SkillList.Add(PlayerEnhancementType.skill3 , new GavialsAssistance(gameObject));
        fightUI = GameObject.Find("FightUI");
        now1stSkill = SkillList[PlayerEnhancementType.skill3];
        now2ndSkill = SkillList[PlayerEnhancementType.skill2];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            // if(fightUI.GetComponent<FightUIController>().GetSkillCold() <= 0 && now1stSkill != null)
                ReleaseSkills();
        }
        if(Input.GetKeyDown(KeyCode.Mouse0) && shootTimer <= 0)
        {
            Shoot();
            shootTimer = shootColdTime;
        }
        shootTimer -= Time.deltaTime;
        GetMousePoint();
        MoveFirePoint();
        // if(Input.GetKeyDown(KeyCode.R))              //改变技能的函数改为由UI调用，此处取消
        // {
        //     if(fightUI.GetComponent<FightUIController>().CanChangeSkill())
        //     {
        //         ChangeSkill();
        //     }
        // }
    }
    private void ReleaseSkills()
    {
        fightUI.GetComponent<FightUIController>().UseSkill(now1stSkill.ReleaseSkill(mousePositionInWorld));
    }
    private void AttackChange(float changeNum)
    {
        attack += changeNum;
    }
    private void NumOfAttackChange(float changeNum)
    {

    }
    private void ShootColdTimeChange(float changeNum)
    {
        shootColdTime -= changeNum;
    }
    public void GetAttackEnhancement()
    {
        if(attack < 70)
            AttackChange(10);
        else
        {
            if(health < 40)
            {
                health += 10;
            }
            else
            {
                health = 50;
            }
        }
    }
    public void GetIntervalEnhancement()
    {
        if(shootColdTime < 0.15f)
            ShootColdTimeChange(0.05f);
        else
        {
            if(health < 40)
            {
                health += 10;
            }
            else
            {
                health = 50;
            }
        }
    }
    public void GetRangeEnhancementt()
    {
        if(originSpeedDistance < originSpeedDistanceUpLimit)
            originSpeedDistance += originSpeedDistanceChange;
        else
        {
            if(health < 40)
            {
                health += 10;
            }
            else
            {
                health = 50;
            }
        }
    }
    public void Getskill1()
    {
        LoadSkill(1);
    }
    public void Getskill2()
    {
        LoadSkill(2);
    }
    public void Getskill3()
    {
        LoadSkill(3);
    }
    private void LoadSkill(int skill)
    {
        if(skill == 1)
        {
            now1stSkill = SkillList[PlayerEnhancementType.skill1];
        }
        if(skill == 2)
        {
            now1stSkill = SkillList[PlayerEnhancementType.skill2];
        }
        if(skill == 3)
        {
            now1stSkill = SkillList[PlayerEnhancementType.skill3];
        }
    }
    public void ChangeSkill()
    {
        if(now1stSkill != null && now2ndSkill != null)
        {
            PlayerSkill emptySkill;
            emptySkill = now1stSkill;
            now1stSkill = now2ndSkill;
            now2ndSkill = emptySkill;
        }
        //播放声音，从游戏里录？
    }
    public void GetScatterEnhancement()
    {
        if(shootDeviation >= 15)
        {
            shootDeviation -= 15;
        }
        else
        {
            if(health < 40)
            {
                health += 10;
            }
            else
            {
                health = 50;
            }
        }
    }
    public void GetDebuff(StartDeBuff type)
    {
        switch (type)
        {
            case StartDeBuff.AttackDown:
                attack -= 5f;
                break;
            case StartDeBuff.IntervalDown:
                shootColdTime += 0.05f;
                break;
            case StartDeBuff.ScatterDown:
                shootDeviation += 15f;
                break;
            case StartDeBuff.RangeDown:
                originSpeedDistance = 0f;
                break;
            default:
                break;
        }
    }
    private void Shoot()
    {
        // GameObject bullet = GameObject.Instantiate(bulletPrefab , firePoint.transform.position , Quaternion.identity);
        GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
        bullet.transform.position = transform.position;
        if(shootDeviation > 0.05f || shootDeviation < -0.05f)
        {
            bullet.transform.right = firePointDirection.normalized;
            bullet.transform.Rotate(0 , 0 , Random.Range(-shootDeviation , shootDeviation) , Space.Self);
            firePointDirection = bullet.transform.right;
        }
        bullet.GetComponent<PlayerBullet>().SetBullet(attack , bulletSpeed , firePointDirection.normalized , transform , originSpeedDistance , bulletAcceleration , minxSpeedX);        //方向需要重新计算
    }
    private void GetMousePoint()
    {
        //获取鼠标在场景中坐标

        mousePositionOnScreen = Input.mousePosition;

        //将相机中的坐标转化为世界坐标

        mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);
    }
    private void MoveFirePoint()
    {
        firePointDirection.x = mousePositionInWorld.x - transform.position.x;
        firePointDirection.y = mousePositionInWorld.y - transform.position.y;
        firePointPosition.x = firePointDirection.normalized.x + transform.position.x;
        firePointPosition.y = firePointDirection.normalized.y + transform.position.y;
        firePoint.transform.position = firePointPosition;
    }
    public StartDeBuff GiveDebuff()
    {
        StartDeBuff debuff = StartDeBuff.NoDebuff;
        switch (Random.Range(0 , 4))
        {
            case 0:
                debuff = StartDeBuff.AttackDown;
                break;
            case 1:
                debuff = StartDeBuff.IntervalDown;
                break;
            case 2:
                debuff = StartDeBuff.RangeDown;
                break;
            case 3:
                debuff = StartDeBuff.ScatterDown;
                break;
            default:
                break;
        }
        return debuff;
    }
    public StartDeBuff[] GiveDebuffs(int debuffNum)
    {
        int GotDbuff = -1;
        StartDeBuff[] debuffs = new StartDeBuff[2];
        for(int i = 0; i < debuffNum ; i++)
        {
            switch (Random.Range(0 , 4))
            {
                case 0:
                    if(GotDbuff == 0)
                    {
                        i--;
                        break;
                    }
                    debuffs[i] = StartDeBuff.AttackDown;
                    GotDbuff = 0;
                    break;
                case 1:
                    if(GotDbuff == 1)
                    {
                        i--;
                        break;
                    }
                    debuffs[i] = StartDeBuff.IntervalDown;
                    GotDbuff = 1;
                    break;
                case 2:
                    if(GotDbuff == 2)
                    {
                        i--;
                        break;
                    }
                    debuffs[i] = StartDeBuff.RangeDown;
                    GotDbuff = 2;
                    break;
                case 3:
                    if(GotDbuff == 3)
                    {
                        i--;
                        break;
                    }
                    debuffs[i] = StartDeBuff.ScatterDown;
                    GotDbuff = 3;
                    break;
                default:
                    break;
            }
        }
        return debuffs;
    }
    public void RestoreToFullHP()
    {
        health = healthUpLimit;
    }
    public bool isFullOfHealth()
    {
        if(health == healthUpLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public float RestoreHP(float restoreNum)
    {
        if(health + restoreNum > healthUpLimit)
        {
            health = healthUpLimit;
        }
        else
        {
            health += restoreNum;
        }
        return health;
    }
}

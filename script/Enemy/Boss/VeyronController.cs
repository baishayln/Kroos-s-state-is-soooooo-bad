using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum VeyronStateType
{
    // IdleAndMove, Run, Jump, Chase, React, Attack, Hit, Death, Dash ,Hited ,Break ,DashPrepare ,DashAttack
    MoveShoot , Move , MoveToPoint , LeaveScreen , MoveStrafe , Switch , MoveStrafePrepare , StandingStrafe , StandingStrafePrepare , 
    RangeStrafe , RangeStrafePrepara , MissileAttack , MissileAttackPrepare , HalfHealth , Bomb , Dead
}

public class VeyronController : MonoBehaviour , OnHit , BossBorn
{
    private float healthUplimit = 1000;
    private float health;
    [SerializeField]private string nameInGame = "";
    private Vector2 speed = Vector2.zero;
    private float restoreAngularVelocity = 60;
    private Vector3 nowEuler;
    private float eulerUplimit = 30;
    private float speedX;
    public Rigidbody2D rig;
    private Transform movePoint;
    private State currentState;
    private VeyronStateType currentStateName;
    private VeyronStateType nextStateName;
    private int lastStateNum = 0;
    private Dictionary<VeyronStateType, State> states = new Dictionary<VeyronStateType, State>();
    [SerializeField]public GameObject aimLinePrefab;
    private GameObject aimLine;
    private LineRenderer line;
    [SerializeField]public Transform shootPoint;
    [SerializeField]public Transform gunPoint;
    [SerializeField]public Transform gunCenterPoint;
    public float gunLenght;
    private Vector3 startShootPoint;
    public bool isSecondPhase = false;
    [SerializeField]public float moveStrafeSpeed = 10;
    public Transform player;
    public Vector3 moveTargetPoint = Vector3.one;
    [SerializeField]public GameObject attackAreaPrefab;
    [SerializeField]public GameObject missilePrefab;
    [SerializeField]public GameObject bizelMissilePrefab;
    [SerializeField]public GameObject scanPre;
    [SerializeField]public GameObject CGAreaPrefab;
    [SerializeField]public GameObject aimFramePre;
    [SerializeField]public GameObject BulletTracerPre;
    [SerializeField]public GameObject bulletPre;
    [SerializeField]public GameObject smallExplosionPre;
    [SerializeField]public GameObject explosionPre;
    [SerializeField]public GameObject crashPre;
    [SerializeField]public AudioClip explosionSound;
    private GameObject cam;
    [SerializeField]public float shootRotateAngle = 3.5f;
    private float bornX;
    private float bornYRange;
    private Vector2 bornPoint;
    // private Vector2 bornPoint1;
    private Vector2 realBornPoint;
    private float lastX;
    private float speedInFullRotateSelf = 10;
    private GameObject healthBar;
    private bool isDead;
    [SerializeField]public ParticleSystem smokeParticleEffects;
    [SerializeField]public ParticleSystem halfHealthParticleEffects;
    [SerializeField]public AudioClip halfHealthSound;
    [SerializeField]public AudioClip deadExplosionSound;
    [SerializeField]public AudioClip missileExplosionSound;
    [SerializeField]public AudioClip scanSound;
    [SerializeField]public AudioClip[] shootEffect;
    public float scale;
    void Start()
    {
        player = GameObject.Find("Player").transform;
        rig = transform.GetComponent<Rigidbody2D>();
        if (!shootPoint)
        {
            shootPoint = transform.GetChild(0);
        }
        startShootPoint = shootPoint.position - transform.position;
        if (!gunPoint)
        {
            gunPoint = transform.GetChild(1);
        }
        if (!gunCenterPoint)
        {
            gunCenterPoint = transform.GetChild(3);
        }
        gunLenght = Vector2.Distance(shootPoint.position , gunCenterPoint.position);
        movePoint = transform.GetChild(2);
        states.Add(VeyronStateType.Move, new VeyronMovestateact(this));
        states.Add(VeyronStateType.MoveToPoint, new VeyronMoveToPointstateact(this));
        states.Add(VeyronStateType.LeaveScreen, new VeyronLeaveScreenstateact(this));
        states.Add(VeyronStateType.MoveShoot, new VeyronMoveShootstateact(this));
        states.Add(VeyronStateType.MoveStrafe, new VeyronMoveStrafestateact(this));
        states.Add(VeyronStateType.MoveStrafePrepare, new VeyronMoveStrafePreparestateact(this));
        states.Add(VeyronStateType.StandingStrafe, new VeyronStandingStrafestateact(this));
        states.Add(VeyronStateType.StandingStrafePrepare, new VeyronStandingStrafePreparestateact(this));
        states.Add(VeyronStateType.RangeStrafe, new VeyronRangeStrafestateact(this));
        states.Add(VeyronStateType.MissileAttack, new VeyronMissileAttackstateact(this));
        states.Add(VeyronStateType.MissileAttackPrepare, new VeyronMissileAttackPreparestateact(this));
        states.Add(VeyronStateType.HalfHealth, new HalfHealthstateact(this));
        states.Add(VeyronStateType.Bomb, new Bombstateact(this));
        states.Add(VeyronStateType.Switch, new Switchstateact(this));
        states.Add(VeyronStateType.Dead, new Deadstateact(this));
        scale = transform.localScale.x;
    }
    void OnEnable()
    {
        health = healthUplimit;
        smokeParticleEffects.Stop();
        isSecondPhase = false;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState != null)
        {
            currentState.OnUpdate();
        }
        else
        {
            StateSwitchingStatus();
        }
        // nowEuler = Vector3.MoveTowards(transform.rotation.eulerAngles , Vector3.zero , restoreAngularVelocity * Time.deltaTime);
        // nowEuler.z = Mathf.Clamp(nowEuler.z , eulerUplimit , -eulerUplimit);
        // transform.rotation = Quaternion.Euler(nowEuler);
        rig.velocity = speed;
        // speed.x = Mathf.MoveTowards(speed.x , 0 , 5 * speed.x * Time.deltaTime);
        // speed.y = Mathf.MoveTowards(speed.y , 0 , 5 * speed.y * Time.deltaTime);
        
        RotateSelf();
    }
    void FixedUpdate()
    {
        if(currentState != null)
        {
            currentState.OnFixedUpdate();
        }
    }
    void RotateSelf()
    {
        if (lastX > transform.position.x)
        {
            transform.localScale = (Vector3.one - 2 * Vector3.right) * scale;
        }
        else if(lastX < transform.position.x)
        {
            transform.localScale = Vector3.one * scale;
        }
        if (lastX > transform.position.x)
        {
            nowEuler.z = eulerUplimit * Mathf.Min(Mathf.Abs(lastX - transform.position.x) , (Time.deltaTime * speedInFullRotateSelf))/(Time.deltaTime * speedInFullRotateSelf);
        }
        if (lastX < transform.position.x)
        {
            nowEuler.z = -1 * eulerUplimit * Mathf.Min(Mathf.Abs(lastX - transform.position.x) , (Time.deltaTime * speedInFullRotateSelf))/(Time.deltaTime * speedInFullRotateSelf);
        }
        // Debug.Log(nowEuler.z);
        // Debug.Log(Time.deltaTime * speedInFullRotateSelf);
        // Debug.Log((transform.position.x - lastX));
        // Debug.Log(Mathf.Min(Mathf.Abs(lastX - transform.position.x) , (Time.deltaTime * speedInFullRotateSelf))/(Time.deltaTime * speedInFullRotateSelf));
        // nowEuler.z = Mathf.Clamp(nowEuler.z , -eulerUplimit , eulerUplimit);
        transform.rotation = Quaternion.Euler(nowEuler);
        lastX = transform.position.x;
    }
    public void StateSwitchingStatus()
    {
        //随机当前阶段可用的所有攻击状态，如果是需要离开屏幕的攻击则会进入上升离开状态，如果是在屏幕内的攻击则会直接移动到指定点位
        //转阶段时会有特殊的轰炸攻击
        // if (currentStateName == VeyronStateType.Move || currentStateName == VeyronStateType.MoveToPoint)
        // {
        //     NextState();
        // }
        // else
        // {

            //  1 , 5
            int randomNum;
            randomNum = Random.Range(1 , 5);
            while(randomNum == lastStateNum)
            {
                randomNum = Random.Range(2 , 4);
            }
            lastStateNum = randomNum;
            switch(randomNum)     
            {
                case 1:
                    TransitionState(VeyronStateType.MoveShoot);
                    break;
                case 2:
                    TransitionState(VeyronStateType.MoveStrafePrepare);
                    break;
                case 3:
                    TransitionState(VeyronStateType.StandingStrafePrepare);
                    break;
                case 4:
                    TransitionState(VeyronStateType.MissileAttackPrepare);
                    break;
                // case 5:
                //     SwitchToAttackMode(VeyronStateType.RangeStrafe);
                //     break;
                default:
                    TransitionState(VeyronStateType.MoveShoot);
                    break;
            }
        // }
        
    }
    public void EndNowState()
    {
        TransitionState(VeyronStateType.Switch);
    }
    public void NextState()
    {
        TransitionState(nextStateName);
        // Debug.Log(nextStateName);
    }
    
    virtual public void PlayShootAudio()
    {
        SoundManager.Instance.PlayVoiceSound(shootEffect[Random.Range(0 , shootEffect.Length)]);
    }

    public void TransitionState(VeyronStateType state)
    {
        if(currentState != null)
        {
            currentState.OnExit();
        }
        currentState = states[state];
        // if(state == xx || state == xx || state == xx || state == xx || state == xx )
        // {
        //      SwitchAttackMode(state);
        // }
        // Debug.Log(currentState);
        currentState.OnEnter();
    }

    // public void SwitchToAttackMode(VeyronStateType state)
    // {
    //     //切换攻击模式，切换所有的攻击状态时都会通过此函数
    //     //切换弹药类型和射击模式时会有换弹生效和一个小的图标或者其他文字提示？
    //     switch(state)
    //     {
    //         case VeyronStateType.MoveShoot:
    //             TransitionState(VeyronStateType.MoveShoot);
    //             break;
    //         case VeyronStateType.MoveStrafe:
    //             TransitionState(VeyronStateType.MoveStrafePrepare);
    //             break;
    //         case VeyronStateType.StandingStrafe:
    //             TransitionState(VeyronStateType.StandingStrafePrepare);
    //             break;
    //         case VeyronStateType.MissileAttack:
    //             TransitionState(VeyronStateType.MissileAttackPrepare);
    //             break;
    //         // case VeyronStateType.RangeStrafe:
    //         //     TransitionState(VeyronStateType.RangeStrafePrepare);
    //         //     break;
    //         default:
    //             TransitionState(VeyronStateType.MoveShoot);
    //             break;
    //     }
    // }
    public void MoveToPoint(Vector3 targetPoint , VeyronStateType nextState)
    {
        moveTargetPoint = targetPoint;
        nextStateName = nextState;
        TransitionState(VeyronStateType.MoveToPoint);
    }
    public void LeaveScreen(Vector3 targetPoint , VeyronStateType nextState)
    {
        nextStateName = nextState;
        TransitionState(VeyronStateType.LeaveScreen);
    }
    public void LeaveScreen(VeyronStateType nextState)
    {
        nextStateName = nextState;
        TransitionState(VeyronStateType.LeaveScreen);
    }

    public LineRenderer GenerateLine()
    {
        aimLine = ObjectPool.Instance.GetObject(aimLinePrefab);
        line = aimLine.GetComponent<LineRenderer>();
        line.enabled = true;
        //这个地方导致的全场只能存在一个瞄准线的bug是怎么产生的？
        return line;
        //生成瞄准线
    }
    public void RemoveLine()
    {
        aimLine = null;
        line = null;
    }

    public void GenerateArea()
    {
        //生成瞄准区域
    }

    public bool OnHit(float damage)
    {
        //如果有护甲加减法或者特殊算法的伤害减免，此处应有一个算法函数用来计算最终真正受到的伤害，在受到伤害后才判断是否进入第二阶段
        
        health -= damage;
        DamageCount(damage);
        if (healthBar)
        {
            healthBar.GetComponent<Slider>().value = health / healthUplimit;
        }
        if(health <= 0)
        {
            Die();
            // DeadFly(speed);
        }
        else if (health < healthUplimit / 2 && !isSecondPhase)
        {
            EnterSecondPhase();
        }
        return health > 0;
    }
    
    public bool OnHit(float damage , float Dir)
    {
        return OnHit(damage);
    }

    public bool OnHit(float damage , Vector2 speed)
    {
        return OnHit(damage);
    }
    public float GetHealth()
    {
        return health;
    }
    public float GetHealthUplimit()
    {
        return healthUplimit;
    }
    public void SetBornPosition(Vector3 ROfenemyBorn)
    {
        //地面敌人需要检测地面位置
        int bornDir;
        if(Random.Range(0 , 2) == 0)
        {
            bornDir = 1;
        }
        else
        {
            bornDir = 1;
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
        // else
        // {
        //     ObjectPool.Instance.PushObject(gameObject);
        //     return false;
        // }
    }
    virtual public void DamageCount(float damage)
    {
        GlobleDamageCounter.Instance.CauseDamage(damage);
    }
    private void EnterSecondPhase()
    {
        isSecondPhase = true;
        smokeParticleEffects.Play();
        Crash();
        SoundManager.Instance.PlayEffectSound(halfHealthSound);
        TransitionState(VeyronStateType.HalfHealth);
        //在进入第二阶段后，会直接进行一个特动：爆炸冒烟因此略微 移动，斜飞离开屏幕，地面轰炸AOE
    }
    public void Crash()
    {
        halfHealthParticleEffects.Play();
        SoundManager.Instance.PlayEffectSound(missileExplosionSound);
    }
    public void ResetShootPoint()
    {
        shootPoint.position = transform.position + startShootPoint.x * transform.right + startShootPoint.y * transform.up;
        //如果之后射击点在战利扫射后发生错位，大概率是此函数出现问题
    }

    protected void Die()
    {
        // EnemyCountDown();
        // isDead = true;
        if (!isDead)
        {
            TransitionState(VeyronStateType.Dead);
            isDead = true;
        }
        // ObjectPool.Instance.PushObject(gameObject);
        // StartCoroutine(DeadExplosion());
    }
    public void Destory()
    {
        GameObject crash = ObjectPool.Instance.GetObject(crashPre);
        crash.transform.position = transform.position;
        crash.GetComponent<ParticleSystem>().Play();
        // if (crashSound)
        // {
        //     SoundManager.Instance.PlayEffectSound(crashSound);
        // }
        currentState = null;
        healthBar.GetComponentInParent<FightUIController>().StopTiming();
        healthBar.GetComponentInParent<FightUIController>().End();
        ObjectPool.Instance.PushObject(gameObject);
    }
    public void Warning()
    {
        
    }
    public void SmallExplosion()
    {
        GameObject explosion = ObjectPool.Instance.GetObject(explosionPre);
        explosion.transform.position = transform.position + Random.Range(-1.5f , 1.5f) * Vector3.right + Random.Range(-1.5f , 1.5f) * Vector3.up;;
        if (deadExplosionSound)
        {
            SoundManager.Instance.PlayEffectSound(deadExplosionSound);
        }
    }
    IEnumerator DeadExplosion()
    {
        GameObject explosion;
        for (int i = 0; i < 15; i++)
        {
            explosion = ObjectPool.Instance.GetObject(smallExplosionPre);
            explosion.transform.position = transform.position + Random.Range(-1.5f , 1.5f) * Vector3.right + Random.Range(-1.5f , 1.5f) * Vector3.up;
            if (explosionSound)
            {
                SoundManager.Instance.PlayEffectSound(explosionSound);
            }
            yield return new WaitForSeconds(0.2f);
        }
        explosion = ObjectPool.Instance.GetObject(explosionPre);
        if (explosionSound)
        {
            SoundManager.Instance.PlayEffectSound(explosionSound);
        }
        explosion.transform.position = transform.position;
        yield return new WaitForSeconds(0.07f);
        Crash();
        healthBar.GetComponentInParent<FightUIController>().End();
        ObjectPool.Instance.PushObject(gameObject);
    }
    public void SpeedChange(float x , float y)
    {
        speed.x += x;
        speed.y += y;
        rig.velocity = speed;
    }
    public void SpeedChange(Vector2 targetSpeed , float changeSpeed)
    {
        speed = Vector2.MoveTowards(speed , targetSpeed , 0.1f);
        rig.velocity = speed;
    }
    public void SetSpeed(Vector2 speed)
    {
        this.speed = speed;
        rig.velocity = speed;
    }
    public Vector2 GetSpeed()
    {
        return speed;
    }
    public void MoveToPoint(float speed , float speedUplimit)
    {
        transform.position = Vector3.MoveTowards(transform.position , movePoint.position , Mathf.Min(speedUplimit, speed * Vector3.Distance(transform.position, movePoint.position)));

    }
    public void MoveToPoint(float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position , movePoint.position , speed);

    }
    public Transform MoveTargetPoint()
    {
        MoveShootPointToTarget.MoveFireGun(gunPoint , shootPoint , gunCenterPoint , gunLenght);
        return movePoint;
    }
    public void ShootPointAimTarget()
    {
        MoveShootPointToTarget.MoveFirePoint(gunPoint , player , shootPoint , gunCenterPoint , gunLenght);
    }
    public void ShootPointAimDir(Vector3 dir)
    {
        MoveShootPointToTarget.MoveFirePoint(gunPoint , dir , shootPoint , gunCenterPoint , gunLenght);
    }
    
    public void SetBossHealthBar(GameObject BossHealthBar)
    {
        healthBar = BossHealthBar;
        healthBar.transform.GetChild(3).GetComponent<Text>().text = nameInGame;
    }
}

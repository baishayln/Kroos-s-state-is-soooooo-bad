using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeyronBossBehaviour : State
{
    public void OnEnter()
    {

    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {

    }
}
public class Veyronemptystateact : State
{
    public void OnEnter()
    {

    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {

    }
}

public class VeyronMovestateact : State
{

    private VeyronController veyron;
    private Vector3 dirction;
    private float speed;
    private float time;
    private float timer;
    [SerializeField] private float accelerationTime = 0.5f;
    private bool isSecondPhase;
    public VeyronMovestateact(VeyronController veyron)
    {
        this.veyron = veyron;
    }
    public void SetDate(float speed, float time, Vector3 dir)
    {
        this.speed = speed;
        this.time = time;
        dirction = dir;
    }
    public void OnEnter()
    {
        timer = 0;
        isSecondPhase = veyron.isSecondPhase;
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer < time)
        {
            veyron.transform.position += speed * dirction * Time.deltaTime * (Mathf.Min(accelerationTime, accelerationTime * (timer / accelerationTime)));
        }
        else
        {
            veyron.EndNowState();
        }
        // Vector3.MoveTowards(veyron.transform.position , targetPoint , speed * (Mathf.Min(Vector3.Distance(veyron.transform.position , targetPoint) , speedUplimit / 4) / (speedUplimit / 4)));
    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {
        timer = 0;
    }

}
public class VeyronMoveToPointstateact : State
{

    private VeyronController veyron;
    private Vector3 targetPoint;
    private float speedUplimit = 15;
    private float speed = 0;
    private float timer;
    private bool startMove = false;
    private bool isSecondPhase;
    public VeyronMoveToPointstateact(VeyronController veyron)
    {
        this.veyron = veyron;
        targetPoint = veyron.moveTargetPoint;
    }
    public void SetTargetPoint(Vector3 point)
    {
        targetPoint = point;
        startMove = true;
    }
    public void OnEnter()
    {
        speed = 0;
        timer = 0;
        targetPoint = veyron.moveTargetPoint;
        isSecondPhase = veyron.isSecondPhase;
    }

    public void OnUpdate()
    {
        speed = Mathf.MoveTowards(speed, speedUplimit, Time.deltaTime * speedUplimit);
        veyron.transform.position = Vector3.MoveTowards(veyron.transform.position, targetPoint, Time.deltaTime * speed * (Mathf.Min(Vector3.Distance(veyron.transform.position, targetPoint), speedUplimit / 4) / (speedUplimit / 4)));

        if (Vector3.Distance(veyron.transform.position, targetPoint) < 0.1f)
        {
            veyron.NextState();
        }
    }

    public void OnFixedUpdate()
    {

    }


    public void OnExit()
    {
        speed = 0;
        timer = 0;
        startMove = false;
        targetPoint = Vector3.up * 20;
    }

    public void SetPoint(Vector3 point)
    {
        targetPoint = point;
    }

}

public class VeyronLeaveScreenstateact : State
{
    private int moveDir;
    private VeyronController veyron;
    private float speedY;
    private float speedX;
    private float timer;
    private float speed;
    private float risingTime = 1f;
    private bool isSecondPhase;

    public VeyronLeaveScreenstateact(VeyronController veyron)
    {
        this.veyron = veyron;
    }

    public void OnEnter()
    {
        if (CameraBehaviour.Instance.ReturnCameraPosition().x > veyron.transform.position.x)
        {
            moveDir = -1;
        }
        else
        {
            moveDir = 1;
        }
        speedX = 0;
        speedY = 0;
        timer = 0;
        speed = Random.Range(1f, 2f);
        isSecondPhase = veyron.isSecondPhase;
    }

    public void OnUpdate()
    {
        speedX += Time.deltaTime * 3;
        speedY = speedX * speedX;
        veyron.SpeedChange(moveDir * speedX * speed, speedY);
        timer += Time.deltaTime;
        if (timer > risingTime || Vector3.Distance(veyron.transform.position, CameraBehaviour.Instance.ReturnBornPosition()) > 2 * CameraBehaviour.Instance.ReturnCameraX())
        {
            // veyron.EndNowState();
            veyron.NextState();
        }
    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {
        veyron.SetSpeed(Vector2.zero);
    }
}

public class VeyronMoveShootstateact : State
{
    private float shootTimer;
    [SerializeField] private float secondPhaseShootInterval = 0.6f;
    [SerializeField] private float secondPhaseAimTime = 0.2f;
    [SerializeField] private float firstPhaseShootInterval = 1f;
    [SerializeField] private float firstPhaseAimTime = 0.2f;
    private bool isAiming = false;
    private float stateTimer;
    private float stateTime = 10;
    private float moveSpeed;
    private float moveDir;
    private int shootCount = 0;
    private LineRenderer line;
    private VeyronController veyron;
    private RaycastHit2D aimResult;
    private Transform target;
    private Vector3 shootDir;
    private float aimDistense = 60;
    private bool isSecondPhase;
    private Vector3 shootAimPoint;
    private float lineTwinkleTimer;
    [SerializeField] private float lineTwinkleTime = 0.1f;
    private float screenX;
    private float screenY;
    private Vector2 cameraPoint;
    private Vector3 position;
    [SerializeField] private float positionX = 3;
    [SerializeField] private float positionY = 1;
    private float angle = 0;
    private float angleChangeSpeed = 1;
    private RaycastHit2D shootResult;
    [SerializeField] private float damage = 3;
    [SerializeField] private float speedUplimit = 15;
    [SerializeField] private float speedMutiple = 6;
    [SerializeField]private AudioClip shootSound;
    [SerializeField]private GameObject bulletPrefab;
    public VeyronMoveShootstateact(VeyronController veyron)
    {
        this.veyron = veyron;
        bulletPrefab = veyron.BulletTracerPre;
    }

    public void OnEnter()
    {
        shootTimer = 0;
        shootCount = 0;
        target = GameObject.Find("Player").transform;
        isAiming = false;
        lineTwinkleTimer = 0;
        isSecondPhase = veyron.isSecondPhase;
        screenY = CameraBehaviour.Instance.ReturnCameraY();
        cameraPoint = CameraBehaviour.Instance.ReturnCameraPosition();
        stateTimer = stateTime;
    }

    public void OnUpdate()
    {
        Move();
        if (isSecondPhase)
        {
            SecondPhaseUpdate();
        }
        else
        {
            FirstPhaseUpdate();
        }
        if (stateTimer > 0)
        {
            stateTimer -= Time.deltaTime;
        }
        //如果要达到让玩家在找到规律后加上一定量的操作可以达到无伤，那么就要减少威龙和玩家两点之间的垂直角度带来的影响，即加高威龙在空中的盘旋高度或者减小盘旋范围
    }

    public void OnFixedUpdate()
    {

    }

    private void FirstPhaseUpdate()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= firstPhaseAimTime && !isAiming)
        {
            isAiming = true;
            line = veyron.GenerateLine();
            veyron.RemoveLine();
            line.enabled = true;
        }
        if (isAiming && shootTimer <= firstPhaseShootInterval - (firstPhaseShootInterval - firstPhaseAimTime) / 2)
        {
            if (shootCount % 2 == 1)
            {
                //此处为预判玩家走位
                if (Vector2.Distance(Vector2.zero, target.GetComponent<PlayerMove>().GetSpeed()) > 8)
                {
                    shootAimPoint = target.position + (Vector3)target.GetComponent<PlayerMove>().GetSpeed().normalized * 8 * (firstPhaseShootInterval - firstPhaseAimTime) / 2;
                }
                else
                {
                    shootAimPoint = target.position + (Vector3)target.GetComponent<PlayerMove>().GetSpeed() * (firstPhaseShootInterval - firstPhaseAimTime) / 2;
                }
            }
            else
            {
                shootAimPoint = target.position;
            }
            Aim(veyron.shootPoint.position, shootAimPoint);
            lineTwinkleTimer += Time.deltaTime;
            if (lineTwinkleTimer > lineTwinkleTime)
            {
                lineTwinkleTimer = 0;
                // line.enabled = !line.enabled;
            }
        }
        else if (isAiming)
        {
            Aim(veyron.shootPoint.position, shootAimPoint);
            lineTwinkleTimer += Time.deltaTime;
            if (lineTwinkleTimer > lineTwinkleTime)
            {
                lineTwinkleTimer = 0;
                line.enabled = !line.enabled;
            }
        }
        if (shootTimer >= firstPhaseShootInterval)
        {
            isAiming = false;
            shootTimer = 0;
            Shoot();
        }
    }

    private void SecondPhaseUpdate()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= secondPhaseAimTime && !isAiming)
        {
            isAiming = true;
            line = veyron.GenerateLine();
            line.enabled = true;
            if (shootCount % 2 == 1)
            {
                //此处为预判玩家走位
                // shootAimPoint = target.position + (Vector3)target.GetComponent<PlayerMove>().GetSpeed() * 0.4f;

                if (Vector2.Distance(Vector2.zero, target.GetComponent<PlayerMove>().GetSpeed()) > 8)
                {
                    shootAimPoint = target.position + (Vector3)target.GetComponent<PlayerMove>().GetSpeed().normalized * 8 * (firstPhaseShootInterval - firstPhaseAimTime) / 2;
                }
                else
                {
                    shootAimPoint = target.position + (Vector3)target.GetComponent<PlayerMove>().GetSpeed() * (firstPhaseShootInterval - firstPhaseAimTime) / 2;
                }
            }
            else
            {
                shootAimPoint = target.position;
            }
        }
        if (isAiming)
        {
            Aim(veyron.shootPoint.position, shootAimPoint);
            lineTwinkleTimer += Time.deltaTime;
            if (lineTwinkleTimer > lineTwinkleTime)
            {
                lineTwinkleTimer = 0;
                line.enabled = !line.enabled;
            }
        }
        //瞄准的代码需要在射击前的0.4秒每帧执行吗？
        if (shootTimer >= secondPhaseShootInterval)
        {
            shootTimer = 0;
            isAiming = false;
            Shoot();
        }
    }
    public void Move()
    {
        // if (veyron.transform.position.x < )
        // {

        // }
        angleChangeSpeed = Mathf.Max(0.5f, Mathf.Min(Vector3.Distance(veyron.transform.position, target.transform.position + position) / 1.5f, 1));
        angle += Time.deltaTime * 2.7f * angleChangeSpeed;
        if (angle > 360)
        {
            angle = angle % 360;
        }
        position.x = Mathf.Sin(angle) * positionX;
        position.y = Mathf.Sin(angle * 2) * positionY;
        position += 6 * Vector3.up;
        veyron.transform.position = Vector3.MoveTowards(veyron.transform.position, target.transform.position + position, Mathf.Min(speedUplimit, speedMutiple * Vector3.Distance(veyron.transform.position, target.transform.position + position)) * Time.deltaTime);
    }
    public void OnExit()
    {
        if (line)
        {
            ObjectPool.Instance.PushObject(line.gameObject);
            line = null;
        }
        veyron.SetSpeed(((target.transform.position + position) - veyron.transform.position).normalized * Mathf.Min(speedUplimit, speedMutiple * Vector3.Distance(veyron.transform.position, target.transform.position + position)) * Time.deltaTime);
    }
    private void Aim(Vector3 selfPosition, Vector3 targetPosition)
    {
        //此函数实现的功能是调整瞄准线的位置,使用传入的射击初始点
        aimResult = Physics2D.Raycast(veyron.shootPoint.position, shootDir = targetPosition - selfPosition, aimDistense, LayerMask.GetMask("Ground"));
        line.SetPosition(0, selfPosition);
        if (aimResult.point == Vector2.zero)
        {
            line.SetPosition(1, selfPosition + new Vector3(shootDir.normalized.x * aimDistense, shootDir.normalized.y * aimDistense, selfPosition.z));
        }
        else
        {
            line.SetPosition(1, aimResult.point);
        }
        veyron.ShootPointAimDir(shootDir);
    }
    private void Shoot()
    {
        shootCount++;
        shootResult = Physics2D.Raycast(veyron.shootPoint.position, (shootAimPoint - veyron.shootPoint.position).normalized, aimDistense, LayerMask.GetMask("PlayerBody"));
        if (shootResult.transform)
        {
            shootResult.transform.GetComponent<OnHit>().OnHit(damage, (target.transform.position - veyron.shootPoint.position).normalized.x);
        }
        if (line)
        {
            ObjectPool.Instance.PushObject(line.gameObject);
            line = null;
        }
        if (stateTimer <= 0)
        {
            veyron.EndNowState();
        }
        if (shootSound)
        {
            // SoundManager.Instance.PlayEffectSound(shootSound);
            veyron.PlayShootAudio();
        }
        if (bulletPrefab)
        {
            GameObject bullet = ObjectPool.Instance.GetObject(bulletPrefab);
            LineRenderer tracer = bullet.GetComponent<LineRenderer>();
            tracer.SetPosition(0, veyron.shootPoint.position);
            tracer.SetPosition(1, aimResult.point);
        }
        
    }
}

public class VeyronMoveShootEndstateact : State
{
    private int moveDir;
    private VeyronController veyron;
    private float speedY;
    private float speedX;
    private float timer;
    [SerializeField] private float risingTime = 3;
    private bool isSecondPhase;
    private GameObject Missiles;
    public VeyronMoveShootEndstateact(VeyronController veyron)
    {
        this.veyron = veyron;
    }

    public void OnEnter()
    {
        if (CameraBehaviour.Instance.ReturnCameraPosition().x > veyron.transform.position.x)
        {
            moveDir = -1;
        }
        else
        {
            moveDir = 1;
        }
        speedX = 0;
        speedY = 0;
        timer = 0;
        isSecondPhase = veyron.isSecondPhase;
        GameObject missile = ObjectPool.Instance.GetObject(Missiles);
    }

    public void OnUpdate()
    {
        speedX += Time.deltaTime * 3;
        speedY = Time.deltaTime * Time.deltaTime * 3;
        timer += Time.deltaTime;
        if (timer > risingTime)
        {
            // veyron.EndNowState();
            veyron.NextState();
        }
    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {

    }
}


public class VeyronMoveStrafePreparestateact : State
{
    //此种射击方式存在弹道？比较密集的机枪只扫射一遍？然后转换方向？还是扫射多次需要玩家把我闪避时机？
    //配合玩家的两次闪避充能来扫射两次？
    //如果没有弹道提示则会在枪口有一个充能提示？聚集的红光特效？子弹的拖尾很长很明显？还是直接换成无拖尾但会留下弹道的射击模式？

    private VeyronController veyron;
    public VeyronMoveStrafePreparestateact(VeyronController veyron)
    {
        this.veyron = veyron;
    }
    public void OnEnter()
    {
        veyron.LeaveScreen(VeyronStateType.MoveStrafe);
    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {

    }
    public void OnExit()
    {

    }

}
public class VeyronMoveStrafestateact : State
{
    private VeyronController veyron;
    // private Vector3 dirction = Vector3.right;
    private int dirction;
    private float speed = 10;
    private float moveTime;
    private float moveTimer;
    [SerializeField] private float accelerationTime = 0.5f;
    private bool isSecondPhase;
    private Transform target;
    private float shootInterval = 0.1f;
    private float shootTimer;
    private RaycastHit2D shootResult;
    [SerializeField] private float damage = 3;
    private float cameraX;
    private Transform camera;
    [SerializeField]private AudioClip shootSound;
    private GameObject bulletPre;
    [SerializeField]private float bulletSpeed = 30;
    public VeyronMoveStrafestateact(VeyronController veyron)
    {
        this.veyron = veyron;
        bulletPre = veyron.bulletPre;
    }
    // public void SetDate(float speed , float time , Vector3 dir)
    // {
    //     this.speed = speed;
    //     this.time = time;
    //     dirction = dir;
    // }
    public void OnEnter()
    {
        moveTimer = 0;
        shootTimer = 0;
        target = GameObject.Find("Player").transform;
        camera = GameObject.Find("Main Camera").transform;
        speed = veyron.moveStrafeSpeed;
        cameraX = CameraBehaviour.Instance.ReturnCameraX();
        if (veyron.transform.position.x > CameraBehaviour.Instance.ReturnCameraPosition().x)
        {
            dirction = -1;
        }
        else
        {
            dirction = 1;
        }
        //此上六行等价于将dirction替换为（(target.position.x - veyron.transform.position.x).normalized）
        veyron.transform.position = (Vector2)CameraBehaviour.Instance.ReturnCameraPosition() + CameraBehaviour.Instance.ReturnCameraY() * 0.2f * Vector2.up - dirction * CameraBehaviour.Instance.ReturnCameraX() * Vector2.right * 0.75f;

        isSecondPhase = veyron.isSecondPhase;
    }

    public void OnUpdate()
    {
        if (isSecondPhase)
        {
            SecondPhaseUpdate();
        }
        else
        {
            FirstPhaseUpdate();
        }
        veyron.ShootPointAimDir((Vector3.right * dirction + Vector3.down).normalized);
    }

    public void OnFixedUpdate()
    {

    }

    private void FirstPhaseUpdate()
    {
        moveTimer += Time.deltaTime;
        if (veyron.transform.position.x > camera.position.x - CameraBehaviour.Instance.ReturnCameraX() * 0.75f && dirction < 0)
        {
            // veyron.transform.position += Vector3.right * speed * dirction * Time.deltaTime * (Mathf.Min(accelerationTime, accelerationTime * (moveTimer / accelerationTime)));
            veyron.transform.position += Vector3.right * speed * dirction * Time.deltaTime;
        }
        if (veyron.transform.position.x < camera.position.x + CameraBehaviour.Instance.ReturnCameraX() * 0.75f && dirction > 0)
        {
            veyron.transform.position += Vector3.right * speed * dirction * Time.deltaTime;
        }
        shootTimer += Time.deltaTime;
        if (shootTimer > shootInterval)
        {
            Shoot();
        }
    }
    private void SecondPhaseUpdate()
    {
        moveTimer += Time.deltaTime;
        if (veyron.transform.position.x > camera.position.x - CameraBehaviour.Instance.ReturnCameraX() * 0.75f && dirction < 0)
        {
            veyron.transform.position += Vector3.right * 2 * speed * dirction * Time.deltaTime;
        }
        if (veyron.transform.position.x < camera.position.x + CameraBehaviour.Instance.ReturnCameraX() * 0.75f && dirction > 0)
        {
            veyron.transform.position += Vector3.right * 2 * speed * dirction * Time.deltaTime;
        }
        // if (veyron.transform.position.x < CameraBehaviour.Instance.ReturnCameraPosition().x - CameraBehaviour.Instance.ReturnCameraX() && dirction < 0)
        // {
        //     veyron.transform.position += Vector3.right * 2 * speed * dirction * Time.deltaTime;
        // }
        shootTimer += Time.deltaTime;
        if (shootTimer > shootInterval / 2)
        {
            Shoot();
        }
    }

    public void OnExit()
    {
        moveTimer = 0;
    }
    private void Shoot()
    {
        // shootResult = Physics2D.Raycast(veyron.shootPoint.position, (Vector3.right + Vector3.down).normalized, aimDistense, LayerMask.GetMask("PlayerBody"));
        // if (shootResult.transform)
        // {
        //     shootResult.transform.GetComponent<PlayerMove>().OnHit(damage, (target.transform.position - veyron.shootPoint.position).normalized.x);
        // }
        if (veyron.transform.position.x < camera.position.x - CameraBehaviour.Instance.ReturnCameraX() * 0.75f && dirction < 0)
        {
            veyron.EndNowState();
        }
        if (veyron.transform.position.x > camera.position.x + CameraBehaviour.Instance.ReturnCameraX() * 0.75f && dirction > 0)
        {
            veyron.EndNowState();
        }
        veyron.PlayShootAudio();
        // if (shootSound)
        // {
        //     SoundManager.Instance.PlayEffectSound(shootSound);
        // }

        GameObject bullet = ObjectPool.Instance.GetObject(bulletPre);
        bullet.GetComponent<Bullet>().SetBullet(damage , bulletSpeed , (Vector3.right * dirction + Vector3.down).normalized , veyron.shootPoint.position);
        shootTimer = 0;
        veyron.PlayShootAudio();
    }
}

public class VeyronStandingStrafePreparestateact : State
{
    //此种射击方式存在弹道？比较密集的机枪只扫射一遍？然后转换方向？还是扫射多次需要玩家把我闪避时机？
    //配合玩家的两次闪避充能来扫射两次？
    //如果没有弹道提示则会在枪口有一个充能提示？聚集的红光特效？子弹的拖尾很长很明显？还是直接换成无拖尾但会留下弹道的射击模式？

    private VeyronController veyron;
    public VeyronStandingStrafePreparestateact(VeyronController veyron)
    {
        this.veyron = veyron;
    }
    public void OnEnter()
    {
        if (Random.Range(0, 2) > 0)
        {
            veyron.MoveToPoint((Vector3)((Vector2)CameraBehaviour.Instance.ReturnCameraPosition() + CameraBehaviour.Instance.ReturnCameraX() * 0.3f * Vector2.right + CameraBehaviour.Instance.ReturnCameraY() * 0.1f * Vector2.up), VeyronStateType.StandingStrafe);
        }
        else
        {
            veyron.MoveToPoint((Vector3)((Vector2)CameraBehaviour.Instance.ReturnCameraPosition() - CameraBehaviour.Instance.ReturnCameraX() * 0.3f * Vector2.right + CameraBehaviour.Instance.ReturnCameraY() * 0.1f * Vector2.up), VeyronStateType.StandingStrafe);
        }
    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {

    }
    public void OnExit()
    {

    }

}

public class VeyronStandingStrafestateact : State
{
    //此种射击方式存在弹道？比较密集的机枪只扫射一遍？然后转换方向？还是扫射多次需要玩家把我闪避时机？
    //配合玩家的两次闪避充能来扫射两次？
    //如果没有弹道提示则会在枪口有一个充能提示？聚集的红光特效？子弹的拖尾很长很明显？还是直接换成无拖尾但会留下弹道的射击模式？

    private float shootInterval = 0.1f;
    private float shootTimer;
    private bool isSecondPhase;
    private VeyronController veyron;
    [SerializeField] private int strafeNum = 2;
    private int strafeCount = 0;
    [SerializeField] private int shootNum = 40;
    private int shootCount = 0;
    private float shootRotateAngle;
    private int dir;
    private int rotateDir;
    private float preTime = 2;
    private float preTimer;
    private float endTimer;

    private RaycastHit2D shootResult;
    [SerializeField] private float damage = 3;
    private float aimDistense = 30;
    [SerializeField]private AudioClip shootSound;
    private GameObject bulletPre;
    [SerializeField]private float bulletSpeed = 30;
    private Vector3 veyronScale = Vector3.one;
    public VeyronStandingStrafestateact(VeyronController veyron)
    {
        this.veyron = veyron;
        bulletPre = veyron.bulletPre;
        shootRotateAngle = veyron.shootRotateAngle;
    }
    public void OnEnter()
    {
        isSecondPhase = veyron.isSecondPhase;
        strafeCount = 0;
        preTimer = 0;
        rotateDir = 1;
        endTimer = 0;
        if (veyron.transform.position.x < CameraBehaviour.Instance.ReturnCameraPosition().x)
        {
            dir = 1;
            rotateDir = dir;
        }
        else
        {
            dir = -1;
            rotateDir = dir;
        }
        veyronScale.x = dir;
        ChangeVeyronScale();
        veyron.ShootPointAimDir(Vector3.down + -dir * Vector3.right);
        veyron.Warning();
    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {
        if (isSecondPhase)
        {
            SecondPhaseFixedUpdate();
        }
        else
        {
            FirstPhaseFixedUpdate();
        }
        ChangeVeyronScale();
    }
    public void ChangeVeyronScale()
    {
        veyron.transform.localScale = veyronScale * veyron.scale;
    }
    private void FirstPhaseFixedUpdate()
    {
        if (preTimer < preTime)
        {
            preTimer += Time.deltaTime;
        }
        else
        {
            if (strafeCount < strafeNum)
            {
                if (shootCount < shootNum)
                {
                    Shoot();
                    shootCount++;
                }
                else
                {
                    shootCount = 0;
                    strafeCount++;
                    rotateDir = -rotateDir;
                }
            }
        }
        if (strafeCount >= strafeNum)
        {
            endTimer += Time.deltaTime;
            if (endTimer > 1)
            {
                veyron.EndNowState();
            }
        }
        // shootTimer += Time.deltaTime;
        // if (shootTimer > shootInterval)
        // {
        //     Shoot();
        // }
    }
    private void SecondPhaseFixedUpdate()
    {
        if (preTimer < preTime)
        {
            preTimer += Time.deltaTime;
        }
        else
        {
            if (strafeCount < strafeNum * 2)
            {
                if (shootCount < shootNum)
                {
                    Shoot();
                    shootCount++;
                }
                else
                {
                    shootCount = 0;
                    strafeCount++;
                    rotateDir = -rotateDir;
                }
            }
        }
        if (strafeCount >= strafeNum * 2)
        {
            endTimer += Time.deltaTime;
            if (endTimer > 1)
            {
                veyron.EndNowState();
            }
        }
        // if (preTimer < preTime)
        // {
        //     if (strafeCount < strafeNum * 2)
        //     {
        //         if (shootCount < shootNum)
        //         {
        //             Shoot();
        //             shootCount++;
        //         }
        //         else
        //         {
        //             shootCount = 0;
        //             strafeCount++;
        //         }
        //     }
        // }
        // if (strafeCount >= strafeNum * 2)
        // {
        //     endTimer += Time.deltaTime;
        //     if (endTimer > 1)
        //     {
        //         veyron.EndNowState();
        //     }
        // }



        // shootTimer += Time.deltaTime; 
        // if (shootTimer > shootInterval/2)
        // {
        //     Shoot();
        // }
    }
    private void SetShootAngle()
    {

    }
    private void RotateMuzzle(float angle, int dir)
    {
        // veyron.shootPoint.RotateAround(veyron.gunPoint.position, Vector3.forward, angle * dir);          //奇妙的bug造成了奇妙的射击点旋转效果，旋转外点会随着围绕中心旋转的角度越大而旋转越慢
        veyron.shootPoint.RotateAround(veyron.gunCenterPoint.position, Vector3.forward, angle * dir);
    }

    public void OnExit()
    {
        veyron.ResetShootPoint();
    }
    private void Shoot()
    {
        RotateMuzzle(shootRotateAngle, rotateDir);

        veyron.MoveTargetPoint();

        // shootResult = Physics2D.Raycast(veyron.shootPoint.position, (veyron.shootPoint.position - veyron.gunPoint.position).normalized, aimDistense, LayerMask.GetMask("PlayerBody"));

        // if (shootResult.transform)
        // {
        //     shootResult.transform.GetComponent<PlayerMove>().OnHit(damage, (shootResult.transform.position - veyron.shootPoint.position).normalized.x);
        // }

        if (shootSound)
        {
            // SoundManager.Instance.PlayEffectSound(shootSound);
        }
        veyron.PlayShootAudio();

        GameObject bullet = ObjectPool.Instance.GetObject(bulletPre);
        bullet.GetComponent<Bullet>().SetBullet(damage , bulletSpeed , (veyron.shootPoint.position - veyron.gunPoint.position).normalized , veyron.shootPoint.position);
    }
}

public class VeyronRangeStrafestateact : State
{
    private VeyronController veyron;
    private bool isSecondPhase;
    //此种射击存在一个射击范围表示，在准备阶段会根据玩家位置来调整射击方向，如果射击点较远则射击持续时间较长，弹幕密集？如果向下射击造成的伤害区域很小则会很快停止？
    //向天上射击也会较快停止
    //位置会相对来说低一些？还是范围大一些飞高一些？如果玩家跑远则会扩大此范围和地面的接触面积
    //如果飞得太高并且射击范围的角度太小则会造成打击面过小无法命中玩家和造成限制走位效果，并且持续时间长玩家射不到，两边一起挂机
    //位置降低一些并且向远处地面射击时会给射程短的玩家一个较好的输出机会
    //效果类似泰拉瑞亚火星飞船第一阶段站立不动的小激光散射，或者是神秘链妹红的不死鸟之尾
    //射击结束会有枪口冒烟效果？
    //瞄准前是否会随着玩家的位置选择射击悬停点？
    //调整角度让威龙在处于不太高的位置向较远处射击时能有较大的地面覆盖效果

    public VeyronRangeStrafestateact(VeyronController veyron)
    {
        this.veyron = veyron;
    }
    public void OnEnter()
    {
        isSecondPhase = veyron.isSecondPhase;
    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {

    }
}



public class VeyronMissileAttackPreparestateact : State
{
    private int moveDir;
    private VeyronController veyron;
    private float speedY;
    private float speedX;
    private float timer;
    [SerializeField] private float risingTime = 3;
    private bool isSecondPhase;
    private GameObject Missiles;

    public VeyronMissileAttackPreparestateact(VeyronController veyron)
    {
        this.veyron = veyron;
    }

    public void OnEnter()
    {
        veyron.LeaveScreen(VeyronStateType.MissileAttack);
    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {

    }
    //导弹攻击是威龙自身释放导弹还是引导远处导弹射击？
    //如果是自身发射的导弹则需要在飞离屏幕的时候从身上向像上60度角射出很多带拖尾效果的导弹用来模拟导弹出仓再返回，但实际上这些导弹并不是落地的导弹，向上飞行一段时间后会自行销毁
    //如果是远处导弹射击是否需要在屏幕坐上或者右上角弹出一个类似漫画的小格子来提示导弹来袭？加一个红色感叹号？
}


public class VeyronMissileAttackstateact : State, MissileLauncher
{
    [SerializeField] private float damage = 4;
    private VeyronController veyron;
    private bool isSecondPhase;
    private int attackRound = 2;
    private int attackRoundCount;
    private Transform player;
    private Transform cameraCenterPoint;
    private GameObject attackAreaPrefab;
    private GameObject missilePrefab;
    [SerializeField] private float shootPlayerInterval = 0.8f;
    private float shootTimer;
    private int shootPlayerNum = 6;
    private int shootPlayerCount;
    private int missileCGNum = -1;
    private int missileCGCount;
    private RaycastHit2D aimResults;
    private bool isPrepareDone;
    private int strafeDir;
    private int strafeOrigin;
    private Vector3 cameraPoint;
    [SerializeField] private float strafeDstsInterval = 2.5f;
    [SerializeField] private float missileCGInterval = 0.35f;
    [SerializeField] private float missileSpeed = 50;
    private float startXDsts;
    private float startYDsts;
    private Vector3 nowTargetPoint = Vector3.zero;
    private Vector3 aimArea = new Vector3(0.3f, 30, 1);
    private GameObject scanPre;
    private GameObject aimFramePre;
    private float startTimer;
    [SerializeField]private AudioClip lockSound;
    [SerializeField]private AudioClip explosionSound;

    //瞄准导弹和扫射的导弹是否需要在一个状态中？如果不捆绑分开两个状态那是否需要分成两个攻击进行随机判定？只进行一种？

    public VeyronMissileAttackstateact(VeyronController veyron)
    {
        this.veyron = veyron;
        attackAreaPrefab = veyron.attackAreaPrefab;
        missilePrefab = veyron.missilePrefab;
        explosionSound = veyron.missileExplosionSound;
        lockSound = veyron.scanSound;
    }
    public void OnEnter()
    {
        isSecondPhase = veyron.isSecondPhase;
        player = GameObject.Find("Player").transform;
        shootTimer = 0;
        shootPlayerCount = 0;
        missileCGCount = 0;
        isPrepareDone = false;
        scanPre = veyron.scanPre;
        aimFramePre = veyron.aimFramePre;
        MissileCGprepare();
        startTimer = 2;
        ScanPlayer();
        attackRoundCount = 0;
    }

    public void OnUpdate()
    {
        if (startTimer > 0)
        {
            startTimer -= Time.deltaTime;
        }
        if (startTimer <= 0)
        {
            if (attackRoundCount % 2 == 0)
            {
                ShootOnPlayer();
            }
            else
            {
                MissileClearGround();
            }
        }
        if (attackRoundCount >= attackRound && !isSecondPhase)
        {
            veyron.EndNowState();
        }
        if (attackRoundCount >= 2 * attackRound && isSecondPhase)
        {
            veyron.EndNowState();
        }
    }

    public void OnFixedUpdate()
    {

    }
    private void ScanPlayer()
    {
        GameObject scan = ObjectPool.Instance.GetObject(scanPre);
        SoundManager.Instance.PlayEffectSound(lockSound);
        scan.GetComponent<ScanObj>().SetData("Player", this, CameraBehaviour.Instance.ReturnBornPosition() + CameraBehaviour.Instance.ReturnCameraX() * 0.6f * Vector3.right + CameraBehaviour.Instance.ReturnCameraY() * 0.6f * Vector3.up);
    }
    public void LockPlayer(Transform player)
    {
        GameObject aimFrame = ObjectPool.Instance.GetObject(aimFramePre);
        aimFrame.GetComponent<AimFrame>().SetData(1, player);

        if (lockSound)
        {
            SoundManager.Instance.PlayEffectSound(lockSound);
        }
    }
    private void ShootOnPlayer()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootPlayerInterval)
        {
            shootTimer = 0;
            shootPlayerCount++;
            PrepareOneLaunch(player.position);
            if (shootPlayerCount >= shootPlayerNum)
            {
                attackRoundCount++;
                shootPlayerCount = 0;
                shootTimer = 0;
            }
        }
    }

    private void MissileClearGround()
    {
        // if (!isPrepareDone)
        // {
        //     MissileCGprepare();
        // }
        shootTimer += Time.deltaTime;
        if (shootTimer >= missileCGInterval)
        {
            shootTimer = 0;
            PrepareOneLaunch(nowTargetPoint);
            nowTargetPoint.x += strafeDstsInterval * strafeDir;
            //(此时X - 应到X) * 射击点移动方向 > 0(或者是屏幕外的一定距离)
            //(此时X - (-1 * (cameraPoint.x + strafeOrigin * startXDsts)（起始点相对于屏幕中心的反位置） )) * 射击点移动方向 > 0(或者是屏幕外的一定距离)
            //此算式可在初始点相反的两种情况下使用固定参数得出相同的结果，结果在当射击点无限向着射击点移动方向移动的过程中会从负二倍的屏幕X半轴趋近于无穷大，故只需大于0或大于大于0的一定值即落点在屏幕外一定距离即可取消导弹洗地
            //当移动到屏幕中心点的另一端X半轴，也就是射击点穿过了整个屏幕后上述算式的结果会趋近于0
            if ((nowTargetPoint.x - (-1 * (cameraPoint.x + strafeOrigin * startXDsts))) * strafeDir > 3)
            {
                attackRoundCount++;
                shootPlayerCount = 0;
                shootTimer = 0;
                MissileCGprepare();
            }
        }
        // if (attackRoundCount >= attackRound)
        // {
        //     veyron.EndNowState();
        // }
    }
    private void MissileCGprepare()
    {
        //直接获得屏幕中心位置然后向左或者向右找一个位置然后直接每次递增一定X平铺过去？
        cameraPoint = CameraBehaviour.Instance.ReturnCameraPosition();
        // startXDsts = cameraPoint.x - (CameraBehaviour.Instance.ReturnCameraX()/2 - Random.Range(0.5f , 1.5f));
        startXDsts = CameraBehaviour.Instance.ReturnCameraX() / 2 + Random.Range(0.5f, 1.5f);
        startYDsts = CameraBehaviour.Instance.ReturnCameraY();
        if (Random.Range(0, 2) > 0)
        {
            strafeOrigin = 1;
            strafeDir = -1;
        }
        else
        {
            strafeOrigin = -1;
            strafeDir = 1;
        }
        nowTargetPoint = CameraBehaviour.Instance.ReturnBornPosition();
        nowTargetPoint.x = cameraPoint.x + strafeOrigin * startXDsts;
        // if (startXDsts < cameraPoint.x)
        // {
        //     strafeDir = 1;
        // }
        // else
        // {
        //     strafeDir = -1;
        // }
    }
    private void PrepareOneLaunch(Vector3 target)
    {
        GameObject attackArea = ObjectPool.Instance.GetObject(attackAreaPrefab);
        attackArea.transform.position = Physics2D.Raycast(target, Vector3.down, CameraBehaviour.Instance.ReturnCameraY() * 5, LayerMask.GetMask("Ground")).point;
        attackArea.GetComponent<VeyronMissileAimArea>().SetData(this, missileSpeed, aimArea);
    }
    public void LaunchMissile(Vector3 targetpoint, Vector3 dir, float startdsts, float speed , GameObject areaObj)
    {
        GameObject missile = ObjectPool.Instance.GetObject(missilePrefab);
        missile.transform.position = targetpoint + -1 * dir * startdsts;
        missile.GetComponent<VeyronMissile>().SetData(this, speed, dir , areaObj);
    }
    public void LaunchMissile(Vector3 targetpoint, Vector3 dir, float startdsts, float speed)
    {
        GameObject missile = ObjectPool.Instance.GetObject(missilePrefab);
        missile.transform.position = targetpoint + -1 * dir * startdsts;
        missile.GetComponent<VeyronMissile>().SetData(this, speed, dir);
    }
    public void ExplosionSound()
    {
        if (explosionSound)
        {
            SoundManager.Instance.PlayEffectSound(explosionSound);
        }
    }
    public void OnExit()
    {

    }
    public void HitPlayer(Collider2D target, float hitDir)
    {
        player.GetComponent<OnHit>().OnHit(damage, hitDir);
    }
}

public class VeyronMissileAttackEndstateact : State
{
    private int moveDir;
    private VeyronController veyron;
    private float speedY;
    private float speedX;
    private float timer;
    [SerializeField] private float risingTime = 3;
    private bool isSecondPhase;
    private GameObject Missiles;

    public VeyronMissileAttackEndstateact(VeyronController veyron)
    {
        this.veyron = veyron;
    }

    public void OnEnter()
    {
        if (CameraBehaviour.Instance.ReturnCameraPosition().x > veyron.transform.position.x)
        {
            moveDir = -1;
        }
        else
        {
            moveDir = 1;
        }
        speedX = 0;
        speedY = 0;
        timer = 0;
        isSecondPhase = veyron.isSecondPhase;
        GameObject missile = ObjectPool.Instance.GetObject(Missiles);
    }

    public void OnUpdate()
    {
        speedX += Time.deltaTime * 3;
        speedY = Time.deltaTime * Time.deltaTime * 3;
        timer += Time.deltaTime;
        if (timer > risingTime)
        {
            // veyron.EndNowState();
            veyron.NextState();
        }
    }

    public void OnFixedUpdate()
    {

    }

    public void OnExit()
    {

    }
}
public class HalfHealthstateact : State
{
    //此种射击方式存在弹道？比较密集的机枪只扫射一遍？然后转换方向？还是扫射多次需要玩家把我闪避时机？
    //配合玩家的两次闪避充能来扫射两次？
    //如果没有弹道提示则会在枪口有一个充能提示？聚集的红光特效？子弹的拖尾很长很明显？还是直接换成无拖尾但会留下弹道的射击模式？

    private VeyronController veyron;
    private float changeSpeed = 3f;
    private float time = 1f;
    private float timer;
    public HalfHealthstateact(VeyronController veyron)
    {
        this.veyron = veyron;
    }
    public void OnEnter()
    {
        veyron.SetSpeed(new Vector3(Random.Range(-4f , 4f) , Random.Range(-4f , 4f)));
        timer = time;
    }

    public void OnUpdate()
    {
        veyron.SpeedChange(Vector2.zero , changeSpeed);
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            EndThisState();
        }
    }
    private void EndThisState()
    {
        if (Random.Range(0, 2) > 0)
        {
            veyron.MoveToPoint((Vector3)((Vector2)CameraBehaviour.Instance.ReturnCameraPosition() + CameraBehaviour.Instance.ReturnCameraX() * 0.5f * Vector2.right + CameraBehaviour.Instance.ReturnCameraY() * 1f * Vector2.up), VeyronStateType.Bomb);
        }
        else
        {
            veyron.MoveToPoint((Vector3)((Vector2)CameraBehaviour.Instance.ReturnCameraPosition() - CameraBehaviour.Instance.ReturnCameraX() * 0.5f * Vector2.right + CameraBehaviour.Instance.ReturnCameraY() * 1f * Vector2.up), VeyronStateType.Bomb);
        }
    }

    public void OnFixedUpdate()
    {
        
    }
    public void OnExit()
    {

    }

}

public class Bombstateact : State , MissileLauncher
{
    //此种射击方式存在弹道？比较密集的机枪只扫射一遍？然后转换方向？还是扫射多次需要玩家把我闪避时机？
    //配合玩家的两次闪避充能来扫射两次？
    //如果没有弹道提示则会在枪口有一个充能提示？聚集的红光特效？子弹的拖尾很长很明显？还是直接换成无拖尾但会留下弹道的射击模式？

    [SerializeField] private float damage = 4;
    private VeyronController veyron;
    private bool isSecondPhase;
    private int attackNum = 30;
    private int attackCount;
    private Transform player;
    private GameObject attackAreaPrefab;
    private GameObject missilePrefab;
    private float shootInterval = 0.2f;
    private float shootTimer;
    private RaycastHit2D aimResults;
    [SerializeField] private float missileSpeed = 50;
    private Vector3 aimArea = new Vector3(0.3f, 30, 1);
    private GameObject scanPre;
    private float startTimer;
    private Vector3 camera;
    [SerializeField]private AudioClip explosionSound;


    public Bombstateact(VeyronController veyron)
    {
        this.veyron = veyron;
        attackAreaPrefab = veyron.CGAreaPrefab;
        missilePrefab = veyron.bizelMissilePrefab;
        explosionSound = veyron.missileExplosionSound;
    }
    public void OnEnter()
    {
        isSecondPhase = veyron.isSecondPhase;
        player = GameObject.Find("Player").transform;
        shootTimer = 0;
        scanPre = veyron.scanPre;
        startTimer = 2;
        camera = CameraBehaviour.Instance.ReturnCameraPosition();
        attackCount = 0;
    }

    public void OnUpdate()
    {
        if (startTimer > 0)
        {
            startTimer -= Time.deltaTime;
        }
        if (startTimer <= 0)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                shootTimer = 0;
                PrepareOneLaunch();
                attackCount ++;
            }
        }
        if (attackCount >= attackNum)
        {
            veyron.EndNowState();
        }
    }

    public void OnFixedUpdate()
    {

    }
    private void MissileCGprepare()
    {
        
    }
    private void PrepareOneLaunch()
    {
        GameObject attackArea = ObjectPool.Instance.GetObject(attackAreaPrefab);
        attackArea.transform.position = Physics2D.Raycast(camera + Random.Range(-0.5f , 0.5f) * CameraBehaviour.Instance.ReturnCameraX() * Vector3.right , Vector3.down, CameraBehaviour.Instance.ReturnCameraY() * 5, LayerMask.GetMask("Ground")).point;
        attackArea.GetComponent<VeyronCGMissileAimArea>().SetData(this, missileSpeed, aimArea);
    }
    public void LaunchMissile(Vector3 targetpoint, Vector3 dir, float startdsts, float speed , GameObject areaObj)
    {
        GameObject missile = ObjectPool.Instance.GetObject(missilePrefab);
        missile.transform.position = veyron.transform.position;
        missile.GetComponent<MissileFather>().SetData(this, speed, dir , areaObj , targetpoint);
    }
    public void LaunchMissile(Vector3 targetpoint, Vector3 dir, float startdsts, float speed)
    {
        GameObject missile = ObjectPool.Instance.GetObject(missilePrefab);
        missile.transform.position = veyron.transform.position;
        missile.GetComponent<MissileFather>().SetData(this, speed, dir);
    }
    public void LockPlayer(Transform player)
    {
         
    }
    public void ExplosionSound()
    {
        if (explosionSound)
        {
            SoundManager.Instance.PlayEffectSound(explosionSound);
        }
    }
    public void OnExit()
    {

    }
    public void HitPlayer(Collider2D target, float hitDir)
    {
        player.GetComponent<OnHit>().OnHit(damage, hitDir);
    }
}

public class Deadstateact : State
{
    //此种射击方式存在弹道？比较密集的机枪只扫射一遍？然后转换方向？还是扫射多次需要玩家把我闪避时机？
    //配合玩家的两次闪避充能来扫射两次？
    //如果没有弹道提示则会在枪口有一个充能提示？聚集的红光特效？子弹的拖尾很长很明显？还是直接换成无拖尾但会留下弹道的射击模式？

    private VeyronController veyron;
    private float changeSpeed = 3f;
    private float time = 3;
    private float timer;
    private float explosionInterval = 0.1f;
    private float explosionCount;
    public Deadstateact(VeyronController veyron)
    {
        this.veyron = veyron;
    }
    public void OnEnter()
    {
        timer = 0;
        explosionCount = 0;
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer > explosionCount * explosionInterval)
        {
            veyron.SmallExplosion();
            explosionCount ++;
        }
        if (timer > time)
        {
            veyron.Destory();
            // ObjectPool.Instance.PushObject(veyron.gameObject);
        }
        // Move();
    }

    public void OnFixedUpdate()
    {
        
    }
    public void OnExit()
    {

    }
    // private void Move()
    // {
    //     angleChangeSpeed = Mathf.Max(0.5f, Mathf.Min(Vector3.Distance(veyron.transform.position, target.transform.position + position) / 1.5f, 1));
    //     angle += Time.deltaTime * 2.7f * angleChangeSpeed;
    //     if (angle > 360)
    //     {
    //         angle = angle % 360;
    //     }
    //     position.x = Mathf.Sin(angle) * positionX;
    //     position.y = Mathf.Sin(angle * 2) * positionY;
    //     position += 6 * Vector3.up;
    //     veyron.transform.position = Vector3.MoveTowards(veyron.transform.position, target.transform.position + position, Mathf.Min(speedUplimit, speedMutiple * Vector3.Distance(veyron.transform.position, target.transform.position + position)) * Time.deltaTime);
    // }

}

public class Switchstateact : State
{
    private VeyronController veyron;
    private float changeSpeed = 3f;
    private float time = 3;
    private float timer;
    public Switchstateact(VeyronController veyron)
    {
        this.veyron = veyron;
    }
    public void OnEnter()
    {
        timer = time;
        if (veyron.isSecondPhase)
        {
            timer /= 2;
        }
    }

    public void OnUpdate()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            veyron.StateSwitchingStatus();
        }
        // Move();
        veyron.SpeedChange(Vector2.zero , 3);
    }

    public void OnFixedUpdate()
    {
        
    }
    public void OnExit()
    {

    }

}

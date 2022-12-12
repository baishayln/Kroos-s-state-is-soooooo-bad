using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rig;
    private PlayerShoot shootController;
    [SerializeField]public float moveSpeed = 8;
    [SerializeField]public float upSpeedY = 14;
    private Vector2 speed = new Vector2(0 , 0);
    private bool isLeft;
    private bool isRight;
    private bool isOnGround;
    private Collider2D clder;
    private bool isDash;
    private bool isJump;
    private float jumpTimer;
    [SerializeField]public float jumpTime = 0.25f;
    //受击,无敌时间，修改rig，无法射击时间
    private bool isOnHit;
    private bool isHited;
    private float healthLimit = 20;
    [SerializeField]public float health;
    [SerializeField]public float onHitInvincibleTime = 2;
    private float invincibleTimer;
    private bool isInvincible;
    [SerializeField]public float cantShootTime = 0.3f;
    private float cantShootTimer;
    //冲刺
    [SerializeField]public float dashSpeed = 50;
    [SerializeField]public float dashTime = 0.25f;
    private float dashTimer;
    [SerializeField]public float dashColdTime = 3;
    private float dashColdTimer;
    private Vector3 mousePositionOnScreen;//鼠标在屏幕的坐标
    private Vector3 mousePositionInWorld;//把鼠标的屏幕坐标转换为世界坐标
    private GameObject FightUI;
    //冲刺和受击时不能射击，冲刺没有碰撞判定，冲刺是任意方向冲刺还是固定方向？会不会残留速度？
    //冲刺动作？动画？就是站着和贪食地牢一样吧，残影问题

    //玩家自身的受伤判定碰撞器
    private GameObject body;
    private int downFromPlatformCount = 5;
    private bool banRight  = false;
    private bool banLeft  = false;
    [SerializeField]public GameObject aftertimeImagePre;
    [SerializeField]public float aftertimeImageDisappearSpeed = 1;
    [SerializeField]public float aftertimeImageStartAlpha = 1;
    [SerializeField]private float rendererFlashInterval = 0.15f;
    private float rendererFlashIntervalTimer;
    private SpriteRenderer playerRenderer;
    
    // private Vector2 p
    // Start is called before the first frame update
    void Start()
    {
        rig = gameObject.GetComponent<Rigidbody2D>();
        shootController = gameObject.GetComponent<PlayerShoot>();
        clder = gameObject.GetComponent<Collider2D>();
        health = healthLimit;
        isInvincible = false;
        dashColdTimer = 0;
        FightUI = GameObject.Find("FightUI").gameObject;
        FightUI.GetComponent<FightUIController>().SetHealthBar(healthLimit , health);
        body = transform.GetChild(1).gameObject;
        downFromPlatformCount = 0;
        playerRenderer = transform.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        isOnGround = (clder.IsTouchingLayers(LayerMask.GetMask("Ground")) || clder.IsTouchingLayers(LayerMask.GetMask("Platform")));
        
        if (isHited && !isOnGround)
        {
            isOnHit = true;
            isHited = false;
        }
        if (banLeft && isOnHit)
        {
            speed.x = 0;
        }
        if (banRight && isOnHit)
        {
            speed.x = 0;
        }
        if((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !isOnHit)
        {
            if (!banLeft)
            {
                isLeft = true;
            }
            else
            {
                isLeft = false;
            }
        }
        else
        {
            isLeft = false;
        }
        if((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !isOnHit)
        {
            if (!banRight)
            {
                isRight = true;
            }
            else
            {
                isRight = false;
            }
        }
        else
        {
            isRight = false;
        }

        // if(Input.GetKeyDown(KeyCode.Mouse1) && !isOnHit && dashColdTimer <= 0)           //原本的冲刺判断代码
        // {
        //     Dash();
        // }
        if(Input.GetKeyDown(KeyCode.Mouse1) && !isOnHit && dashColdTimer <= 0 && (isLeft || isRight))
        {
            Dash();
        }
        
        if(isOnHit && isOnGround)
        {
            isOnHit = false;
        }
        if(Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            if(!clder.IsTouchingLayers(LayerMask.GetMask("FloorWall")))
            {
                speed.y = upSpeedY;
                jumpTimer = jumpTime;
            }
        }
        else if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
            if(Input.GetButtonUp("Jump")) jumpTimer = -1;
            speed.y = upSpeedY;
        }
        else
        {
            if(!isDash)
                {speed.y = rig.velocity.y;}
        }
        if (jumpTimer > 0)
        {
            jumpTimer -= Time.deltaTime;
            if(Input.GetButtonUp("Jump")) jumpTimer = -1;
            speed.y = upSpeedY;
        }

        if (cantShootTimer > 0)
        {
            if (cantShootTimer <= Time.deltaTime)
            {
                transform.GetComponent<PlayerShoot>().CanShoot();
            }
            cantShootTimer -= Time.deltaTime;
        }

        if(!isOnHit && !isDash)
        {
            // speed.x = Mathf.Clamp(speed.x , -moveSpeed , moveSpeed);
            // speed.y = Mathf.Clamp(speed.y , -upSpeedY , upSpeedY);
            // speed = Vector2.ClampMagnitude(speed , (moveSpeed + upSpeedY)/2f);
            //限制最大速度，并且当速度大于一个更小的值时会向其靠拢？
            if(isLeft && !isRight)
            {
                speed.x = Mathf.MoveTowards(speed.x , -moveSpeed , Time.deltaTime * 40);
            }
            else if(isRight && !isLeft)
            {
                speed.x = Mathf.MoveTowards(speed.x , moveSpeed , Time.deltaTime * 40);
            }
            else
            {
                speed.x = Mathf.MoveTowards(speed.x , 0 , Time.deltaTime * 40);
            }
        }
        // else if(!isDash)
        // {
        //     speed.x = Mathf.Clamp(speed.x , moveSpeed , -moveSpeed);
        //     speed.y = Mathf.Clamp(speed.y , upSpeedY , -upSpeedY);
        // }
        rig.velocity = speed;

        if (invincibleTimer > 0)
        {
            if (rendererFlashIntervalTimer <= Time.deltaTime)
            {
                playerRenderer.enabled = !playerRenderer.enabled;
                rendererFlashIntervalTimer = rendererFlashInterval;
            }
            rendererFlashIntervalTimer -= Time.deltaTime;
            if (invincibleTimer <= Time.deltaTime)
            {
                isInvincible = false;
                isOnHit = false;
                rendererFlashIntervalTimer = -1;
                playerRenderer.enabled = true;
            }
            invincibleTimer -= Time.deltaTime;
        }
        // if (dashTimer > 0)
        // {   
        //     if (dashTimer <= Time.deltaTime)
        //     {
        //         isInvincible = false;
        //         isDash = false;
        //     }
        //     dashTimer -= Time.deltaTime;
        // }
    }
    private GameObject aftertimeImage;
    private bool isSetAftertimeImage = true;
    void FixedUpdate()
    {
        if (dashTimer > 0)
        {
            if (isSetAftertimeImage)
            {
                aftertimeImage = ObjectPool.Instance.GetObject(aftertimeImagePre);
                aftertimeImage.GetComponent<AfterImageInPlayerDash>().SetDate(aftertimeImageDisappearSpeed , transform.GetComponent<SpriteRenderer>().sprite , transform.position , aftertimeImageStartAlpha);
            }
            isSetAftertimeImage = !isSetAftertimeImage;
            if (dashTimer <= Time.deltaTime)
            {
                if (invincibleTimer <= 0)
                {
                    isInvincible = false;
                }
                isDash = false;
                speed = Vector2.ClampMagnitude(speed , (moveSpeed + upSpeedY)/2f);
                rig.velocity = speed;
                transform.GetComponent<PlayerShoot>().CanShoot();
                isSetAftertimeImage = true;
            }
            dashTimer -= Time.deltaTime;
        }
        if(dashColdTimer > 0)
        {
            dashColdTimer -= Time.deltaTime;
        }
        
        if (downFromPlatformCount > 0)
        {
            downFromPlatformCount --;
            if (downFromPlatformCount == 0)
            {
                clder.enabled = true;
                // Debug.Log("结束下平台");
            }
        }
        if (clder.IsTouchingLayers(LayerMask.GetMask("Platform")) && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
        {
            downFromPlatformCount = 8;
            clder.enabled = false;
            // Debug.Log("下平台");
        }
    }
    public void onHit(float  damage , float dir)
    {
        if(!isInvincible)
        {
            if (dir > 0)
            {
                dir = 1;
            }
            else
            {
                dir = -1;
            }
            isHited = true;
            health -= damage;
            FightUI.GetComponent<FightUIController>().SetHealthBar(healthLimit , health);
            isInvincible = true;
            invincibleTimer = onHitInvincibleTime;
            speed.x = dir * 8;
            speed.y = 20;
            rig.velocity = speed;
            cantShootTimer = cantShootTime;
            transform.GetComponent<PlayerShoot>().CantShoot();
        }
    }
    public void onHit(float  damage)
    {
        isHited = true;
        health -= damage;
        FightUI.GetComponent<FightUIController>().SetHealthBar(healthLimit , health);
        isInvincible = true;
        invincibleTimer = onHitInvincibleTime;
        cantShootTimer = cantShootTime;
        transform.GetComponent<PlayerShoot>().CantShoot();
    }
    // private void Dash()                    //原本的冲刺函数
    // {
    //     //获取鼠标在场景中坐标

    //     mousePositionOnScreen = Input.mousePosition;

    //     //将相机中的坐标转化为世界坐标

    //     mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);

    //     speed = mousePositionInWorld - transform.position;
    //     speed = speed.normalized * dashSpeed;
    //     dashTimer = dashTime;
    //     isInvincible = true;
    //     isDash = true;
    //     dashColdTimer = dashColdTime;
    //     transform.GetComponent<PlayerShoot>().CantShoot();
    // }
    private void Dash()
    {
        if(isRight)
        {
            speed = Vector2.right * dashSpeed;
        }
        if(isLeft)
        {
            speed = Vector2.left * dashSpeed;
        }
        dashTimer = dashTime;
        isInvincible = true;
        isDash = true;
        dashColdTimer = dashColdTime;
        transform.GetComponent<PlayerShoot>().CantShoot();
    }
    public bool isFullOfHealth()
    {
        if(health == healthLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // public void HealthUP(float num)
    // {
    //     if(health < healthLimit - num)
    //     {
    //         health += num;
    //     }
    //     else
    //     {
    //         health = healthLimit;
    //     }
    //     FightUI.GetComponent<FightUIController>().SetHealthBar(healthLimit , health);
    // }
    public float RestoreHP(float restoreNum)
    {
        if(health < healthLimit - restoreNum)
        {
            health += restoreNum;
        }
        else
        {
            health = healthLimit;
        }
        FightUI.GetComponent<FightUIController>().SetHealthBar(healthLimit , health);
        return health;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.position.x > transform.position.x && other.transform.CompareTag("AirWall"))
        {
            banRight = true;
        }
        if (other.transform.position.x < transform.position.x && other.transform.CompareTag("AirWall"))
        {
            banLeft = true;
        }
        if (other.transform.position.x > transform.position.x && other.transform.CompareTag("Ground") && clder.IsTouchingLayers(LayerMask.GetMask("FloorWall")))
        {
            banRight = true;
        }
        if (other.transform.position.x < transform.position.x && other.transform.CompareTag("Ground") && clder.IsTouchingLayers(LayerMask.GetMask("FloorWall")))
        {
            banLeft = true;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.CompareTag("AirWall"))
        {
            banRight = false;
            banLeft = false;
        }
    }
}

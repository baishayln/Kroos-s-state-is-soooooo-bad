using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveSingleSteeringProjectile : MonoBehaviour
{
    private Vector3 initialAngle;
    private Vector3 targetAngle;
    [SerializeField]public float angleSpeed;
    [SerializeField]public float steerTime;
    private float steerTimer;
    [SerializeField]public float firstInitialSpeed;
    [SerializeField]public float firstSpeedMinxProport;
    [SerializeField]public float secondInitialSpeed;
    [SerializeField]public float secondPhaseSpeedUpLimit;
    [SerializeField]public float secondPhaseTime;
    [SerializeField]public float secondPhaseSightMoveSpeed;
    [SerializeField]public Vector2 secondForwardDir;
    [SerializeField]public float secondAcceleratedVelocity;  //二阶段加速度
    [SerializeField]public float trackingTime;
    private float trackingTimer;
    private float secondPhaseTimer;
    private float speed;
    private float deadTime = 3;
    private float deadTimer;
    private Rigidbody2D rig;
    private bool arrived;
    private float lerp;
    [SerializeField]public Transform target;
    private GameObject sight;
    private GameObject trail;
    protected Collider2D[] objs;
    protected float nearsetEnemyDsts;
    protected Transform nearestEnemy;
    private Vector2 dirction;
    [SerializeField]private GameObject tarilPrefab;
    private GameObject taril;
    [SerializeField]private float trailStopTime = 1f;
    private float damage;
    private bool canExplosion;
    [SerializeField]private GameObject particlePrefab;
    [SerializeField]private GameObject particlePrefab1;
    void Awake()
    {
        rig = transform.GetComponent<Rigidbody2D>();
        sight = transform.GetChild(0).gameObject;
        // sight.transform.SetParent(transform);
        // sight.transform.SetParent(transform.Find("SurrounderPool"));
    }
    void OnEnable()
    {
        target = null;
        deadTimer = deadTime;
        steerTimer = steerTime;
        secondPhaseTimer = 0;
        arrived = false;
        trackingTimer = trackingTime;
        canExplosion = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (steerTimer >= 0)
        {
            FirstPhase();
            if (steerTimer < Time.deltaTime && !target)
            {
                target = GetNearestEnemy(20);
            }

            // if (steerTimer < Time.deltaTime)
            // {
            //     secondForwardDir
            // }
        }
        else
        {
            SecondPhase();
        }
        steerTimer -= Time.deltaTime;
        
        deadTimer -= Time.deltaTime;
        if (deadTimer < 0)
        {
            PushSelf();
        }
        RotateSelf();
    }
    private void FirstPhase()
    {
        speed = (steerTimer/steerTime) * firstInitialSpeed;
        transform.up = Vector3.Slerp(targetAngle , initialAngle , steerTimer/steerTime);
        rig.velocity = transform.up * speed;
        secondForwardDir = transform.up;
    }

    private void RotateSelf()
    {
        dirction.x = rig.velocity.x;
        dirction.y = rig.velocity.y;
        transform.up = dirction;
    }
    
    private void SecondPhase()
    {
        if (secondPhaseTimer < secondPhaseTime)
        {
            secondPhaseTimer += Time.deltaTime;
            if (secondPhaseTimer > secondPhaseTime)
                secondPhaseTimer = secondPhaseTime;
            speed = (secondPhaseTimer/secondPhaseTime) * secondPhaseSpeedUpLimit;

            if (target)
            {
                sight.transform.up = target.position - transform.position;
                secondForwardDir = Vector3.MoveTowards(secondForwardDir , sight.transform.up , secondPhaseSightMoveSpeed * Time.deltaTime);
                if (target.GetComponent<EnemyBehavior>().GetHealth() <= 0)
                {
                    target = null;
                }
            }

            rig.velocity = secondForwardDir.normalized * speed;
            
        }

        
        // secondPhaseTimer += Time.deltaTime;
        // if (speed < secondSpeedUpLimit)
        //     speed += Time.deltaTime * secondAcceleratedVelocity * (float)(1 + secondPhaseTimer/0.5);
        // else if (speed > secondSpeedUpLimit)
        //     speed = secondSpeedUpLimit;
 
        // if(trackingTimer > 0)
        // {
        //     sight.transform.up = new Vector3(target.transform.position.x - transform.position.x , target.transform.position.y - transform.position.y , target.transform.position.z - transform.position.z);
        //     trackingTimer -= Time.deltaTime;
        //     transform.rotation = Quaternion.RotateTowards(transform.rotation , sight.transform.rotation , angleSpeed);
        //     rig.velocity = transform.up * speed;
        // }

    }
    
    private void PushSelf()
    {
        PushRrail();
        ObjectPool.Instance.PushObject(gameObject);
    }

    public void setAngle(Vector3 iniangle)
    {
        initialAngle = iniangle;
        transform.up = initialAngle;
        targetAngle = Vector3.Slerp(initialAngle , transform.right , 2/3f);
        SetRrail();
    }
    public void setDamage(float dmg)
    {
        damage = dmg;
    }
    public void setTarget(GameObject tgt)
    {
        target = tgt.transform;
    }
    
    virtual public Transform GetNearestEnemy(float r)         //当单位受伤死亡后，返回true
    {
        objs = Physics2D.OverlapCircleAll(transform.position , r , LayerMask.GetMask("Enemy"));
        if (objs.Length == 0)
        {
            return null;
        }
        
        nearsetEnemyDsts = Vector2.Distance(transform.position , objs[0].transform.position);
        nearestEnemy = objs[0].transform;
        for(int i = 1 ; i < objs.Length ; i ++)
        {
            if (Vector2.Distance(transform.position , objs[i].transform.position) < nearsetEnemyDsts)
            {
                if (objs[i].GetComponent<EnemyBehavior>().GetHealth() > 0)
                {
                    nearsetEnemyDsts = Vector2.Distance(transform.position , objs[i].transform.position);
                    nearestEnemy = objs[i].transform;
                }
            }
        }
        return nearestEnemy;
    }
    public void SetRrail()
    {
        taril = ObjectPool.Instance.GetObject(tarilPrefab);
        taril.transform.SetParent(transform);
        taril.transform.position = transform.position - transform.up * 0.4f * 0.6f;
        taril.GetComponent<PlayerBulletTrail>().trailRenderer.enabled = true;
    }
    public void PushRrail()
    {
        taril.GetComponent<PlayerBulletTrail>().StopTrail(trailStopTime);
        taril = null;
    }
    public void Explosion()
    {
        canExplosion = false;
        Collider2D[] enemyWhoWasShot =  Physics2D.OverlapBoxAll(transform.position , Vector2.one * 4 , 0 , LayerMask.GetMask("Enemy"));
        for(int i = enemyWhoWasShot.Length ; i > 0 ; i --)
        {
            enemyWhoWasShot[i - 1].GetComponent<EnemyBehavior>().OnHit(damage , (enemyWhoWasShot[i - 1].transform.position - transform.position).normalized * Random.Range(20f , 40f));
        }
        GameObject particle = ObjectPool.Instance.GetObject(particlePrefab);
        particle.GetComponent<ParticleEffect>().SetData(transform.position);
        GameObject particle1 = ObjectPool.Instance.GetObject(particlePrefab1);
        particle1.GetComponent<ParticleEffect>().SetData(transform.position);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            if(canExplosion)
            {
                Explosion();
                PushSelf();
            }
            
        }
        // startxHisTr = false;
    }
}

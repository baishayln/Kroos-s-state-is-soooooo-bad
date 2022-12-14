using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingArea : MonoBehaviour
{
    private float duration;
    private float damage;
    private float injuryInterval;
    private float timer = 0;
    [SerializeField]public GameObject LightingPrefab;
    [SerializeField]public float aimDistense = 30;
    [SerializeField]public int lightingAttackJumpNum = 4;
    [SerializeField]public float lightingAttackJumpR = 3;

    private Queue<GameObject> enemys = new Queue<GameObject>();
    private RaycastHit2D aimResult;
    private RaycastHit2D attackAimResult;
    private float attackAreaHeight;
    private float attackAreaWidth;
    private Vector2 lightingAreaSize;
    protected Collider2D[] objs;
    protected Transform highestHealthEnemy;
    protected float highestHealth;
    protected Transform target;
    protected Transform nextTarget;
    [SerializeField]private GameObject lightningPrefab;
    private Transform targetEnemy;
    // Start is called before the first frame update
    void Start()
    {
        attackAreaWidth = transform.GetComponent<BoxCollider2D>().size.x;
        attackAreaHeight = transform.GetComponent<BoxCollider2D>().size.y;
        lightingAreaSize.x = attackAreaWidth;
        lightingAreaSize.y = attackAreaHeight;
    }

    // Update is called once per frame
    void Update()
    {
        if(duration > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                duration -= injuryInterval;
                timer = injuryInterval;
                targetEnemy = GetHighestHealthEnemy(lightingAreaSize);
                if (targetEnemy)
                {
                    BornLightningAttack(targetEnemy);
                }
            }
        }
        else
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
    public void SetData(float duration , float damage , float injuryInterval , Vector2 mousePosition)
    {
        // transform.position = mousePosition;
        aimResult = Physics2D.Raycast(mousePosition , Vector2.down , aimDistense , LayerMask.GetMask("Ground"));
        if (aimResult.transform != null)
        {
            transform.position = aimResult.point + Vector2.up * 3;
        }
        else
        {
            transform.position = mousePosition;
        }
        this.duration = duration;
        this.damage = damage;
        this.injuryInterval = injuryInterval;
        timer = injuryInterval;
    }
    private void LightingAttack()
    {
        //??????????????????????????????????????????????????????????????????????????????????????????????????????????????????
        //??????????????????????????????physic2D???
    }
    virtual public Transform GetHighestHealthEnemy(Vector2 lightingAreaSize)
    {
        objs = Physics2D.OverlapBoxAll(transform.position , lightingAreaSize , 0 , LayerMask.GetMask("Enemy"));
        if (objs.Length == 0)
        {
            return null;
        }
        highestHealth = objs[0].GetComponent<EnemyBehavior>().GetHealth();
        highestHealthEnemy = objs[0].transform;
        for(int i = 1 ; i < objs.Length ; i ++)
        {
            if (objs[i].GetComponent<EnemyBehavior>().GetHealth() > highestHealth)
            {
                highestHealth = objs[i].GetComponent<EnemyBehavior>().GetHealth();
                highestHealthEnemy = objs[i].transform;
            }
        }
        return highestHealthEnemy;
    }
    virtual public void LightAttack(Transform target)
    {
        if(target != null)
        {
            for(int i = 0 ; i < lightingAttackJumpNum ; i ++)
            {
                nextTarget = target.GetComponent<EnemyBehavior>().GetNearestEnemy(lightingAttackJumpR);
                target.GetComponent<EnemyBehavior>().OnHit(damage , target.transform.position.x - transform.position.x);      //???????????????????????????????????????????????????????????????????????????????????????????????????
                if (nextTarget == null)
                {
                    i = lightingAttackJumpNum;
                }
                target = nextTarget;
            }
        }
        
    }
    public void BornLightningAttack(Transform targetEnemy)
    {
        attackAimResult = Physics2D.Raycast(targetEnemy.position , Vector2.down , CameraBehaviour.Instance.ReturnBornPosition().y * 2 , LayerMask.GetMask("Ground"));
        GameObject lightning = ObjectPool.Instance.GetObject(lightningPrefab);
        lightning.GetComponent<Lightning>().SetData(targetEnemy , this);
        if (attackAimResult.transform)
        {
            lightning.transform.position = attackAimResult.point + Vector2.up * (lightning.GetComponent<BoxCollider2D>().size.y / 2f - 0.25f);
        }
        else
        {
            lightning.transform.position = targetEnemy.position;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {

    }
    void OnTriggerExit2D(Collider2D other)
    {

    }
}

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
    private Queue<GameObject> enemys = new Queue<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
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
            }
        }
        else
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
    public void SetData(float duration , float damage , float injuryInterval , Vector2 mousePosition)
    {
        transform.position = mousePosition;
        this.duration = duration;
        this.damage = damage;
        this.injuryInterval = injuryInterval;
        timer = injuryInterval;
    }
    private void LightingAttack()
    {
        //每次攻击时在选中敌人位置上方一定距离生成闪电模型，直接给予闪电链攻击的第一段
        //判断范围内的敌人使用physic2D？
    }
    void OnTriggerEnter2D(Collider2D other)
    {

    }
    void OnTriggerExit2D(Collider2D other)
    {

    }
}

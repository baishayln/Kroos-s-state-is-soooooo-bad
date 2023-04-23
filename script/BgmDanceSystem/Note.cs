using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField]private float risingSpeed = 25;
    [SerializeField]private float basicTime = 0.7f;
    [SerializeField]private float basicTimeRange = 0.4f;
    private float nowStateTimer;
    [SerializeField]private float basicSpeed = -10f;
    [SerializeField]private float speedRandomRadius = -5f;
    private float speedTarget;
    [SerializeField]private Rigidbody2D rig;
    private Vector2 speed = Vector2.zero;
    [SerializeField]private float layeralSpeed = -35f;
    [SerializeField]private float layeralSpeedRandomRadius = -10f;
    [SerializeField]private float lifeTime = 15f;
    private float lifeTimer;
    private bool isDown;
    private int moveDir;
    private float speedMultiplier = 1;
    // Start is called before the first frame update
    void Start()
    {
        if(!rig)
        {
            rig = transform.GetComponent<Rigidbody2D>();
        }
    }

    void OnEnable()
    {
        nowStateTimer = basicTime + Random.Range(0f , basicTimeRange);
        speedTarget = basicSpeed + Random.Range(0f , speedRandomRadius);
        speed.x = layeralSpeed + Random.Range(-layeralSpeedRandomRadius , layeralSpeedRandomRadius);
        lifeTimer = lifeTime;
        isDown = false;
        
        if(Random.Range(0 , 2) == 0)
        {
            moveDir = 1;
        }
        else
        {
            moveDir = -1;
        }
        
        if(Random.Range(0 , 2) == 0)
        {
            speed.y = 0;
        }
        else
        {
            speed.y = speedTarget * moveDir;
        }
        transform.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(speed.y - speedTarget * moveDir) < 0.2f)
        {
            nowStateTimer -= Time.deltaTime;
            if(nowStateTimer <= 0)
            {
                moveDir = -moveDir;
                nowStateTimer = basicTime + Random.Range(0f , basicTimeRange);
                speedTarget = basicSpeed + Random.Range(0f , speedRandomRadius);
            }
        }
        if (transform.GetComponent<RectTransform>().localScale.x != 1)
        {
            speedMultiplier = Vector3.one.x / transform.GetComponent<RectTransform>().localScale.x;
            transform.GetComponent<RectTransform>().localScale = Vector3.one;
        }
        speed.y = Mathf.MoveTowards(speed.y , speedTarget * moveDir , Mathf.Abs(speedTarget * Time.deltaTime));
        
        rig.velocity = speed * speedMultiplier;

        lifeTimer -= Time.deltaTime;
        if(lifeTimer <= 0)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}

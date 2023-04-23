using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    private static FollowObject instance;
    public static FollowObject Instance
    {
        get
        {
            if (instance == null)
                instance = Transform.FindObjectOfType<FollowObject>();
            return instance;
        }
    }
    private GameObject target;
    private float lastTargetX;
    private float lastTargetY;
    private float targetX;
    private float targetY;
    private float targetXMove;
    private float targetYMove;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Target (1)");
        lastTargetX = target.transform.position.x;
        lastTargetY = target.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        targetX = target.transform.position.x;
        targetY = target.transform.position.y;
        targetXMove = targetX - lastTargetX;
        targetYMove = targetY - lastTargetY;
        transform.position = new Vector3(transform.position.x + targetXMove , transform.position.y + targetYMove , target.transform.position.z - 10);
        lastTargetX = targetX;
        lastTargetY = targetY;
    }
}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class PlayerBullet : MonoBehaviour
// {
//     private Vector2 dirction;
//     private float decelerationThreshold;
//     private float damage;
//     private Rigidbody2D rig;
//     private Color startColor;
//     private Vector4 color;
//     private SpriteRenderer sprtRenderer;
    
    
//     private GameObject target;
//     private float lastTargetX;
//     private float lastTargetY;
//     private float targetX;
//     private float targetY;
//     private float targetXMove;
//     private float targetYMove;

//     //      如果碰撞到地面会存在一段时间然后逐渐消失
//     // Start is called before the first frame update
//     void Start()
//     {
//         // sprtRenderer = transform.GetComponent<SpriteRenderer>();
//         // startColor = sprtRenderer.color;
//     }
//     void OnEnable()
//     {
//         sprtRenderer = transform.GetComponent<SpriteRenderer>();
//         if(startColor == new Color(0 , 0 , 0 , 0))
//             startColor = sprtRenderer.color;
//         rig = gameObject.GetComponent<Rigidbody2D>();
//         target = null;
//         rig.bodyType = RigidbodyType2D.Dynamic;
//         sprtRenderer.color = startColor;
//         color = startColor;
//     }
//     void FixedUpdate()
//     {
//         if(target != null)
//         {
//             if(color.w > 0)
//                 color.w = color.w - 0.03f;
//             Debug.Log(color.w);
//             sprtRenderer.color = color;
//             FollowTarget();
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         RotateSelf();
//         XSpeedDown();   //按照时间开始减速还是按照已经飞行的距离？或者当前距离玩家的位置？（重力会影响第三种）
//     }
//     private void RotateSelf()
//     {
//         dirction.x = rig.velocity.x;
//         dirction.y = rig.velocity.y;
//         transform.right = dirction;
//     }
//     private void XSpeedDown()
//     {

//     }
//     public void SetBullet(float dmg , float speed , Vector3 dirction)
//     {
//         damage = dmg;
//         transform.right = dirction;
//         rig.velocity = Vector2.right * speed;
//     }
//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if(other.CompareTag("Ground") || other.CompareTag("Enemy"))
//         {
//             Hit(other.transform);
//         }
//     }
//     private void Hit(Transform hitTarget)
//     {
//         target = hitTarget.gameObject;
//         // transform.SetParent(hitTarget);
//         rig.velocity = Vector2.zero;
//         rig.bodyType = RigidbodyType2D.Kinematic;
//         if(hitTarget.CompareTag("Enemy"))
//             hitTarget.GetComponent<EnemyBehavior>().OnHit(damage);
//     }
//     private void FollowTarget()
//     {

//     }
// }

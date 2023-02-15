using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColouredRibbon : MonoBehaviour
{
    [SerializeField]private float risingSpeed = 15;
    [SerializeField]private float risingTime = 0.3f;
    [SerializeField]private float risingTimeRange = 0.2f;
    private float risingTimer;
    [SerializeField]private float fallingSpeed = -10f;
    [SerializeField]private float fallingSpeedRandomRadius = -5f;
    private float fallingSpeedTarget;
    [SerializeField]private Rigidbody2D rig;
    private Vector2 speed = Vector2.zero;
    [SerializeField]private float layeralSpeed = -7.5f;
    [SerializeField]private float layeralSpeedRandomRadius = -7.5f;
    [SerializeField]private float lifeTime = 15f;
    private float lifeTimer;
    private bool isDown;
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
        risingTimer = risingTime + Random.Range(0f , risingTimeRange);
        speed.y = risingSpeed;
        speed.x = layeralSpeed + Random.Range(-risingSpeed , risingSpeed);
        lifeTimer = lifeTime;
        isDown = false;
        fallingSpeedTarget = fallingSpeed + Random.Range(0f , fallingSpeedRandomRadius);
    }

    // Update is called once per frame
    void Update()
    {
        risingTimer -= Time.deltaTime;
        if(risingTimer <= 0)
        {
            speed.y = Mathf.MoveTowards(speed.y , fallingSpeedTarget , Mathf.Abs(fallingSpeedTarget));
            if(!isDown)
            {
                isDown = true;
                speed.x = layeralSpeed + Random.Range(layeralSpeedRandomRadius , 0f);
            }
        }
        rig.velocity = speed;

        lifeTimer -= Time.deltaTime;
        if(lifeTimer <= 0)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}

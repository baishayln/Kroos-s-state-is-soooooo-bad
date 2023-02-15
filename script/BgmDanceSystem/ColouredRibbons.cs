using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColouredRibbons : MonoBehaviour
{
    [SerializeField]private float risingSpeed = 150;
    [SerializeField]private float risingTime = 0.4f;
    [SerializeField]private float risingSpeedRange = 0.1f;
    private float risingTimer;
    [SerializeField]private float fallingSpeed = -15;
    [SerializeField]private Rigidbody2D rig;
    private Vector2 speed = Vector2.zero;
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
        risingTimer = risingTime;
        speed.y = risingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        risingTimer -= Time.deltaTime;
        if(risingTimer <= 0)
        {
            speed.y = Mathf.MoveTowards(speed.y , fallingSpeed , Mathf.Abs(fallingSpeed));
        }
        rig.velocity = speed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHand : MonoBehaviour
{
    [SerializeField]
    public GameObject target;
    private float camSpeed = 0;
    [SerializeField]
    private float spdDownSpd = 0.06f;
    private float MINSpd = 0;
    [SerializeField]
    private float XMAX = 3;
    [SerializeField]
    private float YMAX = 1.8f;
    private float Xr = 3;
    private float Yr = 1.8f;
    private float juli;
    private float SELFX;
    private float SELFY;
    // Start is called before the first frame update
    void Start()
    {
        if (!target)
        {
            target = GameObject.Find("Camera Rack");
        }
    }

    void FixedUpdate()
    {
        SELFX = transform.position.x;
        SELFY = transform.position.y;
        if (Mathf.Abs(target.transform.position.x - SELFX) > Mathf.Abs((target.transform.position.y - SELFY)/0.6f))
        {
            juli = Mathf.Abs(target.transform.position.x - SELFX);
        }
        else
        {
            juli = Mathf.Abs((target.transform.position.y - SELFY)/0.6f);
        }

        camSpeed = juli * 5;

        transform.position = Vector3.MoveTowards(transform.position,
            target.transform.position, camSpeed * Time.deltaTime);


    }

    // Update is called once per frame
    void Update()
    {
        // SELFX = transform.position.x;
        // SELFY = transform.position.y;
        // if (Mathf.Abs(player.transform.position.x - SELFX) > Mathf.Abs((player.transform.position.y - SELFY)/0.6f))
        // {
        //     juli = Mathf.Abs(player.transform.position.x - SELFX);
        // }
        // else
        // {
        //     juli = Mathf.Abs((player.transform.position.y - SELFY)/0.6f);
        // }

        // camSpeed = juli * 5;

        // transform.position = Vector2.MoveTowards(transform.position,
        //     player.transform.position, camSpeed * Time.deltaTime);

        // Debug.Log(camSpeed);


        // if ( (cam.transform.position.x - SELFX > XMAX) || (cam.transform.position.x - SELFX > XMAX) )
        // {
            
        // }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private static CameraBehaviour instance;
    public static CameraBehaviour Instance
    {
        get
        {
            if (instance == null)
                instance = Transform.FindObjectOfType<CameraBehaviour>();
            return instance;
        }
    }
    [SerializeField]public GameObject target;
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
    private bool isShake;
    private bool isPause;
    private bool wdnmd;
    private Vector3 shouleBePosition;
    private float ROfcameraX;
    private float ROfcameraY;
    private Vector3 ROfenemyBorn = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!target)
        {
            target = GameObject.Find("Camera Rack");
        }
        shouleBePosition = transform.position;
        // Vector3 cameraZero = transform.GetComponent<Camera>().
        ROfcameraX = (Camera.main.ViewportToWorldPoint(Vector3.one).x - Camera.main.ViewportToWorldPoint(Vector3.zero).x) * 0.5f + 1.5f;
        ROfcameraY = (Camera.main.ViewportToWorldPoint(Vector3.one).y - Camera.main.ViewportToWorldPoint(Vector3.zero).y) * 0.5f + 1.5f;
        ROfenemyBorn.x = ROfcameraX;
        ROfenemyBorn.y = ROfcameraY;
        // ROfenemyBorn.x = Screen.width * 0.5f + 1.5f;
        // ROfenemyBorn.y = Screen.height * 0.5f + 1.5f;
    }

    void FixedUpdate()
    {
        SELFX = shouleBePosition.x;
        SELFY = shouleBePosition.y;
        if (Mathf.Abs(target.transform.position.x - SELFX) > Mathf.Abs((target.transform.position.y - SELFY)/0.6f))
        {
            juli = Mathf.Abs(target.transform.position.x - SELFX);
        }
        else
        {
            juli = Mathf.Abs((target.transform.position.y - SELFY)/0.6f);
        }

        camSpeed = juli * 5;

        shouleBePosition = Vector3.MoveTowards(shouleBePosition,
            target.transform.position, camSpeed * Time.deltaTime);

        if (!isPause || !isShake)
        {
            transform.position = shouleBePosition;
        }

    }

    public void HitPause(int duration)
    {
        StartCoroutine(Pause(duration));
    }

    IEnumerator Pause(int duration)
    {
        isPause = true;
        float pauseTime = duration / 60f;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(pauseTime);
        Time.timeScale = 1;
        isPause = false;
        // yield return wdnmd = new AsyncOperation().isDone;
    }

    public void CameraShake(float duration, float strength)
    {
        if (!isShake)
            StartCoroutine(Shake(duration, strength));
    }

    IEnumerator Shake(float duration, float strength)
    {
        isShake = true;
        Transform camera = Camera.main.transform;

        while (duration > 0)
        {
            camera.position = Random.insideUnitSphere * Random.Range(strength , strength/2) + shouleBePosition;
            duration -= Time.deltaTime;
            yield return null;
        }
        camera.position = shouleBePosition;
        isShake = false;
    }
    public Vector3 ReturnBornPosition()
    {
        // Vector2 objPosition;
        // objPosition.x = Random.Range();
        // // return objPosition;
        // return Vector2.left;
        //不直接返回位置，而是返回当前相机的边框距离加上1.5形成可选择的刷怪范围，由每种怪物的不同脚本计算不同的刷怪位置区域

        ROfcameraX = (Camera.main.ViewportToWorldPoint(Vector3.one).x - Camera.main.ViewportToWorldPoint(Vector3.zero).x) * 0.5f + 1.5f;
        ROfcameraY = (Camera.main.ViewportToWorldPoint(Vector3.one).y - Camera.main.ViewportToWorldPoint(Vector3.zero).y) * 0.5f + 1.5f;
        ROfenemyBorn.x = ROfcameraX;
        ROfenemyBorn.y = ROfcameraY;
        return ROfenemyBorn;
    }
    public Vector3 ReturnCameraPosition()
    {
        return transform.position;
    }
    public float ReturnCameraX()
    {
        return (Camera.main.ViewportToWorldPoint(Vector3.one).x - Camera.main.ViewportToWorldPoint(Vector3.zero).x);
    }
    public float ReturnCameraY()
    {
        return (Camera.main.ViewportToWorldPoint(Vector3.one).y - Camera.main.ViewportToWorldPoint(Vector3.zero).y);
    }
}


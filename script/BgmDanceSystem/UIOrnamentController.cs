using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UIOrnamentController : MonoBehaviour
{
    private Color visible = new Color(1 , 1 , 1 , 1);
    private Color invisible = new Color(1 , 1 , 1 , 0);
    private Color startColor = new Color(1 , 1 , 1 , 0);
    private StreamWriter streamWriter;
    [SerializeField]private int BPM = 118;
    private float Btime;
    private float BGMPlayTime = 0;
    private bool isVisible = true;

    // Start is called before the first frame update
    void Start()
    {
        streamWriter = new StreamWriter(Application.dataPath + "/DanceAct.txt" , true);
        Btime = 60f/BPM;
    }
    void WriteTime()
    {
        streamWriter.WriteLine("Time:" + BGMPlayTime + "BPM:" + BGMPlayTime/Btime);
        Debug.Log("Time:" + BGMPlayTime + "BPM:" + BGMPlayTime/Btime);
        if (Input.GetKeyDown(KeyCode.J))
        {
            streamWriter.WriteLine("Jump");
            Debug.Log("Jump");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            streamWriter.WriteLine("BigJump");
            Debug.Log("BigJump");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            streamWriter.WriteLine("SingANote");
            Debug.Log("SingANote");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            streamWriter.WriteLine("Sing");
            Debug.Log("Sing");
        }
    }
    void OnApplicationQuit()
    {
        streamWriter.Close();
    }

    // Update is called once per frame
    void Update()
    {
        BGMPlayTime = SoundManager.Instance.GetBGMPlayTime();
        // if (Input.anyKeyDown)
        // {
        //     WriteTime();
        // }
    }
    public void SetVisible()
    {
        isVisible = true;
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (transform.GetChild(i).GetComponent<Image>())
            {
                transform.GetChild(i).GetComponent<Image>().color = visible;
            }
            if (transform.GetChild(i).GetComponent<DancePlayer>())
            {
                transform.GetChild(i).GetComponent<DancePlayer>().SetVisible();
            }
        }
    }
    public void SetInvisible()
    {
        isVisible = false;
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (transform.GetChild(i).GetComponent<Image>())
            {
                transform.GetChild(i).GetComponent<Image>().color = invisible;
            }
            if (transform.GetChild(i).GetComponent<DancePlayer>())
            {
                transform.GetChild(i).GetComponent<DancePlayer>().SetInvisible();
            }
        }
    }
    public bool GetIsVisible()
    {
        return isVisible;
    }
    public Color GetVisibleColor()
    {
        return visible;
    }
    public Color GetInVisibleColor()
    {
        return invisible;
    }
    public void ShowUIOrnament()
    {
        StartCoroutine(ShowOrnament());
    }
    IEnumerator ShowOrnament()
    {
        while(startColor.a < 0.95f)
        {
            startColor.a += Time.deltaTime * 2;
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                if (transform.GetChild(i).GetComponent<Image>())
                {
                    transform.GetChild(i).GetComponent<Image>().color = startColor;
                }
            }
            yield return null;
        }
        startColor.a = 1;
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (transform.GetChild(i).GetComponent<Image>())
            {
                transform.GetChild(i).GetComponent<Image>().color = startColor;
            }
        }
    }
    public float ReturnNowBCount(float time)
    {
        float nowBCount = time/(60f/118f);
        return nowBCount;
    }
}

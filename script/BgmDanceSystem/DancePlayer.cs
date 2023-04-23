using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum DanceAction
{
    Jump , BigJump , SingANote , SingStart , SingStop , Sing , ColouredRibbon , EmptyAct
}
public class DancePlayer : MonoBehaviour
{
    private bool isVisible;
    // private bool isInvisible;        //没必要了
    // Start is called before the first frame update
    [SerializeField]private int BPM = 118;
    private float Btime;
    private float BGMPlayTime = 0;
    [SerializeField]private int startDirction = 1;      //the first dirction of BGM play with start
    private float rockLoopTime;
    private Quaternion selfRotation;
    [SerializeField]private float BGMStartMistakeTime = 0.3f;
    private int endBPM = 344;
    private float BGMEndTime;
    [SerializeField]private float rockAngle = 30;
    private float rockAngleOfEnd;
    [SerializeField]private float[] danceActionBPMCount;
    [SerializeField]private DanceAction[] danceActions;
    private bool[] isActDone;
    private int nowDanceActCount = 0;
    private float nowBCount = 0;
    private bool isSinging = false;
    [SerializeField]private float singingInterval = 0.3f;
    private float singingTimer = 0;
    [SerializeField]private Animator animator;
    [SerializeField]private GameObject colouredRibbonPrefab;
    [SerializeField]private GameObject notePrefab1;
    [SerializeField]private GameObject notePrefab2;
    [SerializeField]private GameObject notePrefab3;
    // [SerializeField]private GameObject notePrefab4;
    // [SerializeField]private GameObject notePrefab5;
    // [SerializeField]private GameObject notePrefab6;
    // [SerializeField]private GameObject notePrefab7;
    [SerializeField]private GameObject colouredRibbonPrefab1;
    [SerializeField]private GameObject colouredRibbonPrefab2;
    [SerializeField]private GameObject colouredRibbonPrefab3;
    [SerializeField]private int ribbonCount = 5;
    // [SerializeField]private int sinkingDistance = 20;
    private Transform ribbonBornPoint;
    [SerializeField]private Transform noteBornPoint;
    [SerializeField]private UIOrnamentController UIO;

    // Start is called before the first frame update
    void Start()
    {
        Btime = 60f/BPM;
        BGMEndTime = Btime * endBPM;
        isActDone = new bool[danceActions.Length];
        if (!animator)
        {
            animator = transform.GetComponent<Animator>();
        }
        if (!UIO)
        {
            UIO = transform.parent.GetComponent<UIOrnamentController>();
        }
        ribbonBornPoint = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        // BGMPlayTime = SoundManager.Instance.GetBGMPlayTime();
        // rockLoopTime = -(BGMPlayTime % 2 - 1);
        // if(rockLoopTime > 0)
        // {
        //     transform.rotation = Quaternion.Euler(0 , 0 , (0.5f - Mathf.Abs(rockLoopTime - 0.5f)) * 30);
        // }
        // else
        // {
        //     transform.rotation = Quaternion.Euler(0 , 0 , (0.5f - Mathf.Abs(rockLoopTime + 0.5f)) * -30);
        // }
        // Debug.Log(rockLoopTime);
        
        if(BGMPlayTime > SoundManager.Instance.GetBGMPlayTime())
        {
            if(!Input.GetKey(KeyCode.Q))
            {
                nowDanceActCount = 0;
            }
            for (int i = isActDone.Length - 1; i >= 0; i--)
            {
                isActDone[i] = false;
            }
        }
            

        BGMPlayTime = SoundManager.Instance.GetBGMPlayTime();
        rockLoopTime = -((BGMPlayTime - BGMStartMistakeTime) % (4 * Btime) - (2 * Btime));         //每两个节拍完成从一侧到另一侧的摇摆，每四个节拍完成一次左右摇摆的循环
                                                                                    //BGM计时中减去的0.2f是音频播放开始到声音播放的时差
                                                                                    //当减去两个节拍的时间为负时算作摇摆至左侧的状态，反之则为右侧
        
        if (BGMPlayTime < BGMStartMistakeTime)
        {
            rockLoopTime = 0;
            if (danceActionBPMCount.Length > 0)
            {
                if (isActDone[0])
                {
                    nowDanceActCount = 0;
                    for (int i = isActDone.Length - 1; i >= 0; i--)
                    {
                        isActDone[i] = false;
                    }
                }
            }
        }

        if (BGMPlayTime < BGMEndTime)
        {
            if(rockLoopTime > 0)
            {
                transform.rotation = Quaternion.Euler(0 , 0 , (Mathf.Sin((Btime - Mathf.Abs(rockLoopTime - Btime)) / Btime * Mathf.PI * 0.5f)) * rockAngle * startDirction);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0 , 0 , (Mathf.Sin((Btime - Mathf.Abs(rockLoopTime + Btime)) / Btime * Mathf.PI * 0.5f)) * -rockAngle * startDirction);
            }
        }
        else
        {
            if (BGMPlayTime - BGMEndTime < 2 * Btime)
            {
                rockAngleOfEnd = rockAngle * ( 2 * Btime - (BGMPlayTime - BGMEndTime));
            }
            if(rockLoopTime > 0)
            {
                transform.rotation = Quaternion.Euler(0 , 0 , (Mathf.Sin((Btime - Mathf.Abs(rockLoopTime - Btime)) / Btime * Mathf.PI * 0.5f)) * rockAngleOfEnd * startDirction);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0 , 0 , (Mathf.Sin((Btime - Mathf.Abs(rockLoopTime + Btime)) / Btime * Mathf.PI * 0.5f)) * -rockAngleOfEnd * startDirction);
            }
        }

        if (nowDanceActCount < danceActionBPMCount.Length)
        {
            nowBCount = BGMPlayTime/Btime;
            if (nowBCount > danceActionBPMCount[nowDanceActCount] && isActDone[nowDanceActCount] != true)
            {
                ExecuteCurrentAction();
                // Debug.Log(nowDanceActCount);
            }
        }

        if(isSinging)
        {
            singingTimer -= Time.deltaTime;
            if(singingTimer <= 0)
            {
                singingTimer = singingInterval;
                SingANote();
            }
        }

        // if(Input.GetKeyDown(KeyCode.A))
        // {
        //     // PushColouredRibbon(ribbonCount);
        //     isSinging = !isSinging;
        // }

    }
    private void ExecuteCurrentAction()
    {
        switch(danceActions[nowDanceActCount])
        {
            case DanceAction.Jump:
                animator.Play("Jump");
                break;
            case DanceAction.BigJump:
                animator.Play("BigJump");
                break;
            case DanceAction.SingANote:
                SingANote();
                break;
            case DanceAction.Sing:
                isSinging = !isSinging;
                singingTimer = 0;
                break;
            case DanceAction.ColouredRibbon:
                PushColouredRibbon(5);
                break;
            default:
                break;
        }
        isActDone[nowDanceActCount] = true;
        nowDanceActCount ++;
        // Debug.Log("nowDanceActCount = " + nowDanceActCount + danceActions[nowDanceActCount]);
    }
    public void SingANote()
    {
        if(UIO.GetIsVisible())
        {
            GameObject note = ObjectPool.Instance.GetObject(RollANote());
            note.transform.position = noteBornPoint.position;
            // note.transform.parent = transform.parent;
            note.transform.SetParent(transform.parent , true);
        }
        else
        {
            GameObject note = ObjectPool.Instance.GetObject(RollANote());
            note.transform.position = noteBornPoint.position;
            note.transform.SetParent(transform.parent , true);
            note.GetComponent<Image>().color = UIO.GetInVisibleColor();
        }
    }
    private GameObject RollANote()
    {
        switch(Random.Range(1 , 4))
        {
            case 1:
                return notePrefab1;
            case 2:
                return notePrefab2;
            case 3:
                return notePrefab3;
            // case 4:
            //     return notePrefab4;
            // case 5:
            //     return notePrefab5;
            // case 6:
            //     return notePrefab6;
            // case 7:
            //     return notePrefab7;
            default:
                return notePrefab1;
            
        }
    }
    private void PushColouredRibbon(int ribbonCount)
    {   
        if(UIO.GetIsVisible())
        {
            GameObject colouredRibbon = ObjectPool.Instance.GetObject(RollColouredRibbon());
            colouredRibbon.transform.position = ribbonBornPoint.position;
            colouredRibbon.transform.SetParent(transform.parent , true);
            for(int i = ribbonCount - 1 ; i > 0 ; i --)
            {
                colouredRibbon = ObjectPool.Instance.GetObject(RollColouredRibbon());
                colouredRibbon.transform.position = ribbonBornPoint.position;
                // colouredRibbon.transform.parent = transform.parent;
                colouredRibbon.transform.SetParent(transform.parent , true);
            }
        }
        else
        {
            Color inVisibleColor = UIO.GetInVisibleColor();
            GameObject colouredRibbon = ObjectPool.Instance.GetObject(RollColouredRibbon());
            colouredRibbon.transform.position = ribbonBornPoint.position;
            colouredRibbon.transform.SetParent(transform.parent , true);
            colouredRibbon.GetComponent<Image>().color = inVisibleColor;
            for(int i = ribbonCount - 1 ; i > 0 ; i --)
            {
                colouredRibbon = ObjectPool.Instance.GetObject(RollColouredRibbon());
                colouredRibbon.transform.position = ribbonBornPoint.position;
                // colouredRibbon.transform.parent = transform.parent;
                colouredRibbon.transform.SetParent(transform.parent , true);
                colouredRibbon.GetComponent<Image>().color = inVisibleColor;
            }
        }
        
    }
    private GameObject RollColouredRibbon()
    {
        switch(Random.Range(1 , 4))
        {
            case 1:
                return colouredRibbonPrefab1;
            case 2:
                return colouredRibbonPrefab2;
            case 3:
                return colouredRibbonPrefab3;
            default:
                return colouredRibbonPrefab1;
            
        }
    }

    public void SetVisible()
    {
        isVisible = true;
    }
    public void SetInvisible()
    {
        isVisible = false;
    }
    public void SetNowBCount(float bpm)
    {
        nowBCount = bpm;
    }
    public void SetNowDanceActCount(float BCount)
    {
        nowBCount = BCount;
        nowDanceActCount = 0;
        for (int i = isActDone.Length - 1; i >= 0; i--)
        {
            isActDone[i] = false;
        }
        while(danceActionBPMCount[nowDanceActCount] < BCount)
        {
            isActDone[nowDanceActCount] = true;
            nowDanceActCount ++ ;
        }
    }

}

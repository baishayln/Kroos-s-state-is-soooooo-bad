using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum TypeOfUI
{
    StageChoise , MainMenu , Setting , ExitGame
}
public class StartScenceController : MonoBehaviour
{
    [SerializeField]public float titleShowWaitTime = 1;
    private float timer;
    private bool isTitle;
    private Animator animator;
    private bool canClickButton = false;
    [SerializeField]public GameObject loadScreen;
    [SerializeField]public Slider loadSlider;
    [SerializeField]public Text loadingText;
    private TypeOfUI nowUI;
    private TypeOfUI nextUI;
    private bool isChangeUI = false;
    private bool isSwitchNextUI = false;
    private bool changingUI = false;
    private AnimatorStateInfo info;
    Dictionary<TypeOfUI , GameObject> mainMenuDic = new Dictionary<TypeOfUI, GameObject>();
    [SerializeField]public GameObject title;
    [SerializeField]public GameObject mainMenu;
    [SerializeField]public GameObject stageChoise;
    [SerializeField]public GameObject setting;
    private bool isShowSetting = false;
    private Color UIColor;
    [SerializeField]private AudioClip uiEffect;
    [SerializeField]private AudioClip BGM;
    //      UI适配部分，被Unity自带功能替换
    // [SerializeField]private float basicScreenX = 811;
    // [SerializeField]private float basicScreenY = 456;
    // private float nowScreenX;
    // private float nowScreenY;
    

    // Start is called before the first frame update
    void Start()
    {
        timer = titleShowWaitTime;
        isTitle = false;
        animator = transform.GetComponent<Animator>();
        if(!loadScreen)
        {
            loadScreen = transform.GetChild(2).gameObject;
        }
        if(!loadSlider)
        {
            loadSlider = transform.GetChild(2).GetChild(2).GetComponent<Slider>();
        }
        if(!loadingText)
        {
            loadingText = transform.GetChild(2).GetChild(1).GetComponent<Text>();
        }
        if(!title)
            title = transform.GetChild(0).gameObject;
        if(!mainMenu)
            mainMenu = transform.GetChild(1).gameObject;
        if(!stageChoise)
            stageChoise = transform.GetChild(3).gameObject;
        if(!setting)
            setting = transform.GetChild(4).gameObject;
        
        
        // nowScreenX = basicScreenX;
        // nowScreenY = basicScreenY;
        // float ROfcameraX = (Camera.main.ViewportToScreenPoint(Vector3.one).x - Camera.main.ViewportToScreenPoint(Vector3.zero).x);
        // float ROfcameraY = (Camera.main.ViewportToScreenPoint(Vector3.one).y - Camera.main.ViewportToScreenPoint(Vector3.zero).y);
        // if (true)
        // {
            
        // }
    }
    //按钮的bool，switch传入和一开始的nowUI
    //每个按钮可触发的函数都需要判断当前是否正在切换UI
    //取消UI的最后触发函数讲当前物体置为false，显示UI的动画的开始将物体置为true？还是通过代码控制？
    // Update is called once per frame
    void OnEnable()
    {
        timer = titleShowWaitTime;
        isTitle = false;
    }
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if(!isTitle)
            {
                isTitle = true;
                animator.Play("StartTitleShow");
            }
        }
        // if(isChangeUI)
        // {
        //     info = animator.GetCurrentAnimatorStateInfo(0);
        //     if(info.normalizedTime >= 0.98f)
        //     {
        //         // animator.Play("");
        //         // 每一帧里都用代码改变当前UI和UI的子物体的颜色？   //比较复杂，作为一个小项目不太现实，需要遍历，并且开销较大
        //         nowUI = nextUI;
        //     }
        //     // NowUIColorDown();
        // }
        // if(isSwitchNextUI)
        // {
        //     // NowUIColorUp();
        // }
    }
    
    // IEnumerator NowUIColorDown(GameObject UI)
    // {
    //     if(title.GetComponent<Image>())
    //     {
    //         UIColor = title.GetComponent<Image>().color;
    //         title.GetComponent<Image>().color = new Color(UIColor.r , UIColor.g , UIColor.b , UIColor.a - Time.deltaTime);
    //     }
    //     if(title.GetComponent<Text>())
    //     {
    //         UIColor = title.GetComponent<Image>().color;
    //         title.GetComponent<Image>().color = new Color(UIColor.r , UIColor.g , UIColor.b , UIColor.a - Time.deltaTime);
    //     }
    //     for(int i = 0 ; i < title.transform.childCount ; i++)
    //     {
    //         if(title.transform.GetChild(i).GetComponent<Image>())
    //         {
    //             UIColor = title.GetComponent<Image>().color;
    //             title.GetComponent<Image>().color = new Color(UIColor.r , UIColor.g , UIColor.b , UIColor.a - Time.deltaTime);
    //         }
    //     }
    // }
    // private void NowUIColorDown()
    // {
    //     if(nowUI == TypeOfUI.MainMenu)
    //     {
    //         if(title.GetComponent<Image>())
    //         {
    //             UIColor = title.GetComponent<Image>().color;
    //             title.GetComponent<Image>().color = new Color(UIColor.r , UIColor.g , UIColor.b , UIColor.a - Time.deltaTime);
    //         }
    //         if(title.GetComponent<Text>())
    //         {
    //             UIColor = title.GetComponent<Image>().color;
    //             title.GetComponent<Image>().color = new Color(UIColor.r , UIColor.g , UIColor.b , UIColor.a - Time.deltaTime);
    //         }
    //         for(int i = 0 ; i < title.transform.childCount ; i++)
    //         {
    //             if(title.transform.GetChild(i).GetComponent<Image>())
    //             {
    //                 UIColor = title.GetComponent<Image>().color;
    //                 title.GetComponent<Image>().color = new Color(UIColor.r , UIColor.g , UIColor.b , UIColor.a - Time.deltaTime);
    //             }
    //         }
    //     }
    //     if(nowUI == TypeOfUI.StageChoise)
    //     {

    //     }
    //     if(nowUI == TypeOfUI.Setting)
    //     {

    //     }
    // }
    // private void NowUIColorUp()
    // {
        
    // }
    public void PlayDong()
    {
        if (uiEffect)
        {
            SoundManager.Instance.PlayEffectSound(uiEffect);
        }
    }
    public void PlayBGM()
    {
        if (BGM)
        {
            SoundManager.Instance.PlayMusicSound(BGM);
        }
    }
    public void PlayAudio()
    {

    }
    public void TitleToStageChoise()
    {

    }
    public void StageChoiseToTitle()
    {
        
    }
    public void PublicSwitchUI(int nextUI)
    {
        if(!changingUI && canClickButton)
        {
            // changingUI = true;   //当UI的动态渐入渐出效果启用时才启用这个bool值，用于在切换过程中禁用按钮
            switch (nextUI)
            {
                case 1:
                    SwitchUI(TypeOfUI.MainMenu);
                    break;
                case 2:
                    SwitchUI(TypeOfUI.StageChoise);
                    break;
                case 3:
                    SwitchUI(TypeOfUI.Setting);
                    break;
                default:
                    SwitchUI(TypeOfUI.MainMenu);
                    break;
            }
        }
        //1：初始界面；2：关卡选择界面；3：设置界面；
        // switch (nextUI)
        // {
        //     case 1:
        //         SwitchUI(TypeOfUI.MainMenu);
        //         break;
        //     case 2:
        //         SwitchUI(TypeOfUI.StageChoise);
        //         break;
        //     case 3:
        //         SwitchUI(TypeOfUI.Setting);
        //         break;
        //     default:
        //         SwitchUI(TypeOfUI.MainMenu);
        //         break;
        // }
    }
    private void SwitchUI(TypeOfUI nextUI)
    {
        // this.nextUI = nextUI;
        // isChangeUI = true;
        // isSwitchNextUI = false;
        mainMenu.SetActive(false);
        title.SetActive(false);
        stageChoise.SetActive(false);
        setting.SetActive(false);
        if(nextUI == TypeOfUI.MainMenu)
        {
            mainMenu.SetActive(true);
            title.SetActive(true);
        }
        if(nextUI == TypeOfUI.StageChoise)
        {
            stageChoise.SetActive(true);
        }
        if(nextUI == TypeOfUI.Setting)
        {
            setting.SetActive(true);
        }
    }
    public void ChangeUIComplete()
    {
        changingUI = false;
    }
    private void OpenStageChoise()
    {
        animator.Play("OpenStageChoise");
    }
    private void CloseStageChoise()
    {
        animator.Play("CloseStageChoise");
    }
    private void OpenStartTitle()
    {
        animator.Play("OpenStartTitle");
    }
    private void CloseStartTitle()
    {
        animator.Play("CloseStartTitle");
    }
    private void OpenSetting()
    {
        animator.Play("OpenSetting");
    }
    private void CloseSetting()
    {
        animator.Play("CloseSetting");
    }
    public void CanClickButton()
    {
        canClickButton = true;
        // nowUI = TypeOfUI.MainMenu;
    }
    public void LoadStage(int stageNum)
    {
        if(canClickButton)
            StartCoroutine(LoadScene(stageNum));
    }
    IEnumerator LoadScene(int stageNum)
    {
        mainMenu.SetActive(false);
        title.SetActive(false);
        stageChoise.SetActive(false);
        setting.SetActive(false);
        loadScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(stageNum);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            loadSlider.value = operation.progress;
            if(operation.progress < 0.9f)
            {
                loadingText.text = (operation.progress * 100).ToString("f0") + "%";
            }
            else
            {
                loadSlider.value = 1;
                loadingText.text = "请按任意键继续~";
                if(Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }
    public void ShowSetting()
    {
        if (!isShowSetting)
        {
            title.SetActive(false);
            setting.SetActive(true);
            isShowSetting = true;
        }
        else
        {
            QuitSetting();
        }
    }
    public void QuitSetting()
    {
        setting.SetActive(false);
        title.SetActive(true);
        isShowSetting = false;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

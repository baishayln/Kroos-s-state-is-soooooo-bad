using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum TypeOfUI
{
    StageChoise , MapChoise , CharacterChoise , MainMenu , Setting , ExitGame , SavePage , Record , CharacterIntroduce , EnemyDictionary , BossChoise
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
    [SerializeField]public GameObject characterChoise;
    [SerializeField]public GameObject enemyDictionary;
    [SerializeField]public GameObject characterIntroduce;
    [SerializeField]public GameObject record;
    [SerializeField]public GameObject BossChoise;
    [SerializeField]public GameObject mapChoise;

    private bool isShowSetting = false;
    private Color UIColor;
    [SerializeField]private AudioClip uiEffect;
    [SerializeField]private AudioClip BGM;
    [SerializeField]private UIOrnamentController UIOrnamentController;
    // private int characterNum;
    private GameObject character;
    [SerializeField]private GameObject character1;
    [SerializeField]private GameObject character2;
    [SerializeField]private GameObject Boss1;
    [SerializeField]private GameObject Boss2;

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
        if(!characterChoise)
            characterChoise = transform.GetChild(5).gameObject;
        
        
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
        SoundManager.Instance.PlayBGM1();
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
    }
    public void PlayDong()
    {
        if (uiEffect)
        {
            SoundManager.Instance.PlayEffectSound(uiEffect);
        }
    }
    public void PlayBGM()
    {
        // if (BGM)
        // {
        //     SoundManager.Instance.PlayMusicSound(BGM);
        // }
        SoundManager.Instance.PlayBGM1();
        SoundManager.Instance.ContinueMusicSound();
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
    public void ChoiseCharacter(int characterNum)
    {
        switch(characterNum)
        {
            case 1:
                // character = character1;
                GameController.Instance.SetCharacter(character1);
                break;
            case 2:
                // character = character2;
                GameController.Instance.SetCharacter(character2);
                break;
            default:
                break;
        }
    }
    public void ChoiseBoss(int BossNum)
    {
        switch(BossNum)
        {
            case 1:
                // character = character1;
                GameController.Instance.SetBoss(Boss1);
                GameController.Instance.SetMapNum(1);
                break;
            case 2:
                // character = character2;
                GameController.Instance.SetBoss(Boss2);
                GameController.Instance.SetMapNum(2);
                break;
            default:
                break;
        }
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
                case 4:
                    SwitchUI(TypeOfUI.CharacterChoise);
                    break;
                case 5:
                    SwitchUI(TypeOfUI.EnemyDictionary);
                    break;
                case 6:
                    SwitchUI(TypeOfUI.CharacterIntroduce);
                    break;
                case 7:
                    SwitchUI(TypeOfUI.Record);
                    break;
                case 8:
                    SwitchUI(TypeOfUI.BossChoise);
                    break;
                case 9:
                    SwitchUI(TypeOfUI.MapChoise);
                    break;
                default:
                    SwitchUI(TypeOfUI.MainMenu);
                    break;
            }
        }
    }
    private void SwitchUI(TypeOfUI nextUI)
    {
        mainMenu.SetActive(false);
        title.SetActive(false);
        stageChoise.SetActive(false);
        setting.SetActive(false);
        characterChoise.SetActive(false);
        record.SetActive(false);
        characterIntroduce.SetActive(false);
        enemyDictionary.SetActive(false);
        BossChoise.SetActive(false);
        mapChoise.SetActive(false);
        if(nextUI == TypeOfUI.MainMenu)
        {
            mainMenu.SetActive(true);
            title.SetActive(true);
            isShowSetting = false;
            UIOrnamentController.SetVisible();
        }
        if(nextUI == TypeOfUI.StageChoise)
        {
            stageChoise.SetActive(true);
            isShowSetting = false;
            UIOrnamentController.SetInvisible();
        }
        if(nextUI == TypeOfUI.CharacterChoise)
        {
            characterChoise.SetActive(true);
            isShowSetting = false;
            UIOrnamentController.SetInvisible();
        }
        if(nextUI == TypeOfUI.Setting)
        {
            if (!isShowSetting)
            {
                mainMenu.SetActive(true);
                title.SetActive(false);
                setting.SetActive(true);
                isShowSetting = true;
                UIOrnamentController.SetInvisible();
            }
            else
            {
                QuitSetting();
            }
        }
        if(nextUI == TypeOfUI.EnemyDictionary)
        {
            enemyDictionary.SetActive(true);
            UIOrnamentController.SetInvisible();
        }
        if(nextUI == TypeOfUI.CharacterIntroduce)
        {
            characterIntroduce.SetActive(true);
            UIOrnamentController.SetInvisible();
        }
        if(nextUI == TypeOfUI.Record)
        {
            record.SetActive(true);
            UIOrnamentController.SetInvisible();
        }
        if(nextUI == TypeOfUI.BossChoise)
        {
            BossChoise.SetActive(true);
            UIOrnamentController.SetInvisible();
        }
        if(nextUI == TypeOfUI.MapChoise)
        {
            mapChoise.SetActive(true);
            UIOrnamentController.SetInvisible();
        }
    }
    public void ReturnToPreviousLevel(GameObject menu)
    {
        mainMenu.SetActive(false);
        title.SetActive(false);
        stageChoise.SetActive(false);
        setting.SetActive(false);
        characterChoise.SetActive(false);
        record.SetActive(false);
        characterIntroduce.SetActive(false);
        enemyDictionary.SetActive(false);
        
        menu.SetActive(true);
    }
    public void ReturnToMainMenu()
    {
        stageChoise.SetActive(false);
        setting.SetActive(false);
        characterChoise.SetActive(false);
        record.SetActive(false);
        characterIntroduce.SetActive(false);
        enemyDictionary.SetActive(false);

        mainMenu.SetActive(true);
        title.SetActive(true);
        isShowSetting = false;
        UIOrnamentController.SetVisible();
    }
    public void ShowUIOrnament()
    {
        UIOrnamentController.ShowUIOrnament();
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
    public void CantClickButton()
    {
        canClickButton = false;
        // nowUI = TypeOfUI.MainMenu;
    }
    public void SetKroossState(int num)
    {
        GameController.Instance.SetKroossState(num);
    }
    public void LoadStage()
    {
        if(canClickButton)
            StartCoroutine(LoadScene(GameController.Instance.GetMapNum()));
    }
    IEnumerator LoadScene(int stageNum)
    {
        // SetKroossState(stageNum - 1);
        mainMenu.SetActive(false);
        title.SetActive(false);
        stageChoise.SetActive(false);
        setting.SetActive(false);
        loadScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(stageNum);
        operation.allowSceneActivation = false;
        SoundManager.Instance.TurnDownMusic();
        // SceneManager.MoveGameObjectToScene(player,SceneManager.GetSceneByBuildIndex(newScene));
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
                    SoundManager.Instance.StopMusicSound();
                    SoundManager.Instance.TurnUpMusic();
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
        mainMenu.SetActive(true);
        title.SetActive(true);
        isShowSetting = false;
        UIOrnamentController.SetVisible();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

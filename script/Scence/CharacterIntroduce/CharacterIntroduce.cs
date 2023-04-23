using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterIntroduce : MonoBehaviour
{
    public float animationProgress = 0;
    private float lastAnimationProgress = 0;
    private Transform button;
    [SerializeField]private Transform targetPoint;
    [SerializeField]private Transform introducePage;
    private Vector3 returnPoint;
    private bool isOpenIntroduce;
    private Color introduceBGAlpha = Vector4.one;
    private Color buttonDescribeColor;
    [SerializeField]private Image introduceBG;
    [SerializeField]private Text characterName;
    [SerializeField]private Text characterIntroduce;
    [SerializeField]private Text buttonDescribe;
    [SerializeField]private Transform characters;

    private Animator animator;
    private Color textColor;

    void Awake()
    {
        animator = transform.GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if ((animationProgress != 0 && animationProgress != 1) || ((animationProgress == 0 || animationProgress == 1) && animationProgress != lastAnimationProgress))
        {
            AnimationOnPlay();
            lastAnimationProgress = animationProgress;
        }
    }
    public void OpenIntroduce(Transform button)
    {
        if (!isOpenIntroduce)
        {
            if (animationProgress == 0 || animationProgress == 1)
            {
                isOpenIntroduce = true;
                IntroduceInformation();
                returnPoint = button.position;
                this.button = button;
                buttonDescribe = button.GetComponent<StageShowDescribe>().GetDescribe().GetComponent<Text>();
                buttonDescribeColor = buttonDescribe.color;
                transform.parent.GetComponent<StartScenceController>().CantClickButton();
                animator.Play("EnterIntroduce");
                StartAnimation();
                if (characterName)
                {
                    characterName.text = button.GetComponent<DirctionaryContent>().GetCharacterName();
                }
                if (characterIntroduce)
                {
                    characterIntroduce.text = button.GetComponent<DirctionaryContent>().GetCharacterDescribe();
                }
                textColor = button.GetComponent<DirctionaryContent>().GetCharacterColor();
                introduceBGAlpha = introduceBG.color;
                introduceBG.color = Color.clear;
                if (introducePage)
                {
                    characterName.color = Color.clear;
                }
                if (characterIntroduce)
                {
                    characterIntroduce.color = Color.clear;
                }
            }
        }
        else
        {
            CloseIntroduce(button);
        }
    }
    public void CloseIntroduce(Transform button)
    {
        if (isOpenIntroduce && button == this.button)
        {
            if (animationProgress == 0 || animationProgress == 1)
            {
                isOpenIntroduce = false;
                transform.parent.GetComponent<StartScenceController>().CanClickButton();
                animator.Play("ExitIntroduce");
            }
        }
    }
    public void CloseIntroduce()
    {
        if (animationProgress == 0 || animationProgress == 1)
        {
            isOpenIntroduce = false;
            transform.parent.GetComponent<StartScenceController>().CanClickButton();
            animator.Play("ExitIntroduce");
        }
    }
    private void StartAnimation()
    {
        button.SetParent(introducePage);
    }
    public void EndAnimation()
    {
        button.SetParent(characters);
    }
    private void IntroduceInformation()
    {
        Debug.Log("显示了角色的名字和角色的简介内容。");
    }
    private void AnimationOnPlay()
    {
        button.position = Vector3.Lerp(returnPoint , targetPoint.position , animationProgress);
        introduceBGAlpha.a = animationProgress;
        introduceBG.color = introduceBGAlpha;
        textColor.a = animationProgress;
        if (introducePage)
        {
            characterName.color = textColor;
        }
        if (characterIntroduce)
        {
            characterIntroduce.color = textColor;
        }
        buttonDescribeColor.a = 1 - animationProgress;
        buttonDescribe.color = buttonDescribeColor;
    }
}

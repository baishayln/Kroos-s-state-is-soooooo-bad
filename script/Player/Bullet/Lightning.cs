using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField]private Animator animator;
    private AnimatorStateInfo info;
    private AudioClip effect;
    private LightingArea attackArea;
    private Transform target;
    private GameObject nowTarget;
    private LightingArea lightingArea;
    // Start is called before the first frame update
    void Start()
    {
        if (!animator)
        {
            animator = transform.GetComponent<Animator>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (animator)
        {
            info = animator.GetCurrentAnimatorStateInfo(0);
            if(info.normalizedTime >= 0.95f)
            {
                target = null;
                lightingArea = null;
                ObjectPool.Instance.PushObject(gameObject);
            }
        }
    }
    private void Attack()
    {
        if (target && lightingArea)
        {
            lightingArea.LightAttack(target);
        }
        if (effect)
        {
            SoundManager.Instance.PlayEffectSound(effect);
        }
        //播放音频，调用闪电链
    }
    public void SetData(Transform tgt , LightingArea lightingarea)
    {
        target = tgt;
        lightingArea = lightingarea;
    }
}

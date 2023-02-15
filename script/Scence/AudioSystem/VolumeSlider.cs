using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField]private Slider slider;
    [SerializeField]private AudioPlayers audioPlayerType;
    [SerializeField]private bool isInMainTitleSetting = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!slider)
        {
            slider = transform.GetComponent<Slider>();
        }
        if (audioPlayerType == AudioPlayers.musicPlayer)
        {
            if (isInMainTitleSetting)
            {
                SoundManager.Instance.ChangeMusicVolume(slider.value);
            }
            else
            {
                slider.value = SoundManager.Instance.ReturnMusicVolume();
            }
            slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMusicVolume(val));
        }
        else if (audioPlayerType == AudioPlayers.effectPlayer)
        {
            if (isInMainTitleSetting)
            {
                SoundManager.Instance.ChangeEffectVolume(slider.value);
            }
            else
            {
                slider.value = SoundManager.Instance.ReturnEffectVolume();
            }
            slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeEffectVolume(val));
        }
        else if (audioPlayerType == AudioPlayers.voicePlayer)
        {
            if (isInMainTitleSetting)
            {
                SoundManager.Instance.ChangeVoiceVolume(slider.value);
            }
            else
            {
                slider.value = SoundManager.Instance.ReturnVoiceVolume();
            }
            slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeVoiceVolume(val));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField]private Slider slider;
    [SerializeField]private AudioPlayers audioPlayerType;
    // Start is called before the first frame update
    void Start()
    {
        if (!slider)
        {
            slider = transform.GetComponent<Slider>();
        }
        if (audioPlayerType == AudioPlayers.musicPlayer)
        {
            SoundManager.Instance.ChangeMusicVolume(slider.value);
            slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeMusicVolume(val));
        }
        else if (audioPlayerType == AudioPlayers.effectPlayer)
        {
            SoundManager.Instance.ChangeEffectVolume(slider.value);
            slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeEffectVolume(val));
        }
        else if (audioPlayerType == AudioPlayers.voicePlayer)
        {
            SoundManager.Instance.ChangeVoiceVolume(slider.value);
            slider.onValueChanged.AddListener(val => SoundManager.Instance.ChangeVoiceVolume(val));
        }
    }
}

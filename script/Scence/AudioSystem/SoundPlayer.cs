using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField]private AudioPlayers audioPlayerType;
    [SerializeField]private AudioClip soundNeedPlay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void PlaySound()
    {
        if (audioPlayerType == AudioPlayers.musicPlayer)
        {
            SoundManager.Instance.PlayMusicSound(soundNeedPlay);
        }
        else if (audioPlayerType == AudioPlayers.effectPlayer)
        {
            SoundManager.Instance.PlayEffectSound(soundNeedPlay);
        }
        else if (audioPlayerType == AudioPlayers.voicePlayer)
        {
            SoundManager.Instance.PlayVoiceSound(soundNeedPlay);
        }
    }
}

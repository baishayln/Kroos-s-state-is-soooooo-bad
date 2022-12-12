using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioPlayers
{
    musicPlayer , effectPlayer , voicePlayer
}
public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new SoundManager();
            }
            return instance;
        }
    }
    [SerializeField]private AudioSource musicPlayer;
    [SerializeField]private AudioSource effectPlayer;
    [SerializeField]private AudioSource voicePlayer;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    public void PlayMusicSound(AudioClip clip)
    {
        musicPlayer.PlayOneShot(clip);
    }
    public void PlayEffectSound(AudioClip clip)
    {
        effectPlayer.PlayOneShot(clip);
    }
    public void PlayVoiceSound(AudioClip clip)
    {
        voicePlayer.PlayOneShot(clip);
    }

    public void PauseMusicSound(AudioClip clip)
    {
        musicPlayer.Pause();
    }
    public void PauseEffectSound(AudioClip clip)
    {
        effectPlayer.Pause();
    }
    public void PauseVoiceSound(AudioClip clip)
    {
        voicePlayer.Pause();
    }

    public void ChangeMusicVolume(float value)
    {
        musicPlayer.volume = value;
    }
    public void ChangeEffectVolume(float value)
    {
        effectPlayer.volume = value;
    }
    public void ChangeVoiceVolume(float value)
    {
        voicePlayer.volume = value;
    }

    public void MuteMusicSound()
    {
        if (!musicPlayer.mute)
        {
            musicPlayer.Pause();
        }
        musicPlayer.mute = !musicPlayer.mute;
    }
    public void MuteEffectSound()
    {
        if (!effectPlayer.mute)
        {
            effectPlayer.Pause();
        }
        effectPlayer.mute = !effectPlayer.mute;
    }
    public void MuteVoiceSound()
    {
        if (!voicePlayer.mute)
        {
            voicePlayer.Pause();
        }
        voicePlayer.mute = !voicePlayer.mute;
    }
}

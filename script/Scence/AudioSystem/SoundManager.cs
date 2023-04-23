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
    private float musicVolumeRecord;
    private float effectVolumeRecord;
    private float voiceVolumeRecord;
    [SerializeField]private AudioClip BGM1;
    [SerializeField]private AudioClip BGM2;
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
    public void PlayBGM1()
    {
        musicPlayer.clip = BGM1;
        musicPlayer.time = 0;
        ChangeMusicVolume(1);
    }
    public void PlayBGM2()
    {
        musicPlayer.clip = BGM2;
        musicPlayer.time = 0;
        ChangeMusicVolume(1);
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

    public void PauseMusicSound()
    {
        musicPlayer.Pause();
    }
    public void UnPauseMusicSound()
    {
        musicPlayer.UnPause();
    }
    public void PauseEffectSound()
    {
        effectPlayer.Pause();
    }
    public void PauseVoiceSound()
    {
        voicePlayer.Pause();
    }
    public void StopMusicSound()
    {
        musicPlayer.Stop();
    }
    public void StopEffectSound()
    {
        effectPlayer.Stop();
    }
    public void StopVoiceSound()
    {
        voicePlayer.Stop();
    }
    public void ContinueMusicSound()
    {
        musicPlayer.Play();
    }
    public void ContinueEffectSound()
    {
        effectPlayer.Play();
    }
    public void ContinueVoiceSound()
    {
        voicePlayer.Play();
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
    public float ReturnMusicVolume()
    {
        return musicPlayer.volume;
    }
    public float ReturnEffectVolume()
    {
        return effectPlayer.volume;
    }
    public float ReturnVoiceVolume()
    {
        return voicePlayer.volume;
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
    public float GetBGMPlayTime()
    {
        return musicPlayer.time;
    }
    public void TurnDownMusic()
    {
        StartCoroutine(DownMusic());
    }
    public void TurnDownEffect()
    {
        StartCoroutine(DownEffect());
    }
    public void TurnDownVoice()
    {
        StartCoroutine(DownVoice());
    }
    public void TurnUpMusic()
    {
        StartCoroutine(RecoveryMusic());
    }
    public void TurnUpEffect()
    {
        StartCoroutine(RecoveryEffect());
    }
    public void TurnUpVoice()
    {
        StartCoroutine(RecoveryVoice());
    }
    IEnumerator DownMusic()
    {
        float timer = 0;
        musicVolumeRecord = musicPlayer.volume;
        while (musicPlayer.volume > 0 && timer < 2)
        {
            timer += Time.deltaTime;
            musicPlayer.volume -= Time.deltaTime;
            yield return null;
        }
        musicPlayer.time = 0;
        StopMusicSound();
    }
    IEnumerator DownEffect()
    {
        float timer = 0;
        effectVolumeRecord = effectPlayer.volume;
        while (effectPlayer.volume > 0 && timer < 2)
        {
            timer += Time.deltaTime;
            effectPlayer.volume -= Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator DownVoice()
    {
        float timer = 0;
        voiceVolumeRecord = voicePlayer.volume;
        while (voicePlayer.volume > 0 && timer < 2)
        {
            timer += Time.deltaTime;
            voicePlayer.volume -= Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator RecoveryMusic()
    {
        float timer = 0;
        while (musicPlayer.volume < musicVolumeRecord && timer < 3)
        {
            timer += Time.deltaTime;
            musicPlayer.volume += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator RecoveryEffect()
    {
        float timer = 0;
        while (effectPlayer.volume < effectVolumeRecord && timer < 3)
        {
            timer += Time.deltaTime;
            effectPlayer.volume += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator RecoveryVoice()
    {
        float timer = 0;
        while (voicePlayer.volume < voiceVolumeRecord && timer < 3)
        {
            timer += Time.deltaTime;
            voicePlayer.volume += Time.deltaTime;
            yield return null;
        }
    }
}

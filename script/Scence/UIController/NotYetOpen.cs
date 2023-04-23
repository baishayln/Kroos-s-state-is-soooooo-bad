using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotYetOpen : MonoBehaviour
{
    [SerializeField]private AudioClip sound;
    // Start is called before the first frame update
    public void PlaySound()
    {
        SoundManager.Instance.PlayEffectSound(sound);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioSource;
    [SerializeField]public float startTime = 110f;
    void Start()
    {
        audioSource = transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // audioSource.PlayScheduled(startTime);
            audioSource.time = startTime;
            audioSource.Play();
            Debug.Log(1);
            float BCount = startTime/(60f/118f);
            GameObject.Find("Kroos").GetComponent<DancePlayer>().SetNowDanceActCount(BCount);
            GameObject.Find("Myrtle").GetComponent<DancePlayer>().SetNowDanceActCount(BCount);
            GameObject.Find("Durin").GetComponent<DancePlayer>().SetNowDanceActCount(BCount);
        }
    }
}

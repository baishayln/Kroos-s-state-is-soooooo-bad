using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmTimingSystem : MonoBehaviour
{
    [SerializeField]private int BPM = 118;
    private float Btime;
    private float BGMPlayTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        Btime = 60/BPM;
    }

    // Update is called once per frame
    void Update()
    {
        // BGMPlayTime = SoundManager.Instance.GetBGMPlayTime();
    }
}

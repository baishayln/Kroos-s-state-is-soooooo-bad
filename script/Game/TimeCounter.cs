using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    private GameObject[] objects;
    private float[] times;
    void Update()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            times[i] += Time.deltaTime;
        }
    }
    public void StartTiming(GameObject obj)
    {
        objects[objects.Length] = obj;
        times[times.Length] = 0;
    }
    // public float EndTiming()
    // {
    //     isTiming = false;
    //     return timer;
    // }
}

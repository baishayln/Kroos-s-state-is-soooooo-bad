using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginnerTutorial : MonoBehaviour
{
    [SerializeField]private GameObject beginnerTutorialPage;
    public void Play()
    {
        if (PlayerPrefs.GetString("FirstStart") != "false")
        {
            PlayerPrefs.SetString("FirstStart" , "false");
            beginnerTutorialPage.SetActive(true);
        }
    }
}

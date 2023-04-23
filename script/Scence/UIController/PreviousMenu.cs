using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviousMenu : MonoBehaviour
{
    [SerializeField]private GameObject fatherMenu;
    public GameObject ReturnPreviou()
    {
        return fatherMenu;
    }
    
    public void ReturnToPreviouLevel()
    {
        transform.GetComponentInParent<StartScenceController>().ReturnToPreviousLevel(fatherMenu);
    }
    public void ReturnToMainMenu()
    {
        transform.GetComponentInParent<StartScenceController>().ReturnToMainMenu();
    }
}

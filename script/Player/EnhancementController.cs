using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancementController
{
    [SerializeField]public GameObject Enhancement1;
    [SerializeField]public GameObject Enhancement2;
    [SerializeField]public GameObject Enhancement3;
    [SerializeField]public GameObject Enhancement4;
    [SerializeField]public GameObject Enhancement5;
    [SerializeField]public GameObject Enhancement6;
    [SerializeField]public GameObject Enhancement7;
    public GameObject CreateEnhancement()
    {
        switch (Random.Range(0 , 7))
        {
            case 0:
                return Enhancement1;
            case 1:
                return Enhancement2;
            case 2:
                return Enhancement3;
            case 3:
                return Enhancement4;
            case 4:
                return Enhancement5;
            case 5:
                return Enhancement6;
            case 6:
                return Enhancement7;
            default:
                return null;
        }
        
    }
}

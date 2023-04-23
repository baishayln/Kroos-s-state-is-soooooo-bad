using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageShowDescribe : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]private GameObject describe;
    [SerializeField]private GameObject isThisUnlock;
    // Start is called before the first frame update
    void Start()
    {
        if (!describe)
        {
            describe = transform.GetChild(1).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isThisUnlock)
        {
            describe.SetActive(true);
        }
        else if (!isThisUnlock.activeInHierarchy)
        {
            describe.SetActive(true);
        }
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isThisUnlock)
        {
            describe.SetActive(false);
        }
        else if (!isThisUnlock.activeInHierarchy)
        {
            describe.SetActive(false);
        }
    }
    public GameObject GetDescribe()
    {
        return describe;
    }
}

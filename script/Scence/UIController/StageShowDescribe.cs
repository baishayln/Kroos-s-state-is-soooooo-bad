using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageShowDescribe : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]private GameObject describe;
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
        describe.SetActive(true);
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        describe.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IsThisUnlock : MonoBehaviour
{
    [SerializeField]private GameObject father;
    [SerializeField]private bool isUnlock;
    // [SerializeField]private bool canThisPurchase;
    void Start()
    {
        if (PlayerPrefs.GetString(transform.parent.name) == "true" || isUnlock)
        {
            PlayerPrefs.SetString(transform.parent.name , "true");
            gameObject.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetString(transform.parent.name , "false");
            gameObject.SetActive(true);
        }
        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        transform.GetComponent<RectTransform>().sizeDelta = father.GetComponent<RectTransform>().sizeDelta;
        // transform.GetComponent<RectTransform>().pivot = father.GetComponent<RectTransform>().pivot;
        transform.GetComponent<Image>().sprite = father.GetComponent<Image>().sprite;
    }
    public void Unlock()
    {
        PlayerPrefs.SetString(transform.parent.name , "true");
        gameObject.SetActive(false);
    }
}

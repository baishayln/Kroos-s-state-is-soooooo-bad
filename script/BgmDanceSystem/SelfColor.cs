using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfColor : MonoBehaviour
{
    private Color selfColor;
    // Start is called before the first frame update
    void Start()
    {
        if(transform.GetComponent<Image>())
        {
            selfColor = transform.GetComponent<Image>().color;
        }
        if(transform.GetComponent<SpriteRenderer>())
        {
            selfColor = transform.GetComponent<SpriteRenderer>().color;
        }
    }

    public void ColorCallback()
    {
        if(transform.GetComponent<Image>())
        {
            transform.GetComponent<Image>().color = selfColor;
        }
        if(transform.GetComponent<SpriteRenderer>())
        {
            transform.GetComponent<SpriteRenderer>().color = selfColor;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirctionaryContent : MonoBehaviour
{
    [SerializeField]private string characterName;
    [SerializeField]private string characterDescribe;
    private Color color;
    void Awake()
    {
        if (transform.GetComponent<Text>())
        {
            color = transform.GetComponentInChildren<Text>().color;
        }
    }
    public string GetCharacterName()
    {
        return characterName;
    }
    public string GetCharacterDescribe()
    {
        return characterDescribe;
    }
    public Color GetCharacterColor()
    {
        return color;
    }
}

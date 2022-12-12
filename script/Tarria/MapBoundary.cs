using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBoundary : MonoBehaviour
{
    private GameObject fightUI;
    private Text MapBoundaryTag;
    private bool isTouchingPlayer;
    private Color tagColor;
    // Start is called before the first frame update
    void Start()
    {
        fightUI = GameObject.Find("FightUI");
        MapBoundaryTag = GameObject.Find("MapBoundaryTag").GetComponent<Text>();
        tagColor = MapBoundaryTag.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouchingPlayer)
        {
            if (tagColor.a < 1)
            {
                tagColor.a += Time.deltaTime;
                MapBoundaryTag.color = tagColor;
            }
        }
        else
        {
            if (tagColor.a > 0)
            {
                tagColor.a -= Time.deltaTime;
                MapBoundaryTag.color = tagColor;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isTouchingPlayer = true;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }
}

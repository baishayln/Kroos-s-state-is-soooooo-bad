using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageInPlayerDash : MonoBehaviour
{
    // private float lifeTime;
    // private float lifeTimer;
    private Color color;
    [SerializeField]public float disappearSpeed;
    // Start is called before the first frame update
    void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // lifeTimer -= Time.deltaTime;
        color.a -=  Time.deltaTime * disappearSpeed;
        transform.GetComponent<SpriteRenderer>().color = color;
        if (color.a < 0)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
    public void SetDate(float disappearSpd , Sprite sprite , Vector2 position , float alpha)
    {
        disappearSpeed = disappearSpd;
        transform.position = position;
        transform.GetComponent<SpriteRenderer>().sprite = sprite;
        color = transform.GetComponent<SpriteRenderer>().color;
        color.a = alpha;
        transform.GetComponent<SpriteRenderer>().color = color;
    }
}

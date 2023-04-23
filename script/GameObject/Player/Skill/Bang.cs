using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bang : MonoBehaviour
{
    private Color color;
    private float timer;
    [SerializeField]public float lifeTime = 0.75f;
    private bool isActive = false;
    private TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetComponent<TextMeshPro>();
    }
    void OnEnable()
    {
        isActive = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                isActive = false;
            }
        }
        if (!isActive && color.a > 0)
        {
            color.a -= Time.deltaTime;
            text.color = color;
            if (color.a <= Time.deltaTime)
            {
                ObjectPool.Instance.PushObject(gameObject);
            }
            color.a -= Time.deltaTime;
        }
    }
    public void SetData(Vector2 position , Color color , float angle)
    {
        if (!text)
        {
            text = transform.GetComponent<TextMeshPro>();
        }
        text.color = color;
        this.color = color;
        transform.position = position;
        timer = lifeTime;
        isActive = true;
        transform.up = Vector3.up;
        transform.Rotate(new Vector3( 0 , 0 , angle));
    }
}

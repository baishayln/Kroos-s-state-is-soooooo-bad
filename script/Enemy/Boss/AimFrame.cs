using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimFrame : MonoBehaviour
{
    private float scaleTimer;
    private float lifeTime;
    private float lifeTimer;
    private Color color;
    private Vector2 scale = Vector2.one;
    // Start is called before the first frame update
    void Start()
    {
        color = transform.GetComponent<SpriteRenderer>().color;
    }
    void OnEnable()
    {
        color = transform.GetComponent<SpriteRenderer>().color;
        scaleTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // if (lifeTimer > 0)
        // {
        //     lifeTimer -= Time.deltaTime;
        // }
        // if (lifeTimer <= 0)
        // {
        //     if (color == null)
        //     {
        //         color = transform.GetComponent<SpriteRenderer>().color;
        //     }
        //     color.a -= Time.deltaTime * 3;
        //     transform.GetComponent<SpriteRenderer>().color = color;
        // }
        // if (color.a <= 0)
        // {
        //     ObjectPool.Instance.PushObject(gameObject);
        // }
        // scaleTimer += Time.deltaTime;

        // transform.localScale = scale + Mathf.Sin(scaleTimer * Quaternion.Euler(360 * scale).z) * scale * 0.2f;
    }
    public void SetData(float lifeTime , Transform player)
    {
        transform.position = player.position;
        transform.SetParent(player);
        // lifeTimer = lifeTime;
        // scaleTimer = 0;
        // color = Vector4.one;
        // transform.GetComponent<SpriteRenderer>().color = color;
    }
    public void PushObject()
    {
        ObjectPool.Instance.PushObject(gameObject);
    }
}

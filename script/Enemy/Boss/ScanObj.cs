using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanObj : MonoBehaviour
{
    [SerializeField]private Sprite image;
    private MissileLauncher launcher;
    private string layerMaskName;
    private float lifeTime = 3;
    private float lifeTimer;
    private Color color;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        color = transform.GetComponent<SpriteRenderer>().color;
        lifeTimer = 3;
    }
    void OnEnable()
    {
        color = Vector4.one;
        color = transform.GetComponent<SpriteRenderer>().color;
        transform.localScale = Vector3.one;
        player = null;
    }
    // Update is called once per frame
    void Update()
    {
        transform.localScale += Time.deltaTime * 200 * Vector3.one;
        if (lifeTimer > 0)
        {
            lifeTimer -= Time.deltaTime;
        }
        if (lifeTimer <= 0)
        {
            if (color == null)
            {
                color = transform.GetComponent<SpriteRenderer>().color;
            }
            color.a -= Time.deltaTime * 3;
            transform.GetComponent<SpriteRenderer>().color = color;
        }
        if (color.a <= 0)
        {
            launcher = null;
            lifeTimer = 3;
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
    public void SetData(string layerMaskName , MissileLauncher launcher)
    {
        this.launcher = launcher;
        this.layerMaskName = layerMaskName;
        transform.localScale = Vector3.one;
        lifeTimer = lifeTime;
        color = Vector4.one;
        transform.GetComponent<SpriteRenderer>().color = color;
        player = null;
    }
    public void SetData(string layerMaskName , MissileLauncher launcher , Vector3 position)
    {
        this.launcher = launcher;
        this.layerMaskName = layerMaskName;
        transform.position = position;
        transform.localScale = Vector3.one;
        lifeTimer = lifeTime;
        color = Vector4.one;
        transform.GetComponent<SpriteRenderer>().color = color;
        player = null;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && launcher != null && !player)
        {
            player = other.transform;
            launcher.LockPlayer(other.transform);
        }
    }
}

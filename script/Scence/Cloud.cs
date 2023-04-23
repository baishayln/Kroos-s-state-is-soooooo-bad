using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private float speed;
    [SerializeField]private Rigidbody2D rig;
    [SerializeField]private float speedDown = 1;
    [SerializeField]private float speedUp = 4;
    
    void Awake()
    {
        if (!rig)
        {
            rig = transform.GetComponent<Rigidbody2D>();
        }
    }
    void Start()
    {
        speed = Random.Range(speedDown , speedUp);
        rig.velocity = speed * Vector3.left;
    }

    void Update()
    {
        if(CameraBehaviour.Instance.ReturnCameraPosition().x - transform.position.x > CameraBehaviour.Instance.ReturnCameraX() * 0.75f)
        {
            transform.position = CameraBehaviour.Instance.ReturnCameraPosition() + CameraBehaviour.Instance.ReturnCameraX() * 0.75f * Vector3.right + CameraBehaviour.Instance.ReturnCameraY() * Random.Range(0.15f , 0.4f) * Vector3.up + 20 * Vector3.forward;
            transform.localScale = Random.Range(0.5f , 1.2f) * Vector3.one;
            speed = Random.Range(speedDown , speedUp);
            rig.velocity = speed * Vector3.left;
        }
    }
}

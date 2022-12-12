using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    private GameObject father;
    // Start is called before the first frame update
    void Start()
    {
        father = transform.parent.gameObject;
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            father = transform.parent.gameObject;
            father.GetComponent<UAVEnemyBehavior>().SetTarget(other.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            father = transform.parent.gameObject;
            father.GetComponent<UAVEnemyBehavior>().ReMoveTarget();
        }
    }
}

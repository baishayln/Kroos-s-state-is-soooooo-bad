using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    Transform point1;
    Transform point2;
    // Start is called before the first frame update
    void Start()
    {
        point1 = transform.GetChild(0);
        point2 = transform.GetChild(1);
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if(other.transform.GetComponent<PlayerMove>())
            {
                if (Vector2.Distance(point1.position , other.transform.position) < Vector2.Distance(point2.position , other.transform.position))
                {
                    other.transform.position = point1.position;
                }
                else
                {
                    other.transform.position = point2.position;
                }
            }
        }
    }
}

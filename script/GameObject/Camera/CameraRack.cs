using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRack : MonoBehaviour
{
    
    [SerializeField]public GameObject player;
    private float lastPlayerX;
    private float lastPlayerY;
    private float playerX;
    private float playerY;
    private float playerXMove;
    private float playerYMove;
    [SerializeField]private float dsts;
    [SerializeField]private float z = -22;
    [SerializeField]private bool isFollowPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        if (isFollowPlayer)
        {
            if(!player)
            {
                player = GameObject.Find("Player");
                transform.position = player.transform.position + dsts * Vector3.up;
            }
            lastPlayerX = player.transform.position.x;
            lastPlayerY = player.transform.position.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowPlayer)
        {
            if(!player)
            {
                player = GameObject.Find("Player");
                transform.position = player.transform.position + 1.5f * Vector3.up;
            }
            if(player)
            {
                playerX = player.transform.position.x;
                playerY = player.transform.position.y;
                playerXMove = playerX - lastPlayerX;
                playerYMove = playerY - lastPlayerY;
                transform.position = new Vector3(transform.position.x + playerXMove , transform.position.y + playerYMove , player.transform.position.z + z);
                lastPlayerX = playerX;
                lastPlayerY = playerY;
            }
        }
    }
}

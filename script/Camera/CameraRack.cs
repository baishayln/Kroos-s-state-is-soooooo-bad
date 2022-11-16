using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRack : MonoBehaviour
{
    
    private GameObject player;
    private float lastPlayerX;
    private float lastPlayerY;
    private float playerX;
    private float playerY;
    private float playerXMove;
    private float playerYMove;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player (1)");
        lastPlayerX = player.transform.position.x;
        lastPlayerY = player.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        playerX = player.transform.position.x;
        playerY = player.transform.position.y;
        playerXMove = playerX - lastPlayerX;
        playerYMove = playerY - lastPlayerY;
        transform.position = new Vector3(transform.position.x + playerXMove , transform.position.y + playerYMove , player.transform.position.z - 10);
        lastPlayerX = playerX;
        lastPlayerY = playerY;
    }
}

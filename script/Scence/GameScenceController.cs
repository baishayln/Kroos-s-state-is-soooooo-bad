using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScenceController : MonoBehaviour
{
    float x;
    [SerializeField]public float mousePointDsts = 16;
    [SerializeField]public Texture2D mouseTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.one;
    private Vector3 mousePositionOnScreen;//鼠标在屏幕的坐标
    private Vector3 mousePositionInWorld;//把鼠标的屏幕坐标转换为世界坐标
    private bool isMouseLeave = false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(mouseTexture , new Vector2(mousePointDsts , mousePointDsts) , CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        //获取鼠标在场景中坐标

        mousePositionOnScreen = Input.mousePosition;

        //将相机中的坐标转化为世界坐标

        mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionOnScreen);

        // if(!isMouseLeave && (mousePositionInWorld.x >= Camera.main.ViewportToWorldPoint(Vector3.one).x || mousePositionInWorld.y >= Camera.main.ViewportToWorldPoint(Vector3.one).y || mousePositionInWorld.x <= Camera.main.ViewportToWorldPoint(Vector3.zero).x || mousePositionInWorld.y <= Camera.main.ViewportToWorldPoint(Vector3.zero).y))
        // {
        //     isMouseLeave = true;
        // }
        if(!isMouseLeave && (Input.mousePosition.x > Screen.width || Input.mousePosition.x < 0 || Input.mousePosition.y > Screen.height || Input.mousePosition.y < 0))
        {
            isMouseLeave = true;
        }
        if(isMouseLeave && (Input.mousePosition.x < Screen.width && Input.mousePosition.x > 0 && Input.mousePosition.y < Screen.height && Input.mousePosition.y > 0))
        {
            Cursor.SetCursor(mouseTexture , new Vector2(mousePointDsts , mousePointDsts) , CursorMode.Auto);
            isMouseLeave = false;
        }
        if(Input.GetMouseButton(0) && (Input.mousePosition.x < Screen.width && Input.mousePosition.x > 0 && Input.mousePosition.y < Screen.height && Input.mousePosition.y > 0))
        {
            Cursor.SetCursor(mouseTexture , new Vector2(mousePointDsts , mousePointDsts) , CursorMode.Auto);
            isMouseLeave = false;
        }
    }
}

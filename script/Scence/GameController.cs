using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance
    {
        get
        {
            if (instance == null)
                instance = Transform.FindObjectOfType<GameController>();
            return instance;
        }
    }
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private GameObject character;
    private GameObject Boss;
    private GameObject BossInGame;
    private int mapNum;
    private int kroossState;
    [SerializeField]private float enemyBoenPrepareTime;
    [SerializeField]private int sponsoredUVACount = 0;
    public void SetCharacter(GameObject chara)
    {
        character = chara;
    }
    public GameObject GetCharacter()
    {
        return character;
    }
    public void SetBossInGame(GameObject BossInGame)
    {
        this.BossInGame = BossInGame;
    }
    public GameObject GetBossInGame()
    {
        return BossInGame;
    }
    public void SetBoss(GameObject Boss)
    {
        this.Boss = Boss;
    }
    public GameObject GetBoss()
    {
        return Boss;
    }
    public void SetMapNum(int mapNum)
    {
        this.mapNum = mapNum;
    }
    public int GetMapNum()
    {
        return mapNum;
    }
    public void SetKroossState(int kroossState)
    {
        this.kroossState = kroossState;
    }
    public int GetKroossState()
    {
        return kroossState;
    }
    public void SetSponsoredUVACount(int count)
    {
        sponsoredUVACount = count;
    }
    public void ResetSponsoredUVACount()
    {
        sponsoredUVACount = 0;
    }
    public void UVACountIncrese()
    {
        sponsoredUVACount++;
    }
    public int GetSsponsoredUVACount()
    {
        return sponsoredUVACount;
    }
    public void SetEnemyBoenPrepareTime(float time)       //这个应该设置一次就够了
    {
        enemyBoenPrepareTime = time;
    }
    public float GetEnemyBoenPrepareTime()
    {
        return enemyBoenPrepareTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveJsonData
{
    public Savedata savedata;
    public PlayHistorydata playHistorydata;
    public SaveJsonData()
    {
        savedata = new Savedata();
        playHistorydata = new PlayHistorydata();
    }
    //存档数据分两层的意义是，上一层的保存各类存档数据中可以有多种存档数据
    //当开发者使用时可以获取特定的数据类型如玩家属性、剧情进度等，并在其下寻找数据，方便使用
}

[System.Serializable]
public class Savedata
{
    public int longmenCoin = 0;
    public bool isCharacter1Unlock = true;
    public bool isCharacter2Unlock = false;
    public bool isSkill1Unlock = false;
    public bool isSkill2Unlock = false;
    public bool isSkill3Unlock = false;
    public bool isEnemy1Unlock = false;
    public bool isEnemy2Unlock = false;
    public bool isEnemy3Unlock = false;
    public bool isBossVeyronUnlock = false;
}
[System.Serializable]
public class PlayHistorydata
{
    public int playEndCount = 0;
    public int enemyHasBeat = 0;
    public int BossHasBeat = 0;
    public int highestScore = 0;
}

// public class SettingData
// {
//     public float Music;
//     public float highestScore;
//     public float highestScore;
// }

public class ArchiveSystem : MonoBehaviour
{
    private static ArchiveSystem instance;
    public static ArchiveSystem Instance
    {
        get
        {
            if (instance == null)
                instance = Transform.FindObjectOfType<ArchiveSystem>();
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
        InitJsonData();
        Load();
    }

    private SaveJsonData jsonData;
    string JsonPath()
    {
        return Path.Combine(Application.streamingAssetsPath , "Data.json");
    }
    void InitJsonData()
    {
        jsonData = new SaveJsonData();
    }
    bool ExistsJson()
    {
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }
        return File.Exists(JsonPath());
    }
    public void Save()
    {
        if (!ExistsJson())
        {
            File.Create(JsonPath()).Close();
        }
        string json = JsonUtility.ToJson(jsonData , true);
        File.WriteAllText(JsonPath() , json);
        Debug.Log("Save success!");
    }
    public void Load()
    {
        if (!ExistsJson())
        {
            Debug.Log("Save Error! File is not exists!");
            Save();
            return;
        }
        string json = File.ReadAllText(JsonPath());
        jsonData = JsonUtility.FromJson<SaveJsonData>(json);
        Debug.Log("Load success!");
    }
    // public void GameEnd(int coinGet , int score , int enemyBeated , int BossBeated)
    public void GameEnd(int coinGet , int score)
    {
        jsonData.savedata.longmenCoin += coinGet;
        if (jsonData.playHistorydata.highestScore < score)
        {
            jsonData.playHistorydata.highestScore = score;
        }
        jsonData.playHistorydata.playEndCount ++;
        Save();
    }
    public void GameOver(int coinGet , int score)
    {
        jsonData.savedata.longmenCoin += coinGet;
        if (jsonData.playHistorydata.highestScore < score)
        {
            jsonData.playHistorydata.highestScore = score;
        }
        Save();
    }
    public Savedata GetPlayerData()
    {
        return jsonData.savedata;
    }
    public PlayHistorydata GetHistoryData()
    {
        return jsonData.playHistorydata;
    }
}

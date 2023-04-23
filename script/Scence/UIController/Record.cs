using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Record : MonoBehaviour
{
    [SerializeField]private Text LongmenCoin;
    [SerializeField]private Text HighsetScore;
    [SerializeField]private Text PassTimes;
    
    void Start()
    {
        LongmenCoin.text = "持有龙门币：" + ArchiveSystem.Instance.GetPlayerData().longmenCoin.ToString();
        HighsetScore.text = "最高评价：" + ArchiveSystem.Instance.GetHistoryData().highestScore.ToString();
        PassTimes.text = "任务成功次数：" + ArchiveSystem.Instance.GetHistoryData().playEndCount.ToString();
    }
    public void GetData()
    {
        LongmenCoin.text = "持有龙门币：" + ArchiveSystem.Instance.GetPlayerData().longmenCoin.ToString();
        HighsetScore.text = "最高评价：" + ArchiveSystem.Instance.GetHistoryData().highestScore.ToString();
        PassTimes.text = "任务成功次数：" + ArchiveSystem.Instance.GetHistoryData().playEndCount.ToString();
    }

}

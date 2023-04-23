using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseSponsoredUVA : PurchasePage
{
    [SerializeField]new protected int price;
    [SerializeField]protected Text countText;
    [SerializeField]protected Text longmenCoinText;
    void Start()
    {
        countText.text = GameController.Instance.GetSsponsoredUVACount().ToString() + " / 5" ;
        longmenCoinText.text = ArchiveSystem.Instance.GetPlayerData().longmenCoin.ToString();
    }
    override public void ConfirmPayment()
    {
        if (ArchiveSystem.Instance.GetPlayerData().longmenCoin >= price && GameController.Instance.GetSsponsoredUVACount() < 5)
        {
            ArchiveSystem.Instance.GetPlayerData().longmenCoin -= price;
            Product();
            // ArchiveSystem.Instance.Save();
        }
        else
        {
            SoundManager.Instance.PlayEffectSound(defaltSound);
        }
    }
    override protected void Product()
    {
        GameController.Instance.UVACountIncrese();
        SoundManager.Instance.PlayEffectSound(successSound);
        countText.text = GameController.Instance.GetSsponsoredUVACount().ToString() + " / 5" ;
        longmenCoinText.text = ArchiveSystem.Instance.GetPlayerData().longmenCoin.ToString();
        // ArchiveSystem.Instance.GetPlayerData().longmenCoin -= 100;
    }
}

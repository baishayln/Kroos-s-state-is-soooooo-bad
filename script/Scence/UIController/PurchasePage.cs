using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchasePage : MonoBehaviour
{
    protected int price;
    [SerializeField]protected Text text;
    [SerializeField]protected AudioClip defaltSound;
    [SerializeField]protected AudioClip successSound;
    protected GameObject transactionRequestSource;
    virtual public void SetPrice(int price)
    {
        this.price = price;
    }
    virtual public void SetString(string describe)
    {
        text.text = describe;
    }
    virtual public void SetReturnTarget(GameObject transactionRequestSource)
    {
        this.transactionRequestSource = transactionRequestSource;
    }
    virtual public void ConfirmPayment()
    {
        if (ArchiveSystem.Instance.GetPlayerData().longmenCoin >= price)
        {
            ArchiveSystem.Instance.GetPlayerData().longmenCoin -= price;
            Product();
            if (transactionRequestSource)
            {
                transactionRequestSource.GetComponent<IsThisUnlock>().Unlock();
            }
            else
            {
                SoundManager.Instance.PlayEffectSound(defaltSound);
            }
            ArchiveSystem.Instance.Save();
            ExitPurchses();
        }
        else
        {
            SoundManager.Instance.PlayEffectSound(defaltSound);
        }
    }
    virtual public void ExitPurchses()
    {
        // gameObject.SetActive(false);
        ObjectPool.Instance.PushObject(gameObject);
    }
    virtual protected void Product()
    {
        Debug.Log("获得商品.");
        SoundManager.Instance.PlayEffectSound(successSound);
    }
}

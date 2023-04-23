using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterPurchasePage : MonoBehaviour
{
    [SerializeField]GameObject purchesePage;
    // [SerializeField]GameObject prodect;
    [SerializeField]int price;
    [SerializeField]private string text = "是否要花费XX龙门币购买XX？";
    [SerializeField]private Text texttext;
    public void EnterPurchase()
    {
        GameObject page = ObjectPool.Instance.GetObject(purchesePage);
        // page.transform.parent = transform;
        page.transform.SetParent(transform.parent.parent , true);
        page.GetComponent<PurchasePage>().SetPrice(price);
        page.GetComponent<PurchasePage>().SetReturnTarget(gameObject);
        if (texttext)
        {
            page.GetComponent<PurchasePage>().SetString(texttext.text);
        }
        else
        {
            page.GetComponent<PurchasePage>().SetString(text);
        }
    }
}

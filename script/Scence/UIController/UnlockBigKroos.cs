using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockBigKroos : PurchasePage
{
    override protected void Product()
    {
        ArchiveSystem.Instance.GetPlayerData().isCharacter2Unlock = true;
    }
}

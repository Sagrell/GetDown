using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyCoins : MonoBehaviour {

	public void BuySmallCoins()
    {
        PurchaseManager.Instance.BuyConsumable(0);
    }
    public void BuyMediumCoins()
    {
        PurchaseManager.Instance.BuyConsumable(1);
    }
    public void BuyLargeCoins()
    {
        PurchaseManager.Instance.BuyConsumable(2);
    }
}

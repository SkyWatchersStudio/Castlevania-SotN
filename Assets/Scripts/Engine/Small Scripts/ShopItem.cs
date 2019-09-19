using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public void Buy(int price)
    {
        if (price <= GameManager.Coin)
        {
            GameManager.Coin -= price;
            Destroy(this.gameObject);
        }
    }
}

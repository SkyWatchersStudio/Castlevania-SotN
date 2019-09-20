using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Item identity;

    public enum Item { Potion, SwordOfFire}

    private Button m_Button;

    private void Awake()
    {
        m_Button = GetComponentInChildren<Button>();
    }

    public void Buy(int price)
    {
        if (price <= GameManager.Coin)
        {
            GameManager.Coin -= price;
            bool destroy = false;

            switch (identity)
            {
                case Item.SwordOfFire:
                    InventoryItem.FireSword = true;
                    destroy = true;
                    break;
                case Item.Potion:
                    GameManager.Potions += 1;
                    break;
            }

            if (!destroy)
                return;
            Destroy(gameObject);
            ShopEvent.m_Instance.OnDestroyButton();
        }
    }
}

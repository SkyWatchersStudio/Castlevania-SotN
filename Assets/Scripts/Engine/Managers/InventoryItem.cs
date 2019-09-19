using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Item item;

    public enum Item { ShortSword, SwordOfIce, SwordOfFire, Potion };

    private GameObject m_UseButton;

    private void Start()
    {
        m_UseButton = GetComponentInChildren<Button>().gameObject;
    }

    public void UseItem()
    {
        m_UseButton.SetActive(false);

        switch(item)
        {
            case Item.ShortSword:

                break;
            case Item.SwordOfIce:

                break;
            case Item.SwordOfFire:

                break;
            case Item.Potion:
                if (GameManager.Potions > 0)
                {

                }
                break;
        }
    }
}

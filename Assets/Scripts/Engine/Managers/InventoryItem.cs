using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Item item;

    public enum Item { ShortSword, SwordOfIce, SwordOfFire, Potion };

    public static bool IceSword, FireSword;

    public void UseItem()
    {
        switch(item)
        {
            case Item.ShortSword:
                SwapSword(10, 0);
                break;
            case Item.SwordOfIce:
                if (IceSword)
                    SwapSword(20, 1);
                break;
            case Item.SwordOfFire:
                if (FireSword)
                    SwapSword(30, 2);
                break;
            case Item.Potion:
                if (GameManager.Potions > 0)
                {
                    GameManager.Potions -= 1;
                    Player.m_Instance.CurrentHealth += 
                        GameManager.m_Instance.potionHealthRestore;
                }
                break;
        }
    }
    private void SwapSword(float damage, int index)
    {
        Player.m_Instance.CurrentDamage = damage;
        Player.m_Instance.m_OverrideAnimator["attack"] =
            Player.m_Instance.swordAnimations[index];
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ShopEvent : MonoBehaviour
{
    public GameObject shop;
    public GameObject potion;

    public static ShopEvent m_Instance;

    private void Start()
    {
        m_Instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var firstSelected = NextButton();
            GameManager.MenuActivator(shop, firstSelected, false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var firstSelected = NextButton();
            GameManager.MenuActivator(shop, firstSelected, false);
        }
    }

    private GameObject NextButton() => shop.GetComponentInChildren<Button>().gameObject;

    public void OnDestroyButton()
    {
        GameManager.m_Instance.eventSystem.SetSelectedGameObject(potion);
    }
}

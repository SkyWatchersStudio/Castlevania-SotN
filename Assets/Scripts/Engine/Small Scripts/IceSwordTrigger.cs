using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSwordTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InventoryItem.IceSword = true;
            Destroy(gameObject);
            AudioManager.Play("IceSwordSound");
        }
    }
}

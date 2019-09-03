using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject[] dropItems;

    public void OnInteract()
    {
        var item = Random.Range(0, dropItems.Length);
        Instantiate(dropItems[item], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject[] dropItems;

    public void OnInteract()
    {
        //choose a random item to drop...
        var item = Random.Range(0, dropItems.Length);

        Destroy(gameObject);

        if (dropItems[item] == null)
            return;

        Instantiate(dropItems[item], transform.position, Quaternion.identity);
    }
}

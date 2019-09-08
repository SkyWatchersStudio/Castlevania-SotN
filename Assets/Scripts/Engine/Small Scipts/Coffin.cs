using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffin : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            if (Input.GetKeyDown(KeyCode.UpArrow))
                GameManager.SavingData(collision.transform);
    }
}

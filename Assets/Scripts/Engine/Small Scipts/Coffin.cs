using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffin : MonoBehaviour
{
    public delegate void OnSave();
    public static event OnSave PlayerSaveEvent;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            if (Input.GetKeyDown(KeyCode.UpArrow))
                PlayerSaveEvent();
    }
}

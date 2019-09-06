using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject[] frames = new GameObject[2];

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            foreach (var frame in frames)
                frame.SetActive(!frame.activeSelf);
    }
}

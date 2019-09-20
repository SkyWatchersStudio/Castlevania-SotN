using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffin : MonoBehaviour
{
    private static AudioManager m_audioManager;

    private void Start()
    {
        if(m_audioManager == null)
            m_audioManager = FindObjectOfType<AudioManager>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                GameManager.SavingData(collision.transform);
                m_audioManager.Play("Savesound");
            }
                

    }
}

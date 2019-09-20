using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionPickup : MonoBehaviour
{
    private static AudioManager m_Audio;

    private void Start()
    {
        if (m_Audio == null)
            m_Audio = FindObjectOfType<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameManager.Potions += 1;
            m_Audio.Play("Potionpickup");
        }
    }
}

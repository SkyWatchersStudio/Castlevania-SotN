using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.m_DashAbility = true;
            Destroy(gameObject);
        }
    }
}

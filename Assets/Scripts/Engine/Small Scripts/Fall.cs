using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    private Transform m_Respawn;

    private void Start() => m_Respawn = transform.GetChild(0);

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.attachedRigidbody.position = m_Respawn.position;
            collision.attachedRigidbody.velocity = Vector2.zero;
        }
    }
}

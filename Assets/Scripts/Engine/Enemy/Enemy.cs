using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Rigidbody2D player = collision.GetComponent<Rigidbody2D>();
            player.AddForce(-collision.transform.forward * 2);
        }
    }
}

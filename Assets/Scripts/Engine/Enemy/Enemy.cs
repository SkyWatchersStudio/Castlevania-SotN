using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    //number of hit require to die
    public int health;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

        }
    }
    void TakeDamage()
    {
        health -= 1;
        if (health <= 0)
            Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int coin = 50;
    public float lifeTime = 3;

    private float m_Timer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameManager.Coin = this.coin;
        }
    }

    private void Update()
    {
        m_Timer += Time.deltaTime;
        if (m_Timer >= lifeTime)
            Destroy(gameObject);
    }
}

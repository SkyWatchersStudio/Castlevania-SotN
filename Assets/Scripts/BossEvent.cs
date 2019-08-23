using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEvent : MonoBehaviour
{
    public Vector3 targetPosition;

    Death m_Death;
    BoxCollider2D m_Collider;

    private void Awake()
    {
        m_Death = GameObject.Find("Death").GetComponent<Death>();
        m_Collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //say a warm welcome to the player
            var rigidbody = collision.GetComponent<Rigidbody2D>();
            rigidbody.position = targetPosition;

            //give a power to the player
            collision.GetComponent<PlayerAttack>().attackRange = 2;

            //feed player to the Death
            m_Death.m_PlayerTransform = collision.transform;
            //close the door
            m_Collider.isTrigger = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //throw player back
            var rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            rigidbody.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
        }
    }
}

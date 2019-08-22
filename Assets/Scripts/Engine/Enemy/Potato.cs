using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potato : MonoBehaviour
{
    public float moveSpeed;
    public float health;

    Transform m_Target;
    Rigidbody2D m_Rigidbody;
    bool m_targetFound = false;

    private void Awake()
    {
        m_Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (!m_targetFound)
            return;

        if (m_Rigidbody == null)
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
            print("Rigidbody assigned again");
        }

        Vector2 movement = Vector2.MoveTowards(transform.position, m_Target.position,
                                                moveSpeed * Time.deltaTime);
        m_Rigidbody.MovePosition(movement);
    }

    public override IEnumerator TakeDamage()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            m_targetFound = true;
    }
}

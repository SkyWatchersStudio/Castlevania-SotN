using System.Collections;
using UnityEngine;
using System;

public class Death : MonoBehaviour
{
    public float moveSpeed;
    public float timeBtwMove;
    public int health = 3;

    private bool m_FacingRight = false;
    Rigidbody2D m_Rigidbody;
    float m_Timer;
    Vector2 m_DeltaPosition;

    [System.NonSerialized]
    public Transform m_PlayerTransform;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (m_PlayerTransform == null)
            return;

        m_DeltaPosition = m_PlayerTransform.position - transform.position;
        var direction = m_DeltaPosition.normalized;

        if (direction.x > 0 && !m_FacingRight)
            RotateDeath();
        else if (direction.x < 0 && m_FacingRight)
            RotateDeath();

        if (m_Timer <= 0)
            Move();
    }
    void RotateDeath()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    void Move()
    {
        float desireSpeed = moveSpeed * (1 - (1 / m_DeltaPosition.magnitude)) 
            * Time.deltaTime;
        var desireMove = m_DeltaPosition.normalized * desireSpeed;

        m_Rigidbody.MovePosition((Vector2)transform.position + desireMove);

        if (m_DeltaPosition.magnitude < 5)
        {
            m_Timer = timeBtwMove;
            var playerRigid = m_PlayerTransform.GetComponent<Rigidbody2D>();
            playerRigid.AddForce(m_DeltaPosition.normalized * 20, ForceMode2D.Impulse);

            playerRigid.GetComponent<PlayerMotion>().TakeDamage();
        }
    }
    private void Update()
    {
        m_Timer -= Time.deltaTime;
    }

    public void TakeDamage()
    {
        health -= 1;
        if (health <= 0)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var rigid = collision.gameObject.GetComponent<Rigidbody2D>();
        rigid.AddForce(m_DeltaPosition.normalized * 20, ForceMode2D.Impulse);

        collision.gameObject.GetComponent<PlayerMotion>().TakeDamage();
    }
}

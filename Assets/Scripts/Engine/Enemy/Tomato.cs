using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tomato : MonoBehaviour
{
    public float moveSpeed;
    public int health;
    public float collisionForce = 4;

    [Space(10)]
    public Transform detectionPos;
    public float detectionRadius;

    private Rigidbody2D m_Rigidbody;
    private bool m_IsPlayerFound = false;
    private Transform m_PlayerTransform;
    private LayerMask m_LayerMask;
    private Vector2 m_Direction;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_LayerMask = LayerMask.NameToLayer("NotGround");
    }

    private void FixedUpdate()
    {
        if (!m_IsPlayerFound)
            return;
        var nextMove = (Vector2)transform.position + (m_Direction * moveSpeed * Time.deltaTime);
        m_Rigidbody.MovePosition(nextMove);
    }
    void DetectPlayer()
    {
        Collider2D[] results = new Collider2D[2];

        var overlapNum = Physics2D.OverlapCircleNonAlloc
            (detectionPos.position, detectionRadius, results);
        if (overlapNum > 0)
            foreach (var colliders in results)
            {
                if (colliders == null)
                    continue;
                else if (colliders.tag == "Player")
                {
                    m_IsPlayerFound = m_Rigidbody.simulated = true;
                    m_PlayerTransform = colliders.transform;
                    m_Direction = Vector2.right * GetDirection();
                }
            }
    }
    Vector2 GetDirection()
    {
        var deltaPosition = m_PlayerTransform.position - transform.position;
        return deltaPosition.normalized;
    }
    private void Update()
    {
        if (!m_IsPlayerFound)
            DetectPlayer();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //applly force to the player in opposite direction of player sees enemy
        if (collision.gameObject.CompareTag("Player"))
        {
            var animator = collision.gameObject.GetComponent<Animator>();
            animator.SetTrigger("Hit");

            var rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            rigidbody.AddForce(GetDirection() * collisionForce, ForceMode2D.Impulse);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(detectionPos.position, detectionRadius);
    }
}

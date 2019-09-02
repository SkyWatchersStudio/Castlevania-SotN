using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Characters : MonoBehaviour
{
    public int health;
    public float moveSpeed;
    [Space(10)]
    public Transform groundCheck;
    public float groundRadius;
    [Space(10)]
    public bool m_FacingRight;

    protected Rigidbody2D m_Rigidbody;
    protected Animator m_Animator;
    protected const int m_NotGroundLayer = 1 << 8;
    protected const int m_GroundLayer = ~m_NotGroundLayer;

    public virtual void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
    }
    public virtual void FixedUpdate()
    {
        Move();
    }
    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }

    protected void Flip(float horizontal)
    {
        if ((horizontal > 0 && m_FacingRight) || 
            (horizontal < 0 && !m_FacingRight) || (horizontal == 0))
            return;

        m_FacingRight = !m_FacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    protected bool CheckArea(
        Vector3 checkPosition, float radius, LayerMask layer, out Collider2D target)
    {
        Collider2D[] results = new Collider2D[1];
        int colliders = Physics2D.OverlapCircleNonAlloc
                              (checkPosition, radius, results, layer);

        target = results[0];

        return colliders > 0;
    }
    protected bool CheckGround() =>
            CheckArea(groundCheck.position, groundRadius, m_GroundLayer, out _);

    public abstract void Move();
    public abstract void TakeDamage();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
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

    private const int m_GroundLayer = ~(1 << 8);

    public virtual void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();

        if (m_Rigidbody == null || m_Animator == null)
            Debug.LogError($"{this.gameObject} need animator and rigidbody component");
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
    protected bool CheckGround(out Collider2D target) =>
            CheckArea(groundCheck.position, groundRadius, m_GroundLayer, out target);

    public abstract void Move();
    public abstract void TakeDamage();
}

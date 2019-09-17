using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommonAbilities : MonoBehaviour
{
    public float jumpForce;
    [Space(10)]
    public float attackForce;
    public float timeBetweenAttack;
    public Transform attackPosition;
    public float attackRange;
    [Space(10)]
    public float dashForce;
    public float timeBetweenDash;
    [Space(10)]
    public float dodgeForce;

    private Characters m_Base;

    private bool m_IsJumping, m_Lock;
    private float m_TimeBtwAttack, m_TimeBtwDash;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;

    [System.NonSerialized]
    public bool m_Grounded, m_Attack, m_Dash,
        m_Dodge, m_InterruptJump, m_ShouldJump;
    [System.NonSerialized]
    public Collider2D[] m_GroundColliders;

    public bool IsLock { get => m_Lock; }

    enum WhichAnimation { dodge, dash}
    private WhichAnimation m_AnimDD;
    private int m_AttackID, m_SpeedID, m_IsGroundID;


    private void Awake()
    {
        m_Base = GetComponent<Characters>();
    }
    private void Start()
    {
        //override this cause animator component is different than base class thought
        m_Animator = GetComponentInChildren<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();

        //Animator parameter Ids
        m_AttackID = Animator.StringToHash("Attack");
        m_SpeedID = Animator.StringToHash("Speed");
        m_IsGroundID = Animator.StringToHash("isGround");
    }
    private void Update()
    {
        m_TimeBtwAttack -= Time.deltaTime;
        m_TimeBtwDash -= Time.deltaTime;
    }

    public static bool m_DashAbility;

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, m_Rigidbody.velocity, Color.cyan);

        m_Animator.SetBool(m_IsGroundID, m_Grounded);
        m_Animator.SetFloat("vSpeed", m_Rigidbody.velocity.y);
        m_Animator.SetFloat(m_SpeedID, Mathf.Abs(m_Rigidbody.velocity.x));

        if (m_Attack && m_TimeBtwAttack < 0)
        {
            m_Attack = false;
            m_Animator.SetTrigger(m_AttackID);
            m_TimeBtwAttack = timeBetweenAttack;
        }
        if (!m_Lock)
        {
            if (m_Grounded)
                m_Rigidbody.gravityScale = 0;
            else
                m_Rigidbody.gravityScale = 1;

            if (m_Dash)
            {
                if (m_DashAbility && m_TimeBtwDash < 0)
                {
                    Dash(ref m_Dash, dashForce, true);

                    m_Animator.SetBool("Dash", true);
                    m_AnimDD = WhichAnimation.dash;

                    m_TimeBtwDash = timeBetweenDash;
                }
            }
            else if (m_Dodge && m_Grounded)
            {
                Dash(ref m_Dodge, dodgeForce, false);

                m_Animator.SetBool("Doudge", true);
                m_AnimDD = WhichAnimation.dodge;
            }
        }
        else if (m_Lock)
        {
            if (Mathf.Abs(m_Rigidbody.velocity.x) <= 6)
            {
                m_Lock = false;
                switch (m_AnimDD)
                {
                    case WhichAnimation.dash:
                        m_Animator.SetBool("Dash", false);
                        break;
                    case WhichAnimation.dodge:
                        m_Animator.SetBool("Doudge", false);
                        break;
                }
            }
            return;
        }

        JumpStatus();
    }

    public void Move(float direction)
    {
        var slope = Vector2.right;
        if (m_Grounded)
            foreach (var groundCollider in m_GroundColliders)
            {
                if (groundCollider == null)
                    continue;

                slope = groundCollider.transform.right;
            }

        Vector2 movement = slope * direction * m_Base.moveSpeed;
        m_Rigidbody.AddForce(movement);
    }

    private void JumpStatus()
    {
        if (m_IsJumping && m_Grounded)
            m_IsJumping = false;

        if (m_ShouldJump && m_Grounded)
        {
            m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, 0);
            m_Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            m_IsJumping = true;
            m_ShouldJump = false;
        }
        else if (m_InterruptJump && m_Rigidbody.velocity.y > 0 && m_IsJumping)
        {
            Vector2 vel = m_Rigidbody.velocity;
            vel.y = 0;
            m_Rigidbody.velocity = vel;
            m_InterruptJump = false;
        }
    }
    private void Dash(ref bool job, float force, bool isForwardDir)
    {
        job = false;

        m_Rigidbody.velocity = Vector2.zero;
        m_Rigidbody.gravityScale = 0;
        m_Lock = true;

        var forceDir = Vector2.right * force;
        if ((isForwardDir && !m_Base.m_FacingRight) || (!isForwardDir && m_Base.m_FacingRight))
            forceDir = -forceDir; //whether we should be forced forward or not

        m_Rigidbody.AddForce(forceDir, ForceMode2D.Impulse);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
#endif
}

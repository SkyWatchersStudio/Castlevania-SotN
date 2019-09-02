using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Player : Characters
{
    public float jumpSaveTime;
    public float jumpForce;
    [Space(10)]
    public float attackForce;
    public float timeBetweenAttack;
    public Transform attackPosition;
    public float attackRange;
    [Space(10)]
    public Transform nextFoot;

    private float m_HorizontalInput;
    private float m_JumpSaveTime;
    private bool m_Grounded, m_IsJumping, m_InterruptJumping;
    private float m_TimeBtwAttack;
    private bool m_Attack;

    int m_AttackID, m_SpeedID, m_IsGroundID;

    private void Update()
    {
        m_JumpSaveTime -= Time.deltaTime;
        m_TimeBtwAttack -= Time.deltaTime;

        //jump input determiner
        if (Input.GetButtonDown("Jump"))
            m_JumpSaveTime = jumpSaveTime;
        //we don't want to apply force when we are falling or we are not jumping ofcourse!
        else if (Input.GetButtonUp("Jump") && m_Rigidbody.velocity.y > 0 && m_IsJumping)
            m_InterruptJumping = true;

        if (Input.GetButtonDown("Fire2") && m_TimeBtwAttack < 0)
        {
            m_TimeBtwAttack = timeBetweenAttack;
            m_Attack = true;
        }

        m_HorizontalInput = Input.GetAxisRaw("Horizontal");
        Flip(m_HorizontalInput);

        m_Animator.SetFloat(m_SpeedID, Mathf.Abs(m_HorizontalInput));
    }
    private void JumpStatus()
    {
        if (m_IsJumping && m_Grounded)
            m_IsJumping = false;

        m_Animator.SetFloat("vSpeed", m_Rigidbody.velocity.y);

        if (m_JumpSaveTime > 0 && m_Grounded)
        {
            m_Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            m_IsJumping = true;
        }
        else if (m_InterruptJumping)
        {
            m_Rigidbody.AddForce(Vector2.up * -m_Rigidbody.velocity.y, ForceMode2D.Impulse);
            m_InterruptJumping = m_IsJumping = false;
        }
    }
    private void Attack()
    {
        m_Attack = false;
        m_Animator.SetTrigger(m_AttackID);

        var enemies = Physics2D.OverlapCircleAll(
            attackPosition.position, attackRange, m_NotGroundLayer);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                //add force to the opposite direction of enemy
                var rigidbody = enemy.GetComponent<Rigidbody2D>();
                Vector2 direction = (enemy.transform.position - transform.position).normalized;
                rigidbody.AddForce(direction * attackForce, ForceMode2D.Impulse);

                //apply damage to enemy
                var enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                    enemyScript.TakeDamage();
            }
        }
    }
    private Vector2 MoveForceDirection()
    {
        var defaultDirection = Vector2.right * m_HorizontalInput * moveSpeed;
        if (!m_Grounded)
            return defaultDirection;

        var rayDir = nextFoot.position - transform.position;
        RaycastHit2D[] hit = new RaycastHit2D[1];

        Physics2D.RaycastNonAlloc(
            transform.position, rayDir.normalized, hit, rayDir.magnitude, m_GroundLayer);

        var alpha = Vector2.Angle(hit[0].normal, Vector2.up);
        if (alpha == 0)
            return defaultDirection;

        //if we reached this point we should calculate these variables(cause we need them!)
        float groundAngle = Vector2.Angle(hit[0].transform.right, Vector2.up);
        //slope gravity
        var gravity = Mathf.Abs(m_Rigidbody.mass * Physics2D.gravity.y / Mathf.Sin(alpha));
        float xVector, yVector;
        float forceVector;

        if (groundAngle < 90)
        {
            forceVector = moveSpeed + (m_HorizontalInput * gravity);
            //calculate x, y vectors to reach the desire force in slope
            xVector = m_HorizontalInput * forceVector * Mathf.Cos(alpha);
            yVector = m_HorizontalInput * forceVector * Mathf.Sin(alpha);
        }
        else
        {
            forceVector = moveSpeed + (-m_HorizontalInput * gravity);
            xVector = m_HorizontalInput * forceVector * Mathf.Cos(alpha);
            yVector = -m_HorizontalInput * forceVector * Mathf.Sin(alpha);
        }
        return new Vector2(xVector, yVector);
    }

    public override void Start()
    {
        base.Start();

        //override this cause animator component is different than base class thought
        m_Animator = GetComponentInChildren<Animator>();

        //Animator parameter Ids
        m_AttackID = Animator.StringToHash("Attack");
        m_SpeedID = Animator.StringToHash("Speed");
        m_IsGroundID = Animator.StringToHash("isGround");
    }
    public override void FixedUpdate()
    {
        m_Grounded = CheckGround();
        m_Animator.SetBool(m_IsGroundID, m_Grounded);

        JumpStatus();
        if (m_Attack)
            Attack();

        base.FixedUpdate();
    }
    public override void Move()
    {
        //debuger...
        Debug.DrawRay(transform.position, m_Rigidbody.velocity, Color.cyan);

        m_Rigidbody.AddForce(MoveForceDirection());
    }
    public override void TakeDamage()
    {
        health -= 1;
        if (health <= 0)
            SceneManager.LoadScene(0);
    }
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
        var rayDir = nextFoot.position - transform.position;
        Gizmos.DrawRay(transform.position, rayDir);
    }
}

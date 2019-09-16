using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerCommonAbilities))]
public sealed class Player : Characters
{
    public float jumpSaveTime;
    public float mistForce;
    [Space(10)]
    public Image healthImage;

    private float m_HorizontalInput;
    private float m_JumpSaveTime;
    private bool m_MistTransform;

    private Collider2D[] m_GroundColliders;

    private PlayerCommonAbilities m_Abilities;

    private float m_Health;
    public float CurrentHealth
    {
        get => m_Health;
        set
        {
            m_Health = value;
            healthImage.fillAmount = m_Health / health;
        }
    }

    private void Update()
    {
        m_JumpSaveTime -= Time.deltaTime;

        //if (m_MistAbility)
        if (Input.GetButtonDown("Mist"))
            m_MistTransform = true;

        m_HorizontalInput = Input.GetAxisRaw("Horizontal");
        Flip(m_HorizontalInput);

        if (m_IsMist)
            return;

        InputDetection();
    }

    public static bool m_DashAbility, m_MistAbility;

    private void InputDetection()
    {
        //jump input determiner
        if (Input.GetButtonDown("Jump"))
            m_JumpSaveTime = jumpSaveTime;
        //we don't want to apply force when we are falling or we are not jumping ofcourse!
        else if (Input.GetButtonUp("Jump") && m_Rigidbody.velocity.y > 0 && m_IsJumping)
        {
            Vector2 vel = m_Rigidbody.velocity;
            vel.y = 0;
            m_Rigidbody.velocity = vel;
        }

        if (Input.GetButtonDown("Attack") && m_TimeBtwAttack < 0)
        {
            m_TimeBtwAttack = timeBetweenAttack;
            m_Attack = true;
        }
        else if (Input.GetButtonDown("Dodge") && m_Grounded)
            m_Dodge = true;

        if (m_DashAbility)
        {
            if (Input.GetButtonDown("Dash") && m_TimeBtwDash < 0)
            {
                m_Dash = true;
                m_TimeBtwDash = timeBetweenDash;
            }
        }
    }

    private bool m_IsMist;
    private void MistShifting()
    {
        m_MistTransform = false;
        m_Rigidbody.velocity = Vector2.zero;

        // cause mist to not interact with enemies
        gameObject.layer = gameObject.layer == 8 ? 12 : 8;
        
        if (m_IsMist || !m_Abilities.m_Grounded)
            m_Rigidbody.gravityScale = (m_Rigidbody.gravityScale + 1) % 2;

        m_IsMist = !m_IsMist;
    }
    private void MistMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 vector = new Vector2(horizontal, vertical);
        vector.Normalize();

        m_Rigidbody.AddForce(vector * mistForce);
    }

    public override void Start()
    {
        base.Start();

        m_Abilities = GetComponent<PlayerCommonAbilities>();

        CurrentHealth = health;
    }
    public override void FixedUpdate()
    {
        m_Abilities.m_Grounded = CheckGround(out m_GroundColliders);

        if (m_IsMist)
        {
            if (m_MistTransform) // transform back to normal
                MistShifting();
            MistMove();
            return;
        }

        if (m_MistTransform && !m_Abilities.m_Lock)
            MistShifting(); // transform to mist
    }
    public override void Move()
    {
        var direction = Vector2.right;
        if (m_Abilities.m_Grounded)
            foreach (var groundCollider in m_GroundColliders)
            {
                if (groundCollider == null)
                    continue;

                direction = groundCollider.transform.right;
            }

        Vector2 movement = direction * m_HorizontalInput * moveSpeed;
        m_Rigidbody.AddForce(movement);
    }
    public override void TakeDamage()
    {
        CurrentHealth -= 1;
        if (CurrentHealth <= 0)
            GameManager.Loading(this.transform);
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPosition.position, attackRange);
    }
#endif
}

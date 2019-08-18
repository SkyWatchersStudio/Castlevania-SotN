using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveForce = 10;
    public float jumpForce = 5;

    [Space(10)]
    //Check jump variables:
    public Transform groundCheck;
    public float groundCheckRadius;
    public float saveJumpTime = .01f;

    [Space(10)]
    public float maxJumpDuration;

    Rigidbody2D m_Rigid;
    float m_HorizontalAxis;
    float m_SaveJump, m_JumpTimer;
    bool m_InterruptJump, m_IsJumping;

    void Awake()
    {
        m_Rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Jump();

        Move();
    }
    void Jump()
    {
        if (m_SaveJump > 0)
        {
            if (CheckForGround())
            {
                m_Rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        if (m_InterruptJump)
        {
            m_Rigid.AddForce(Vector2.up * -m_Rigid.velocity.y, ForceMode2D.Impulse);
            m_InterruptJump = false;
        }
    }
    void Move()
    {
        Vector2 movement = Vector2.right * m_HorizontalAxis * moveForce;
        m_Rigid.AddForce(movement);
    }
    bool CheckForGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position,
                                                  groundCheckRadius);
        foreach (var col in colliders) //check in the colliders and remove ourselves
            if (col.gameObject != gameObject)
                return true;

        return false;
    }
    void Update()
    {
        m_HorizontalAxis = Input.GetAxis("Horizontal");

        JumpInteraction(); //get jump input...

        m_SaveJump -= Time.deltaTime; //will decrease time for jump input
    }
    void JumpInteraction()
    {
        if (Input.GetButtonDown("Jump"))
        {
            m_SaveJump = saveJumpTime; //determine jump input pressed
            m_JumpTimer = 0;
        }

        //if (!m_IsJumping)
        //    return;

        //if (Input.GetButton("Jump"))
        //    m_JumpTimer += Time.deltaTime;

        //if (Input.GetButtonUp("Jump") && (m_JumpTimer < maxJumpDuration) && m_IsJumping)
        //    m_InterruptJump = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}

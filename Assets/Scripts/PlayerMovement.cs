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

    Rigidbody2D m_Rigid;
    float m_HorizontalAxis;
    float m_SaveJump;
    bool m_IsJumping;

    void Awake()
    {
        m_Rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (m_IsJumping)
        {
            m_Rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            m_IsJumping = false;
            m_SaveJump = 0;
        }

        Move();
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
        foreach (var col in colliders)
        {
            if (col.gameObject != gameObject)
                return true;
        }

        return false;
    }
    void Update()
    {
        m_HorizontalAxis = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
            m_SaveJump = saveJumpTime; //determine jump input pressed

        CheckForJump(); //Can determine if player should jump or not
    }
    void CheckForJump()
    {
        //if we are in the limit time and we hit the ground we should jump
        if (m_SaveJump > 0 && CheckForGround())
            m_IsJumping = true;

        m_SaveJump -= Time.deltaTime; //will decrease time for jump input
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }
}

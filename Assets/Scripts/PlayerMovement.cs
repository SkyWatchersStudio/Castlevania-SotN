using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveForce = 10;
    public float jumpForce = 5;

    Rigidbody2D m_Rigid;
    float m_HorizontalAxis;
    bool m_IsJump;

    void Start()
    {
        m_Rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (m_IsJump)
            m_Rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        Vector2 movement = Vector2.right * m_HorizontalAxis * moveForce;
        m_Rigid.AddForce(movement);
    }

    void Update()
    {
        m_HorizontalAxis = Input.GetAxis("Horizontal");
        m_IsJump = Input.GetButtonDown("Jump");
    }
}

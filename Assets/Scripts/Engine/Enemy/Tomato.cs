using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Tomato : Enemy
{
    public override void Move()
    {
        bool frontGround = CheckGround(out _);
        if (!frontGround)
        {
            m_Rigidbody.velocity = Vector2.zero;
            return;
        }
        Vector2 movement = Vector2.right * m_TargetDirection * moveSpeed;
        m_Rigidbody.AddForce(movement);
    }
}

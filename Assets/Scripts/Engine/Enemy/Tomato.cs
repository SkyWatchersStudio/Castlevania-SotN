using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Tomato : Enemy
{
    public override void Move()
    {
        Vector2 movement = Vector2.right * m_TargetDirection * moveSpeed;
        m_Rigidbody.AddForce(movement);
    }
}

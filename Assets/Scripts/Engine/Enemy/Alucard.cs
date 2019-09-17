using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alucard : Enemy
{
    PlayerCommonAbilities m_Abilities;

    public override void Start()
    {
        base.Start();
        m_Abilities = GetComponent<PlayerCommonAbilities>();
    }
    public override void FixedUpdate()
    {
        m_Abilities.m_Grounded = CheckGround(out m_Abilities.m_GroundColliders);
        m_Abilities.PhysicUpdate();

        if (m_Abilities.IsLock)
            return;

        base.FixedUpdate();
    }
    public override void Move() => m_Abilities.Move(m_TargetDirection.x);
}

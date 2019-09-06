using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Characters
{
    public float collisionForce;
    [Space(10)]
    public Transform detectionPosition; //range that require to detect enemy
    public float detectionRange;
    [Space(10)]
    public float distanceMagnitude; //delta distance require to disable enemy
    [Space(10)]
    public int experiencePoint = 10;

    protected Vector2 m_TargetDirection;

    private Transform m_PlayerTransform;

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_Animator.SetTrigger("AttackEnemy");

            var playerGameObject = collision.gameObject;
            playerGameObject.GetComponentInChildren<Animator>().SetTrigger("Hit");
            playerGameObject.GetComponent<Player>().TakeDamage();

            collision.rigidbody.AddForce(
                m_TargetDirection * collisionForce, ForceMode2D.Impulse); //push player back
        }
    }

    private void CheckPlayer()
    {
        //check the specified area, this time if player was init get its transform
        var someoneInThere = CheckArea(detectionPosition.position,
                                       detectionRange,
                                       m_NotGroundLayer,
                                       out Collider2D[] targets);

        if (someoneInThere)
            foreach (var target in targets)
            {
                if (target == null)
                    continue;
                if (target.CompareTag("Player")) //if player found simulate in physics engine
                {
                    m_Rigidbody.simulated = true;
                    m_PlayerTransform = target.transform;
                }
            }
    }
    private Vector2 GetPlayerDirection()
    {
        var deltaPosition = m_PlayerTransform.position - transform.position;

        if (deltaPosition.magnitude > distanceMagnitude)
        {
            m_Rigidbody.simulated = false;
            m_PlayerTransform = null;
        }

        return deltaPosition.normalized;
    }

    public override void FixedUpdate()
    {
        if (!m_PlayerTransform)
        {
            CheckPlayer();
            return;
        }

        m_TargetDirection = GetPlayerDirection(); //Get direction toward the player
        Flip(m_TargetDirection.x); //Flip Enemy if needed

        base.FixedUpdate();
    }
    public override void TakeDamage()
    {
        health -= 1;
        if (health <= 0)
        {
            Destroy(this.gameObject);
            GameManager.ExperiencePoint = experiencePoint;
        }
    }
#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(detectionPosition.position, detectionRange);

        if (!UnityEditor.EditorApplication.isPlaying && !m_PlayerTransform)
            return;

        Gizmos.DrawRay(transform.position, m_TargetDirection * distanceMagnitude);
    }
#endif
}

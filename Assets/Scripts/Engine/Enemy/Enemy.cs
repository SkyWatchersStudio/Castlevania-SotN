using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Characters
{
    public float collisionForce;
    [Space(10)]
    public float distanceMagnitude; //delta distance require to disable enemy
    [Space(10)]
    public int experiencePoint = 10;

    protected Vector2 m_TargetDirection;
    protected Transform m_PlayerTransform;
    protected bool m_IsPlayerFound;

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerGameObject = collision.gameObject;
            playerGameObject.GetComponentInChildren<Animator>().SetTrigger("Hit");
            playerGameObject.GetComponent<Player>().TakeDamage();

            var attackDirection = m_TargetDirection.normalized;
            attackDirection.y *= collisionForce * .75f;
            attackDirection.x *= collisionForce *  .25f;

            collision.rigidbody.AddForce(
                attackDirection, ForceMode2D.Impulse); //push player back
        }
    }

    public virtual Vector2 GetPlayerDirection()
    {
        var deltaPosition = m_PlayerTransform.position - transform.position;

        if (deltaPosition.magnitude > distanceMagnitude)
        {
            m_IsPlayerFound = false;
            m_PlayerTransform = null;
        }

        return deltaPosition.normalized;
    }

    public override void FixedUpdate()
    {
        if (!m_IsPlayerFound)
            return;

        m_TargetDirection = GetPlayerDirection();
        Flip(m_TargetDirection.x); //Flip Enemy if needed

        base.FixedUpdate();
    }
    public override void TakeDamage()
    {
        health -= 1;
        if (health <= 0)
        {
            Destroy(this.gameObject);
            GameManager.ExperiencePoint += experiencePoint;
        }
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (!m_IsPlayerFound)
            return;
        Gizmos.DrawRay(transform.position, m_TargetDirection * distanceMagnitude);
    }
#endif
}

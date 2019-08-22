using System.Collections;
using UnityEngine;
using System;

public class Death : MonoBehaviour
{
    public float moveSpeed;

    [Space(10)]
    public Transform lookPosition;
    public Vector2 lookSize;

    [Space(10)]
    //patrol requirements
    public float patrolRange;

    private bool m_PlayerFound;
    Rigidbody2D m_Rigidbody;
    Transform m_PlayerTransform;
    States m_CurrentState = States.Patrol;
    Vector2[] m_PatrolTarget = new Vector2[2];
    int m_CurrentTarget = 0;

    enum States { Patrol, Found, Fight }

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_PatrolTarget = PatrolPosition();
    }

    Vector2[] PatrolPosition()
    {
        //setting up patrol positions
        var middlePoint = patrolRange / 2;

        Vector2[] targets = 
        {
            new Vector2(transform.position.x - middlePoint, transform.position.y),
            new Vector2(transform.position.x + middlePoint, transform.position.y)
        };

        return targets;
    }

    private void FixedUpdate()
    {
        switch (m_CurrentState)
        {
            default:
                PatrolState();
                break;
        }
    }

    Transform CheckForPlayer()
    {
        Collider2D[] results = new Collider2D[1];

        //continuesly check for the player in the specified range
        var lookRange = Physics2D.OverlapBoxNonAlloc
            (lookPosition.position, lookSize, 0, results);

        if (lookRange > 0)
            if (results[0].tag == "Player")
            {
                m_PlayerFound = true;
                return results[0].transform;
            }
        return null;
    }

    void PatrolState()
    {
        //calculate next position based on patrol range...
        Vector2 nextTarget = Vector2.MoveTowards(transform.position,
            m_PatrolTarget[m_CurrentTarget],
            moveSpeed * Time.fixedDeltaTime);

        if ((Vector2)transform.position == m_PatrolTarget[m_CurrentTarget])
        {
            m_CurrentTarget = (m_CurrentTarget + 1) % 2;
            Rotate();
        }

        m_Rigidbody.MovePosition(nextTarget);
    }
    void FoundState()
    { }
    void FightState()
    { }

    void Rotate()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Update()
    {
        if (!m_PlayerFound)
            m_PlayerTransform = CheckForPlayer();
    }

    private void OnDrawGizmosSelected()
    {
        //Detecting range...
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(lookPosition.position, lookSize);
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            //Patrol range...
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(patrolRange, 1));
        }
    }
}

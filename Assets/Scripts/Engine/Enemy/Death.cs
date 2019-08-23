using System.Collections;
using UnityEngine;
using System;

public class Death : MonoBehaviour
{
    public float moveSpeed;

    private bool m_PlayerFound, m_FacingRight = false;
    Rigidbody2D m_Rigidbody;
    States m_CurrentState;

    [System.NonSerialized]
    public Transform m_PlayerTransform;

    enum States { Found, Fight }

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        switch (m_CurrentState)
        {
            case States.Found:
                //do something...
                break;
            case States.Fight:
                //do something else
                break;
        }
    }
}

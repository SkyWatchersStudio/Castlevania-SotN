using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public GameObject alucard;
    public Animator door;

    public static bool m_AlucardDead;

    private void OnEnable()
    {
        if (m_AlucardDead)
        {
            Destroy(alucard);
            Destroy(door);
        }
    }
}

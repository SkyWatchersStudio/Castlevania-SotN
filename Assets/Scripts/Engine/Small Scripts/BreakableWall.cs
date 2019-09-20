using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public GameObject[] walls;
    public ParticleSystem breakEffect;

    private int attackCount;

    public void OnInteract()
    {
        attackCount++;

        foreach (var wall in walls)
        {
            if (wall != null)
            {
                if (wall.activeSelf)
                    Destroy(wall);
                else
                    wall.SetActive(true);
            }
        }

        if (attackCount == 2)
            breakEffect.Play();
    }
}

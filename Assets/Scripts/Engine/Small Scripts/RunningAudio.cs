using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningAudio : MonoBehaviour
{
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    public void runsound()
    {
        audioManager.Play("running");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : StateMachineBehaviour
{
    bool skipAllowed;
    GameObject abilityIntro;

    public bool SkipAllowed { get => skipAllowed; }
    public GameObject AbilityIntro { set => abilityIntro = value; }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!abilityIntro.activeSelf)
            abilityIntro.SetActive(true);
        skipAllowed = true;
    }
    public void Reset()
    {
        abilityIntro = null;
        skipAllowed = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static AudioClip playerrunsound, playerhitsound, playerjumpsound, playerattacksound, enemyjumpsound, enemyhitsound, enemyrunsound, enemyattacksound;
    static AudioSource adioSrc;

    // Start is called before the first frame update
    void Start()
    {
        playerattacksound = Resources.Load<AudioClip>("playerAttack");
        playerrunsound = Resources.Load<AudioClip>("playerRun");
        playerhitsound = Resources.Load<AudioClip>("playerHit");
        playerjumpsound = Resources.Load<AudioClip>("playerJump");
        enemyattacksound = Resources.Load<AudioClip>("enemyAttack");
        enemyhitsound = Resources.Load<AudioClip>("enemyHit");
        enemyjumpsound = Resources.Load<AudioClip>("enemyJump");
        enemyrunsound = Resources.Load<AudioClip>("enemyRun");

        adioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}

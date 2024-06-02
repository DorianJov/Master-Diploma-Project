using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuetterGuetApen : MonoBehaviour
{
    AudioSource[] sources;
    AudioSource OpenEye;
    AudioSource RedBlink;
    AudioSource Harmonic01;
    AudioSource Harmonic02;
    AudioSource Harmonic03;
    AudioSource Harmonic04;

    AudioSource KillMode;

    AudioSource KillModeAmbiant;

    GuetteurSpawner guetteurSpawner;
    private MoveSphereTunnel moveSphereTunnelScript;

    private Animator animator;
    private bool playedOpenEyeOnce = false;
    private bool OpenEyeOnceAnimation = false;
    private bool isChosen = false; // Flag to determine if this prefab is the chosen one

    public bool isLastPrefab = false; // Flag to determine if this prefab is the last one

    int myID = 0;


    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
        guetteurSpawner = GetComponentInParent<GuetteurSpawner>();
        sources = GetComponents<AudioSource>();
        OpenEye = sources[0];
        Harmonic01 = sources[1];
        Harmonic02 = sources[2];
        Harmonic03 = sources[3];
        Harmonic04 = sources[4];
        RedBlink = sources[5];
        KillMode = sources[6];
        KillModeAmbiant = sources[7];
        if (myID == 0)
        {
            KillModeAmbiant.Play();
            KillModeAmbiant.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play_OpenEye_Sound()
    {
        if (!playedOpenEyeOnce)
        {
            OpenEye.Play();
            playedOpenEyeOnce = true;
        }
    }

    public void Play_RandomHarmonic_Sound()
    {
        if (isChosen) // Only play if this prefab is the chosen one
        {
            // Choose a random harmonic to play
            int randomIndex = Random.Range(1, 5); // Random number between 1 and 4
            AudioSource randomHarmonic = null;

            switch (randomIndex)
            {
                case 1:
                    randomHarmonic = Harmonic01;
                    break;
                case 2:
                    randomHarmonic = Harmonic02;
                    break;
                case 3:
                    randomHarmonic = Harmonic03;
                    break;
                case 4:
                    randomHarmonic = Harmonic04;
                    break;
            }

            if (randomHarmonic != null)
            {
                randomHarmonic.Play();
                CallCameraShake(0);
            }
            else
            {
                Debug.LogError("Random harmonic AudioSource is not assigned.");
            }
        }
    }

    public void CallCameraShake(int whichShake)
    {

        if (guetteurSpawner != null)
        {
            if (whichShake == 0)
            {
                guetteurSpawner.CameraShake();
            }
            else
            {
                guetteurSpawner.CameraShakeTransition();
            }
        }
        else
        {
            Debug.LogError("guetteurSpawner is null");
        }
    }

    public void Play_BlinkRed_Sound()
    {
        RedBlink.Play();
        //CallresetLimuleIncrementation();
    }

    public void Play_KillAmbiant_Sound()
    {
        KillModeAmbiant.Play();
        //CallresetLimuleIncrementation();
    }

    public void KillMode_Sound()
    {
        KillMode.Play();
    }

    public void LetsGoKillMode()
    {
        if (animator != null)
        {
            animator.SetBool("KillMode", true);

            //CallCameraShake();
            //deactivateKillMode.
            StartCoroutine(DeactiveKillModeIn(1.5f));

            CallCameraVibration();
        }
    }

    public void CallCameraVibration()
    {

        if (guetteurSpawner != null)
        {
            guetteurSpawner.CallCameraVibrationFromSpawner();
        }
        else
        {
            Debug.LogError("guetteurSpawner is null");
        }
    }

    private void CallresetLimuleIncrementation()
    {
        GameObject limuleTunnel = GameObject.Find("LimuleTunnel");
        if (limuleTunnel != null)
        {
            moveSphereTunnelScript = limuleTunnel.GetComponent<MoveSphereTunnel>();
            if (moveSphereTunnelScript != null)
            {
                //moveSphereTunnelScript.ResetDelayIncrement(0.1f);
                //moveSphereTunnelScript.ResetDelay(0.1f);
                moveSphereTunnelScript.LimuleIsFalling();
            }
            else
            {
                Debug.LogError("MoveSphereTunnel script not found on the limuleTunnel GameObject.");
            }
        }
        else
        {
            Debug.LogError("limuleTunnel GameObject not found.");
        }

    }

    private void CallLimulefollowerRESET()
    {
        GameObject limuleTunnel = GameObject.Find("LimuleTunnel");
        if (limuleTunnel != null)
        {
            moveSphereTunnelScript = limuleTunnel.GetComponent<MoveSphereTunnel>();
            if (moveSphereTunnelScript != null)
            {
                //moveSphereTunnelScript.ResetDelayIncrement(0.1f);
                moveSphereTunnelScript.ResetDelay(0f);
                //moveSphereTunnelScript.LimuleIsFalling();
            }
            else
            {
                Debug.LogError("MoveSphereTunnel script not found on the limuleTunnel GameObject.");
            }
        }
        else
        {
            Debug.LogError("limuleTunnel GameObject not found.");
        }

    }

    private void CallRandomLimuleIncrementation()
    {
        GameObject limuleTunnel = GameObject.Find("LimuleTunnel");
        if (limuleTunnel != null)
        {
            moveSphereTunnelScript = limuleTunnel.GetComponent<MoveSphereTunnel>();
            if (moveSphereTunnelScript != null)
            {
                moveSphereTunnelScript.UpdateDelayIncrementRandom(0.1f, 5f);
            }
            else
            {
                Debug.LogError("MoveSphereTunnel script not found on the limuleTunnel GameObject.");
            }
        }
        else
        {
            Debug.LogError("limuleTunnel GameObject not found.");
        }

    }

    private IEnumerator DeactiveKillModeIn(float Seconds)
    {
        yield return new WaitForSeconds(Seconds);
        if (animator != null)
        {
            animator.SetBool("KillMode", false);
            StartCoroutine(ChangeActiveListenerIN(2f));
        }

        //KillMode.Stop();


    }

    public void LaunchHarmonic(float timeToLaunch)
    {
        //print("LaunchHarmonic");
        StartCoroutine(LaunchHarmonicPhase(timeToLaunch));
    }

    IEnumerator LaunchHarmonicPhase(float seconds)
    {
        // print("LaunchHarmonicPhase");
        yield return new WaitForSeconds(seconds);
        //print("Launched");
        LaunchHarmonicAnimation();
    }

    public void LaunchHarmonicAnimation()
    {
        //print("Guetteur IS BLINK White");
        animator.SetBool("BlinkWhite", true);
        if (myID == 0)
        {
            //CallresetLimuleIncrementation();
            CallLimulefollowerRESET();
        }


    }

    public void TurnONOpenEye()
    {
        //print("IM LUNCH OpenEye:" + myID);
        animator.SetBool("OpenEye", true);
    }

    public void KillMe()
    {
        Destroy(gameObject);
    }

    public void ResetAnimationVariables()
    {
        animator.SetBool("BlinkWhite", false);
        animator.SetBool("BlinkRed", false);
        animator.SetBool("KillMode", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            /* if (!OpenEyeOnceAnimation)
             {
                 animator.SetBool("OpenEye", true);
                 if (isLastPrefab)
                 {
                     print("LightUpSequence");
                     guetteurSpawner.LightUpSequence();
                 }
                 OpenEyeOnceAnimation = true;
             }*/
        }
    }

    // Assign unique ID to each instance
    public void ActivateOpenEye()
    {
        if (isLastPrefab)
        {
            //print("LightUpSequence");
            guetteurSpawner.LightUpSequence();
        }
    }

    public void ActivatALLBlinkRed()
    {
        if (myID == 0)
        {
            Play_BlinkRed_Sound();
            Play_KillAmbiant_Sound();
            CallCameraShake(1);
            CallresetLimuleIncrementation();
            guetteurSpawner.CallAllBlinkRed();
        }
    }

    public void ActivatKillMode()
    {
        if (myID == 0)
        {
            guetteurSpawner.CallKillMode();
        }
    }


    public void ActivatBlinkRed()
    {
        animator.SetBool("BlinkRed", true);

    }

    // Assign unique ID to each instance
    public void SetID(int id, bool AmILast)
    {
        myID = id;
        gameObject.name = "Guetteur_" + id;
        isLastPrefab = AmILast;
        //print("Guetteur_ID" + id + "AmILast ?: " + AmILast);

    }

    public void ChangeListennerPosition()
    {
        if (myID == 0)
        {
            if (guetteurSpawner != null)
            {
                guetteurSpawner.CallChangeActiveListener(1);
            }
            else
            {
                Debug.LogError("guetteurSpawner is null");
            }
        }

    }

    IEnumerator ChangeActiveListenerIN(float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeListennerPosition();
    }



    // Mark as chosen one
    public void SetChosen(bool chosen)
    {
        isChosen = chosen;
    }

    // Animation event method
    public void OnAnimationEvent()
    {
        Play_RandomHarmonic_Sound(); // Call the method that checks if this prefab is chosen
    }
}

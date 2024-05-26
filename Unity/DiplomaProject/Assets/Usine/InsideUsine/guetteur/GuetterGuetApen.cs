using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuetterGuetApen : MonoBehaviour
{
    AudioSource[] sources;
    AudioSource OpenEye;
    AudioSource Harmonic01;
    AudioSource Harmonic02;
    AudioSource Harmonic03;
    AudioSource Harmonic04;

    private Animator animator;
    private bool playedOpenEyeOnce = false;
    private bool isChosen = false; // Flag to determine if this prefab is the chosen one

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        sources = GetComponents<AudioSource>();
        OpenEye = sources[0];
        Harmonic01 = sources[1];
        Harmonic02 = sources[2];
        Harmonic03 = sources[3];
        Harmonic04 = sources[4];
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
            }
            else
            {
                Debug.LogError("Random harmonic AudioSource is not assigned.");
            }
        }
    }

    public void Play_BlinkRed_Sound()
    {
        OpenEye.Play();
    }

    public void Play_KillMode_Sound()
    {
        OpenEye.Play();
    }

    public void LaunchHarmonic(float timeToLaunch)
    {
        print("LaunchHarmonic");
        StartCoroutine(LaunchHarmonicPhase(timeToLaunch));
    }

    IEnumerator LaunchHarmonicPhase(float seconds)
    {
        print("LaunchHarmonicPhase");
        yield return new WaitForSeconds(seconds);
        print("Launched");
        LaunchHarmonicAnimation();
    }

    public void LaunchHarmonicAnimation()
    {
        print("Guetteur IS BLINK White");
        animator.SetBool("BlinkWhite", true);
    }

    public void TurnONOpenEye()
    {
        animator.SetBool("OpenEye", true);
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
            animator.SetBool("OpenEye", true);
        }
    }

    // Assign unique ID to each instance
    public void SetID(int id)
    {
        gameObject.name = "Guetteur_" + id;
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

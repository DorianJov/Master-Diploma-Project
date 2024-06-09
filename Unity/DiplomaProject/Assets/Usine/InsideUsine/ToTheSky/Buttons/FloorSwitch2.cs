using UnityEngine;

public class FloorSwitch2 : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    private AudioSource[] audios;
    private AudioSource floorSwitchSound;

    bool Once = true;
    void Start()
    {
        animator = GetComponent<Animator>();
        audios = GetComponents<AudioSource>();
        floorSwitchSound = audios[0];
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Once)
            {
                animator.SetBool("turnColor", true);
                floorSwitchSound.Play();
                Once = false;
            }
        }
    }

    public void PlayJumpAudio()
    {
        audios[2].Play();
    }

}

using System.Collections;
using UnityEngine;


public class TigeInsideUsine : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;

    public GameObject Player;
    public bool active = false;

    public float LookAtSpeed = 0.1f;
    public float LookAtSpeedRestTarget = 0.5f;

    public float AngularThreshHold = 10f;
    public float AngularThreshHoldRestTarget = 10f;

    Transform circleChild;
    //Transform lightChild;
    public int prevrandomIndex = 0;
    public Transform target;
    public Transform RestTarget;
    public bool lookingAtGenerator = false;
    AudioSource[] sources;

    ParticleSystem myParticleSystem;
    private BoxCollider colliderComponent;




    void Start()
    {
        //Get default Parameters
        // Automatically find and assign the Player GameObject by name
        Player = GameObject.Find("LimuleTunnel");

        // Automatically find and assign the target Transform by name
        target = Player.transform;


        sources = this.gameObject.GetComponents<AudioSource>();
        circleChild = this.gameObject.transform;
        //lightChild = this.gameObject.transform.GetChild(0).GetChild(0);
        animator = GetComponent<Animator>();
        myParticleSystem = GetComponent<ParticleSystem>();

        // Get the Collider component attached to this GameObject
        colliderComponent = GetComponent<BoxCollider>();

        // Check if the Collider component exists
        if (colliderComponent != null)
        {
            // Collider component found, you can now use it
            // For example, you can access its properties or call its methods
            //Debug.Log("Collider component found!");
        }
        else
        {
            // Collider component not found
            Debug.LogError("Collider component not found!");
        }
        //gameObject.tag = "Player";
    }

    // Update is called once per frame
    void Update()
    {
        //circleChild.transform.Rotate(Vector3.right, 90f, Space.Self);

        if (active)
        {

            //StartCoroutine(LookAtSmoothly());


        }



    }

    private IEnumerator LookAtSmoothly(Transform target)
    {
        if (!active) { yield break; }

        while (active)
        {
            Quaternion lookRotation = Quaternion.LookRotation(target.position - circleChild.transform.position);
            circleChild.transform.rotation = Quaternion.Slerp(circleChild.transform.rotation, lookRotation, Time.deltaTime * LookAtSpeed);

            // Calculate the angular difference between the two quaternions
            float angularDifference = Quaternion.Angle(circleChild.transform.rotation, lookRotation);

            // If the angular difference is very small, consider the rotation as complete
            if (angularDifference < AngularThreshHold)
            {
                active = false;
                ToggleCollider(false);
                StartCoroutine(LookAtRestSmoothly(RestTarget));
                //Debug.Log("ENTER");
                animator.SetBool("switch", false);
                //sources[1].Play();
                //StartCoroutine(TurnOFFAudio());
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator LookAtRestSmoothly(Transform target)
    {

        if (active) { yield break; }

        while (!active)
        {
            Quaternion lookRotation = Quaternion.LookRotation(target.position - circleChild.transform.position);
            circleChild.transform.rotation = Quaternion.Slerp(circleChild.transform.rotation, lookRotation, Time.deltaTime * LookAtSpeedRestTarget);
            float angularDifference = Quaternion.Angle(circleChild.transform.rotation, lookRotation);

            if (angularDifference < AngularThreshHoldRestTarget)
            {
                ToggleCollider(true);
            }

            if (circleChild.transform.rotation == lookRotation)
            {
                //ToggleCollider(true);
                yield break;
            }
            yield return null;


        }

    }


    private void OnTriggerEnter(Collider other)
    {   //other.CompareTag("Player") & CheckSpeed() > 0
        //deleted check speed
        if (other.CompareTag("Player"))
        {
            if (!active)
            {
                //myParticleSystem.Play();
                PlayRandomAudio();
                //myParticleSystem.Play();
                Debug.Log("ENTER");
                active = true;
                StartCoroutine(LookAtSmoothly(target));
                //StartCoroutine(LookAtSmoothly(target, xPercentage, yPercentage, zPercentage));
                animator.SetBool("switch", true);
                //sources[0].Play();

            }
            else
            {

            }

        }

        if (other.CompareTag("Player"))
        {


        }
    }

    IEnumerator TurnOFFAudio()
    {
        AudioSource[] sources = this.gameObject.GetComponents<AudioSource>();
        while (sources[0].volume != 0)
        {
            if (active)
            {
                yield break; // Exit the coroutine immediately
            }
            //print("volume down");
            sources[0].volume -= Time.deltaTime * 0.1f;
            yield return null;
        }
    }

    private void PlayRandomAudio()
    {
        // Get all AudioSource components attached to this GameObject
        AudioSource[] sources = GetComponents<AudioSource>();
        sources[prevrandomIndex].Stop();
        // Check if there are AudioSource components
        if (sources.Length > 0)
        {
            // Generate a random index within the range of AudioSource array
            int randomIndex = Random.Range(0, sources.Length);
            prevrandomIndex = randomIndex;
            // Play the audio clip at the randomly selected index
            sources[randomIndex].Play();
            // Print the name of the currently played audio clip
            Debug.Log("Playing audio: " + sources[randomIndex].clip.name);
        }
        else
        {
            Debug.LogWarning("No AudioSource components found on this GameObject.");
        }
    }

    // Function to toggle the collider component
    public void ToggleCollider(bool enableCollider)
    {
        // Check if the Collider component exists
        if (colliderComponent != null)
        {
            // Enable or disable the collider component based on the enableCollider parameter
            colliderComponent.enabled = enableCollider;

            // Log the action
            if (enableCollider)
            {
                //Debug.Log("Collider turned on!");
            }
            else
            {
                //Debug.Log("Collider turned off!");
            }
        }
        else
        {
            // Collider component not found, cannot toggle collider
            Debug.LogError("Collider component not found! Cannot toggle collider.");
        }
    }

    float CheckSpeed()
    {
        // Check if pinceObject is not null and has the pinceScript component
        if (Player != null)
        {
            MoveSphereTunnel moveSphereTunnel = Player.GetComponent<MoveSphereTunnel>();
            if (moveSphereTunnel != null)
            {

                // Return the absolute value of the speed
                return Mathf.Abs(moveSphereTunnel.Speed);
            }
            else
            {
                Debug.LogError("MoveSphereTunnel component not found on moveSphereTunnel.");

            }
        }
        else
        {
            Debug.LogError("moveSphereTunnel is null.");
        }
        return 1;
    }
}

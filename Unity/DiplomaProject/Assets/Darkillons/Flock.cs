using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Flock : MonoBehaviour
{



    [Header("Spawn Setup")]
    [SerializeField] private FlockUnit flockUnitPrefab;
    [SerializeField] private int flockSize;
    [SerializeField] private Vector3 spawnBounds;
    [SerializeField] private Transform spawnOrigin;

    [Header("Speed Setup")]
    [Range(0, 2)]
    [SerializeField] private float _minSpeed;
    public float minSpeed { get { return _minSpeed; } }
    [Range(0, 2)]
    [SerializeField] private float _maxSpeed;
    public float maxSpeed { get { return _maxSpeed; } }

    public float distanceAdditionalSpeed = 0.1f;

    [Header("NoiseIntensity")]
    public float noiseFrequency = 1f; // Adjust this to control the frequency of noise
    public float noiseMagnitude = 0.1f;

    [Header("Target")]
    public GameObject ObjectToFollow;
    public Vector3 goalPos = Vector3.zero;
    public Vector3 assignedBasicPos = Vector3.zero;


    [Header("Detection Distances")]

    [Range(0, 200)]
    [SerializeField] private float _cohesionDistance;
    public float cohesionDistance { get { return _cohesionDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _avoidanceDistance;
    public float avoidanceDistance { get { return _avoidanceDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _aligementDistance;
    public float aligementDistance { get { return _aligementDistance; } }

    [Range(0, 10)]
    [SerializeField] private float _obstacleDistance;
    public float obstacleDistance { get { return _obstacleDistance; } }

    [Range(0, 100)]
    [SerializeField] private float _boundsDistance;
    public float boundsDistance { get { return _boundsDistance; } }


    [Header("Behaviour Weights")]

    [Range(0, 10)]
    [SerializeField] private float _cohesionWeight;
    public float cohesionWeight { get { return _cohesionWeight; } }

    [Range(0, 10)]
    [SerializeField] private float _avoidanceWeight;
    public float avoidanceWeight { get { return _avoidanceWeight; } }

    [Range(0, 10)]
    [SerializeField] private float _aligementWeight;
    public float aligementWeight { get { return _aligementWeight; } }

    [Range(0, 10)]
    [SerializeField] private float _boundsWeight;
    public float boundsWeight { get { return _boundsWeight; } }

    [Range(0, 100)]
    [SerializeField] private float _obstacleWeight;
    public float obstacleWeight { get { return _obstacleWeight; } }

    //public FlockUnit[] allUnits { get; set; }


    public List<FlockUnit> allUnits;


    private void Start()
    {

        allUnits = new List<FlockUnit>();
        //allUnits = new FlockUnit[flockSize];
        GenerateUnits();

    }



    public int howManyAreFollowing = 0;
    public bool darkillonsAreTargetingPlayer = false;

    public float interval = 3;
    float timer;
    //int count = -1;


    public float averageDistance = 0f;

    private void Update()
    {

        goalPos = ObjectToFollow.transform.position;


        for (int i = 0; i < allUnits.Count; i++)
        {
            allUnits[i].MoveUnit();
        }


        float totalDistance = 0f;
        for (int i = 0; i < allUnits.Count; i++)
        {
            totalDistance += allUnits[i].distance;
        }

        averageDistance = totalDistance / allUnits.Count;
        //Debug.Log("Average Distance: " + averageDistance);


    }

    int count;
    private void GenerateUnits()
    {


        if (allUnits.Count >= flockSize)
        {
            Debug.Log("NO MORE SHRIMPS");
            return;
        }

        count++;





        for (int i = 0; i < flockSize; i++)
        {
            var randomVector = UnityEngine.Random.insideUnitSphere;
            randomVector = new Vector3(randomVector.x * spawnBounds.x, randomVector.y * spawnBounds.y, randomVector.z * spawnBounds.z);
            var spawnPosition = transform.position + randomVector;

            // Override of spawn position by Raphael
            //randomVector = GetRandomVectorPosition();
            //spawnPosition = spawnOrigin != null ? spawnOrigin.position + randomVector : transform.position + randomVector;

            var rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);


            assignedBasicPos = spawnPosition;


            FlockUnit tempVariable = Instantiate(flockUnitPrefab, spawnPosition, rotation);
            tempVariable.AssignFlock(this);
            tempVariable.InitializeSpeed(UnityEngine.Random.Range(minSpeed, maxSpeed));

            allUnits.Add(tempVariable);
        }

        Debug.Log(allUnits.Count);


    }

    private Vector3 GetRandomVectorPosition()
    {
        var angleY = Random.Range(0f, 360f);
        var angleX = Random.Range(-45f, 45f);
        var length = Random.Range(0.45f, 0.7f);

        var vector = Vector3.forward;
        var rot = Quaternion.Euler(angleX, angleY, 0f);
        var randomPosition = rot * vector;  // rotates the direction
        return randomPosition * length;
    }

}
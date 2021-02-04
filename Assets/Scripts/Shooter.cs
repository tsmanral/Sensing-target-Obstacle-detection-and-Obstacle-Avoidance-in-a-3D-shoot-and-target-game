using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class Shooter : Agent{
    public GameObject firePoint;
    public GameObject ballPrefab;
    public float power;
    public float rotationSpeed;
    public bool isCounting = false;

    private int minStepsBetweenShots = 50;
    private bool ShotAvaliable = true;

    Vector3 currentPosition;
    Quaternion currentRotation;
    
    private EnvironmentParameters EnvironmentParameters;

    public event Action OnEnvironmentReset;

    public override void Initialize(){
        currentPosition = transform.position;
        currentRotation = transform.rotation;

        StateManager.instance.currentScore.propertyUpdated += onHit;

        StateManager.instance.currentMiss.propertyUpdated += onMiss;
        
        EnvironmentParameters = Academy.Instance.EnvironmentParameters;
    }

    
    public Vector3 calculateForce(){
        return transform.forward * power;
    }

    async void shoot(){
        if (!ShotAvaliable)
            return;

        var layerMask = 1 << LayerMask.NameToLayer("Enemy");
        var direction = transform.forward;
        
        GameObject ball = Instantiate(ballPrefab, firePoint.transform.position, Quaternion.identity);
        ball.GetComponent<Rigidbody>().AddForce(calculateForce(), ForceMode.Impulse);

        isCounting = true;

        await WaitFiveSecondAsync();

        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        await WaitTwoSecondAsync();
        
        transform.position = ball.GetComponent<BallPosition>().finalPos;
        currentPosition = transform.position;
        currentRotation = transform.rotation;

        isCounting = false;

        StateManager.instance.shoot();

        Destroy(ball);
    }

    private async Task WaitTwoSecondAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        Debug.Log("Finished 2 sec waiting.");
    }

    private async Task WaitFiveSecondAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(5));
        Debug.Log("Finished 5 sec waiting.");
    }

    void Update(){
        float vertical = Input.GetAxis ("Vertical");
        float Horizontal = Input.GetAxis ("Horizontal");

        transform.Rotate(
            -vertical * rotationSpeed, 
            Horizontal * rotationSpeed, 
            0.0f
        );

        if(currentRotation != transform.rotation){
           predict();
        }   

        if(currentPosition != transform.transform.position){
           predict();
        }       
    
        currentRotation = transform.rotation;

        if(Input.GetKeyUp(KeyCode.Space)){
            if(isCounting == false ){
                shoot();
            }
        }
    }

    void predict(){
        if(isCounting == false)
        {
            PredictionManager.instance.predict(ballPrefab, firePoint.transform.position, calculateForce());
        }
    }

    void onHit(int v){
        AddReward(0.3f);
        Debug.Log("Nice Shot!!");
    }

    void onMiss(int v){
        AddReward(-1f);
        EndEpisode();
        Debug.Log("Miss !!");
    }


    public override void OnActionReceived(float[] vectorAction){
        
        if (Mathf.RoundToInt(vectorAction[0]) >= 1)
        {
            //shoot();
        }

        transform.Rotate(
            -vectorAction[3] * rotationSpeed, 
            vectorAction[2] * rotationSpeed, 
            0.0f
        );
    }

    public override void Heuristic(float[] actionsOut){
        actionsOut[0] = Input.GetKey(KeyCode.Space) ? 1f : 0f;
        actionsOut[2] = Input.GetAxis("Horizontal");
        actionsOut[3] = Input.GetAxis("Vertical");
    }

    public override void CollectObservations(VectorSensor sensor){
        //sensor.AddObservation(transform.rotation);
        sensor.AddObservation(ShotAvaliable);
    }
    
    public override void OnEpisodeBegin(){
        
        //minStepsBetweenShots = Mathf.FloorToInt(EnvironmentParameters.GetWithDefault("shootingFrequency", 30f));

        predict();

        transform.position = currentPosition;
        transform.rotation = currentRotation;
        StateManager.instance.currentScore.val = 0;
        StateManager.instance.currentShoots.val = 0;
        ShotAvaliable = true;

        OnEnvironmentReset?.Invoke();
    }

}

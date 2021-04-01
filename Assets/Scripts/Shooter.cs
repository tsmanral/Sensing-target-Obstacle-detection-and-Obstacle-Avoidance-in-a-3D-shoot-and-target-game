using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class Shooter : Agent{
    public GameObject ballPrefab;
    public float power;
    public float rotationSpeed;
    public bool isCounting = false;

    private int minStepsBetweenShots = 50;
    private bool ShotAvaliable = true;

    private bool ShotIsReady = true;

    Vector3 currentPosition;
    Quaternion currentRotation;
    
    private EnvironmentParameters EnvironmentParameters;

    public event Action OnEnvironmentReset;

    public override void Initialize(){
        currentPosition = transform.position;
        currentRotation = transform.rotation;

        StateManager.instance.currentScore.propertyUpdated += onHit;

        StateManager.instance.currentMiss.propertyUpdated += onMiss;

        StateManager.instance.currentBounce.propertyUpdated += onBounce;

        StateManager.instance.currentObstacle.propertyUpdated += onObstacle;
        
        EnvironmentParameters = Academy.Instance.EnvironmentParameters;

        //shoot();
    }

    
    public Vector3 calculateForce(){
        return transform.forward * power;
    }

    async void shoot(){
        if (!ShotAvaliable)
            return;

        var direction = transform.forward;

        Debug.DrawRay(transform.position, direction, Color.blue, 50f);

        if(ShotIsReady){
            
            GameObject ball = Instantiate(ballPrefab, this.transform.position, Quaternion.identity);
            ball.GetComponent<Rigidbody>().AddForce(calculateForce(), ForceMode.Impulse);
            Debug.DrawRay(transform.position, direction, Color.blue, 50f);

            isCounting = true;

            await WaitFiveSecondAsync();

            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            await WaitTwoSecondAsync();
            
            transform.position = ball.GetComponent<BallPosition>().finalPos;
            //currentPosition = transform.position;
            //currentRotation = transform.rotation;

            isCounting = false;

            ShotIsReady = false;

            Destroy(ball);

            StateManager.instance.shoot();


            // Removed for AI training
            // predict();
        }
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

    void FixedUpdate(){

        /*if(ShotIsReady){
            RequestDecision();
        }*/

        // Removed for AI training
        /*
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
        */    
    
        //currentRotation = transform.rotation;

        /*
        if(Input.GetKeyUp(KeyCode.Space)){
            if(isCounting == false ){
                shoot();
            }
        }
        */
    }

    // Removed for AI training
    /*
    void predict(){
        if(isCounting == false)
        {
            PredictionManager.instance.predict(ballPrefab, this.transform.position, calculateForce());
        }
    } 
    */

    // When Ball hits the Goal
    void onHit(int v){
        AddReward(0.3f);
        ShotIsReady = true;
    }

    // When Ball misses the Goal
    void onMiss(int v){
        AddReward(-1f);
        EndEpisode();
    }

    // When Ball hits any of the "Walls"
    void onBounce(int v){
        AddReward(0.1f);
    }

    // When Ball hits any of the "Obstacle"
    void onObstacle(int v){
        AddReward(-0.1f);
    }

    public override void OnActionReceived(float[] vectorAction){

        /*
        for (var i = 0; i < vectorAction.Length; i++)
        {
            vectorAction[i] = Mathf.Clamp(vectorAction[i], -1f, 1f);
        }
        
         
        if (Mathf.FloorToInt(vectorAction[1]) == 1)
        {
            shoot();
        }

        transform.Rotate(
            -vectorAction[2] * rotationSpeed, 
            vectorAction[0] * rotationSpeed, 
            0.0f
        );
        */
        

        // Removed for AI training
        // predict(); 


 
        var action = Mathf.FloorToInt(vectorAction[0]);
 
        switch (action)
        {
            case 1:
                transform.Rotate(Vector3.left * Time.deltaTime * rotationSpeed, Space.World);
                break;
            case 2:
                transform.Rotate(Vector3.right * Time.deltaTime * rotationSpeed, Space.World);
                break;
            case 3:
                transform.Rotate(Vector3.down * Time.deltaTime * rotationSpeed, Space.World);
                break;
            case 4:
                transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
                break;
            case 5:
                shoot();
                break;
        }

        AddReward(-1f / MaxStep);
    }

    public override void Heuristic(float[] actionsOut){
        /* actionsOut[0] = Input.GetAxisRaw("Horizontal");
        actionsOut[1] = Input.GetKey(KeyCode.Space) ? 1.0f : 0.0f;
        actionsOut[2] = Input.GetAxisRaw("Vertical"); */

        actionsOut[0] = 0;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            actionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            actionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            actionsOut[0] = 3;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            actionsOut[0] = 2;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            actionsOut[0] = 5;
        }
    }

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(this.transform.rotation.z);
        sensor.AddObservation(this.transform.rotation.y);
        sensor.AddObservation(this.transform.rotation.x);
        sensor.AddObservation(this.transform.position);
        sensor.AddObservation(GameObject.FindWithTag("Goal").transform.position);
        sensor.AddObservation(GameObject.FindWithTag("Goal").transform.position - this.transform.position);
    }
    
    public override void OnEpisodeBegin(){
        Reset();
    }

    private void Reset()
    {
        ShotIsReady = true;
        
        OnEnvironmentReset?.Invoke();

        //minStepsBetweenShots = Mathf.FloorToInt(EnvironmentParameters.GetWithDefault("shootingFrequency", 30f));

        // Removed for AI training
        // predict();

        //transform.position = currentPosition;
        //transform.rotation = currentRotation;

        ShotAvaliable = true;
    }

}

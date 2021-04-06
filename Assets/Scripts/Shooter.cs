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
    private bool isCounting = false;

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
        
        //EnvironmentParameters = Academy.Instance.EnvironmentParameters;

        //shoot();
    }

    
    public Vector3 calculateForce(){
        return transform.forward * power;
    }

    async void shoot(){
        //Debug.Log("Shoot called");

        GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        ball.GetComponent<Rigidbody>().AddForce(calculateForce(), ForceMode.Impulse);

        isCounting = true;

        await WaitOneSecondAsync();

        /*
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        await WaitTwoSecondAsync();
            
        transform.position = ball.GetComponent<BallPosition>().finalPos;
        //currentPosition = transform.position;
        //currentRotation = transform.rotation;
        */
        

        DestroyImmediate(ball);

        StateManager.instance.movePlayer();

        StateManager.instance.shoot();
        
        isCounting = false;

        // Removed for AI training
        // predict();
    }

    private async Task WaitTwoSecondAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
    }

    private async Task WaitOneSecondAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
    }

    void Update(){

        /*if(ShotIsReady){
            RequestDecision();
        }*/
        if(isCounting == false ){
            RequestDecision();
        }

        //Debug.Log(GetCumulativeReward().ToString("0.00"));
        

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
    }

    // When Ball misses the Goal
    void onMiss(int v){
        AddReward(-0.1f);
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

        
        for (var i = 0; i < vectorAction.Length; i++)
        {
            vectorAction[i] = Mathf.Clamp(vectorAction[i], -1f, 1f);
        }
        
         
        if (Mathf.FloorToInt(vectorAction[1]) == 1)
        {
                
                    Debug.Log("Space bar is clicked");
                    shoot();
                
            
        }

        transform.Rotate(
            -vectorAction[2] * Time.deltaTime * rotationSpeed, 
            vectorAction[0] * Time.deltaTime * rotationSpeed, 
            0.0f, Space.World
        );
        

        // Removed for AI training
        // predict(); 


        /*
        var action = Mathf.FloorToInt(vectorAction[0]);
 
        switch (action)
        {
            case 1:
                if(isCounting == false ){
                    Debug.Log("Space bar is clicked");
                    shoot();
                }
                break;
            case 2:
                transform.Rotate(Vector3.left * Time.deltaTime * rotationSpeed, Space.World);
                break;
            case 3:
                transform.Rotate(Vector3.right * Time.deltaTime * rotationSpeed, Space.World);
                break;
            case 4:
                transform.Rotate(Vector3.down * Time.deltaTime * rotationSpeed, Space.World);
                break;
            case 5:
                transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
                break;
        }
        */

        AddReward(-1f / MaxStep);
    }

    public override void Heuristic(float[] actionsOut){
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetKeyDown(KeyCode.Space) ? 1.0f : 0.0f;
        actionsOut[2] = Input.GetAxis("Vertical"); 

        /* 
        actionsOut[0] = 0;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            actionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            actionsOut[0] = 5;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            actionsOut[0] = 2;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            actionsOut[0] = 4;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            actionsOut[0] = 3;
        }
        */
        
    }

    public override void CollectObservations(VectorSensor sensor){
        //sensor.AddObservation(this.transform.rotation.z);
        //sensor.AddObservation(this.transform.rotation.y);
        //sensor.AddObservation(this.transform.rotation.x);
        //sensor.AddObservation(this.transform.position);
        //sensor.AddObservation(GameObject.FindWithTag("Goal").transform.position);
        //sensor.AddObservation(GameObject.FindWithTag("Goal").transform.position - gameObject.transform.position);
        //sensor.AddObservation(ShotAvaliable);
    }
    
    public override void OnEpisodeBegin(){
        Reset();
    }

    private void Reset()
    {
        
        //OnEnvironmentReset?.Invoke();

        StateManager.instance.Respawn();

        // Removed for AI training
        // predict();

        //transform.position = currentPosition;
        //transform.rotation = currentRotation;

        ShotAvaliable = true;

        isCounting = false; 

    }

}

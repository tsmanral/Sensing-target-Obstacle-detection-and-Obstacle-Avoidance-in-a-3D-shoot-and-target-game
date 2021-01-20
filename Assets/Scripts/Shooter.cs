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

    Vector3 currentPosition;
    Quaternion currentRotation;
    
    private EnvironmentParameters EnvironmentParameters;

    public event Action OnEnvironmentReset;

    public override void Initialize(){
        currentPosition = transform.position;
        currentRotation = transform.rotation;
        predict();

        StateManager.instance.currentScore.propertyUpdated += onHit;
        EnvironmentParameters = Academy.Instance.EnvironmentParameters;
    }

    
    public Vector3 calculateForce(){
        return transform.forward * power;
    }

    async void shoot(){
        GameObject ball = Instantiate(ballPrefab, firePoint.transform.position, Quaternion.identity);
        ball.GetComponent<Rigidbody>().AddForce(calculateForce(), ForceMode.Impulse);
        StateManager.instance.shoot();

        isCounting = true;

        await WaitFiveSecondAsync();

        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        await WaitTwoSecondAsync();
        
        transform.position = ball.GetComponent<BallPosition>().finalPos;

        isCounting = false;

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
        Debug.Log("Nice Shot!!");
    }


    //public override void OnActionReceived(float[] vectorAction){

    //}

    //public override void Heuristic(float[] actionsOut){

    //}

    //public override void CollectObservations(VectorSensor sensor){

    //}
    
    //public override void OnEpisodeBegin(){

    //}

}

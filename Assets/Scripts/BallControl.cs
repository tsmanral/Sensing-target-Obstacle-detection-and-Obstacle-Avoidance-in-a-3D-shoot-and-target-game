using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    
    public float moveSpeed = 10f;

    private Renderer rend;
    private Light myLight;
    private Rigidbody ball;

    public Vector3 velocity = Vector3.zero * 0.1f;
    float smoothtime = 0.25f;

    float shootPower = 40f;

    GoalManager goal;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        myLight = GetComponent<Light>();
        goal = GameObject.FindObjectOfType<GoalManager>();
    }

    public void ThrowBall (Rigidbody ball)
    {
        float inputX = Camera.main.transform.position.x;
        float inputZ = Camera.main.transform.position.y;

        float moveX = inputX*moveSpeed*Time.deltaTime;
        float moveZ = inputZ*moveSpeed*Time.deltaTime;
       
        ball = ball;
        //ball.transform.position = ball.transform.position + Camera.main.transform.forward * 2;
        // ball.velocity = Camera.main.transform.forward * shootPower; 
            
        ball.AddForce(Camera.main.transform.forward * shootPower , ForceMode.VelocityChange);
    }

    void OnCollisionEnter(Collision col)
    {
        // col = col.contacts[0]; 
        //print (col.collider.name);
        if(col.collider.name == "GoalManager")
        {
            goal.isGoal = true;
        }
        else if(col.collider.name == "wallLeft")
        {
            rend.material.color = Color.blue;
            myLight.color = Color.blue;
        }
        else if(col.collider.name == "wallRight")
        {
            rend.material.color = Color.red;
            myLight.color = Color.red;
        }
        else if(col.collider.name == "wallFront")
        {
            rend.material.color = Color.green;
            myLight.color = Color.green;
        }
        else if(col.collider.name == "wallBack")
        {
            rend.material.color = Color.yellow;
            myLight.color = Color.yellow;
        }
        else if(col.collider.name == "floor")
        {
            rend.material.color = Color.magenta;
            myLight.color = Color.magenta;
        }
        else if(col.collider.name == "ceiling")
        {
            rend.material.color = Color.black;
            myLight.color = Color.black;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPosition : MonoBehaviour
{
    [HideInInspector] public Vector3 finalPos;
    

    // Start is called before the first frame update
    void Start()
    {
        Physics.sleepThreshold = 0.2f;
        Invoke("posUpdate", 3);
    }

    // Update is called once per frame
    void posUpdate()
    { 
        // Change Fireposition w.r.t change in ball position.

        if(this.GetComponent<Rigidbody>().velocity == Vector3.zero && this.GetComponent<Rigidbody>().angularVelocity.magnitude == 0)
        {
            finalPos = this.transform.position;
        }
    }

    private void OnCollisionEnter(Collision collidedObj)
    {
        if (collidedObj.gameObject.CompareTag("Walls")){
            StateManager.instance.addBounces();
        }
        else if(collidedObj.gameObject.CompareTag("Obstacle")){
            StateManager.instance.addObstacles();
        }
    }
}

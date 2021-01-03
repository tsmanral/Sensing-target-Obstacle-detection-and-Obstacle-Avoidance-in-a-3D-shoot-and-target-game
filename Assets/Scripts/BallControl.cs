using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    float xRot, yRot = 0f;

    public Rigidbody ball;
    
    private Renderer rend;
    private Light myLight;

    public float rotationSpeed = 5f;

    public float shootPower =  30f; 
 


    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        myLight = GetComponent<Light>();         
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ball.position;

        if(Input.GetMouseButton(0))
        {
            xRot += Input.GetAxis("Mouse X") * rotationSpeed;
            yRot += Input.GetAxis("Mouse Y") * rotationSpeed;
            transform.rotation = Quaternion.Euler(yRot, xRot, 0f);
        }

        if(Input.GetMouseButtonUp(0))
        {
            ball.velocity = Camera.main.transform.forward * shootPower;
        }
        
    }

    void OnCollisionEnter(Collision col)
    {
        print (col.collider.name);
        if(col.collider.name == "wallLeft")
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ball : MonoBehaviour
{
    
    public float moveSpeed = 10f;

    private Rigidbody rbody;
    private Renderer rend;
    private Light myLight;
    private Transform cameraTransform;

    Vector3 velocity = Vector3.zero * 0.1f;
    float smoothtime = 0.25f;

    public GameObject prefab;

    // Start is called before the first frame update
    void Start()
    {
        
        rend = GetComponent<Renderer>();
        myLight = GetComponent<Light>();        
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        float moveX = inputX*moveSpeed*Time.deltaTime;
        float moveZ = inputZ*moveSpeed*Time.deltaTime;

        if(Input.GetMouseButton(0))
        {
            GameObject projectile = Instantiate(prefab) as GameObject;
            projectile.transform.position = transform.position + Camera.main.transform.forward * 2;
            rbody = projectile.GetComponent<Rigidbody>();
            rbody.velocity = Camera.main.transform.forward * 40; 
            
            //this.GetComponent<Rigidbody> ().AddForce (Vector3.left * 10000.0f);
            
            /*this.transform.position = Vector3.SmoothDamp(
                this.transform.position, 
                new Vector3(transform.position.x + cameraTransform.forward.x * 2, transform.position.y + cameraTransform.forward.y * 2, transform.position.z + cameraTransform.forward.z * 2), 
                ref velocity,
                smoothtime);
            */

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

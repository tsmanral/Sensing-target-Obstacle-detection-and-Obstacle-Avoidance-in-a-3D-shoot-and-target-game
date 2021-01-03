using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballmovement : MonoBehaviour
{
    private Rigidbody rbody;
    private Renderer rend;
    private Light myLight;

    Vector3 velocity = Vector3.zero * 0.1f;
    float smoothtime = 0.25f;

    public GameObject prefab;
    private const float DESTROY_TIME = 0.0001f;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        myLight = GetComponent<Light>(); 
    }

    // Update is called once per frame
    void Update()
    {
       PlayerMovement();
    }

    void PlayerMovement()
    {
        if(Input.GetMouseButton(0))
        {
            //GameObject projectile = Instantiate(prefab) as GameObject;
            prefab.transform.position = transform.position + Camera.main.transform.forward * 2;
            rbody = prefab.GetComponent<Rigidbody>();
            rbody.velocity = Camera.main.transform.forward * 40; 
            //Destroy(this.gameObject,DESTROY_TIME);
            /* this.transform.position = Vector3.SmoothDamp(
                this.transform.position, 
                new Vector3(transform.position.x + Camera.main.transform.forward.x * 2, transform.position.y + Camera.main.transform.forward.y * 2, transform.position.z + Camera.main.transform.forward.z * 2), 
                ref velocity,
                smoothtime); */
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

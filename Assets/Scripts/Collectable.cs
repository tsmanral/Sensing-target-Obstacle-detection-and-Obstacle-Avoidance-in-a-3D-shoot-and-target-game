using UnityEngine;

public class Collectable : MonoBehaviour{
    
    // Increment the score when the ball toched the Goal.
    void OnTriggerEnter(Collider other) {
        StateManager.instance.addPoint();
        //Destroy(gameObject);
    }

    void Update(){
    }
}

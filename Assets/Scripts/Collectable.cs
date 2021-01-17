using UnityEngine;

public class Collectable : MonoBehaviour{
    

    void OnTriggerEnter(Collider other) {
        StateManager.instance.addPoint();
        Destroy(gameObject);
    }

    void Update(){
    }
}

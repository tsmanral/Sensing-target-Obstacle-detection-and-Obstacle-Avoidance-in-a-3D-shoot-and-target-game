using UnityEngine;

public class CameraController : MonoBehaviour{
    public Transform pivot;
    public float sensitivity = 10f;

    public bool autoRotate = false;

    bool move = false;
    bool moveClock = false;
    float offset = 0f;

    void Update(){
        move = Input.GetMouseButton(1);
        offset = Input.GetAxis("Mouse X");

        if(autoRotate){
            if(move == true)
            {
                moveClock = true;
                move = false;
            }
            else
            {
                move = true;
                moveClock = false;
            }
            offset = 0.5f;
        }
        if(Input.GetKeyUp(KeyCode.C))
        {
            if(!autoRotate)
            {
                autoRotate = true;
            }
            else
            {
                autoRotate = false;
            }
        }
    }

    void LateUpdate (){
        if(move){
            transform.RotateAround(pivot.position, Vector3.up, offset * sensitivity * Time.deltaTime);
        }
        if(moveClock){
            transform.RotateAround(pivot.position, Vector3.down, offset * sensitivity * Time.deltaTime);
        }
    }
}

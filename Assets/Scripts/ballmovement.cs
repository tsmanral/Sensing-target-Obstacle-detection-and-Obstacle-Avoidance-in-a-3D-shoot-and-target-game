using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{

    BallControl controlBall;

    StateManager theStateManager;

    BallStorage storedBalls;

    // Start is called before the first frame update
    void Start()
    {
        theStateManager = GameObject.FindObjectOfType<StateManager>();
        if(controlBall != null){
            controlBall = GameObject.FindObjectOfType<BallControl>();            
        }
        controlBall = GameObject.FindObjectOfType<BallControl>();
        storedBalls = GameObject.FindObjectOfType<BallStorage>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(storedBalls.transform.GetChild(theStateManager.CurrentPlayerId).transform.childCount == 0)
                {
                    Debug.Log("Player" + theStateManager.CurrentPlayerId + "not available");
                    return;
                }
            
            controlBall.ThrowBall(storedBalls.transform.GetChild(theStateManager.CurrentPlayerId).GetComponentInChildren<Rigidbody>());
            theStateManager.IsDoneClicking = true;
        }  
    }
}

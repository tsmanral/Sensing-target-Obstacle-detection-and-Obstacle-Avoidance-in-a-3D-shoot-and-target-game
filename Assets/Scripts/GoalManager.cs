using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{    
    
    StateManager theStateManager;
    
    // Start is called before the first frame update
    void Start()
    {
        theStateManager = GameObject.FindObjectOfType<StateManager>();
    }

    // Update is called once per frame
    public void ShowWinner()
    {
        if(theStateManager.IsGoal)
        {

        }
    }
}

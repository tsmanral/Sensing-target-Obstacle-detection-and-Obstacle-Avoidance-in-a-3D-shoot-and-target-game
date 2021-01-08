using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        goal = GameObject.FindObjectOfType<GoalManager>();
    }

    public int NumberOfPlayers = 2;
    public int CurrentPlayerId = 0;

    public bool IsDoneClicking = false;
    public bool IsDoneAnimating = false;

    GoalManager goal;

    CameraController controlledCamera; 

    public void NewTurn()
    {
        IsDoneClicking = false;
        IsDoneAnimating = false;

        CurrentPlayerId = (CurrentPlayerId + 1) % NumberOfPlayers; 
    }


    // Update is called once per frame
    void Update()
    {
        
        // Is the turn done?
        if(IsDoneClicking && IsDoneAnimating && !goal.isGoal)
        {
            Debug.Log("Turn is done!");
            NewTurn();
        }
        else if(IsDoneClicking && IsDoneAnimating && goal.isGoal)
        {
            goal.GameComplete(CurrentPlayerId);
            goal.isGoal = false;
            foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) {
                Destroy(o);
            } 
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public bool isGoal = false;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void GameComplete(int CurrentPlayerId)
    {
        Debug.Log("Goal! by" + CurrentPlayerId);
    }
}

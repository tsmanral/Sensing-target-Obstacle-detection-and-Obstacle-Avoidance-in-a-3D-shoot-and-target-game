﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public int NumberOfPlayers = 2;
    public int CurrentPlayerId = 0;

    public bool IsDoneClicking = false;
    public bool IsDoneAnimating = false;

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
        if(IsDoneClicking && IsDoneAnimating)
        {
            Debug.Log("Turn is done!");
            NewTurn();
        }
    }
}

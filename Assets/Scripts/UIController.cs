using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour{

    public TextMeshProUGUI ScoreCard;
    public TextMeshProUGUI HitCard;

    void Start(){
        StateManager.instance.currentScore.propertyUpdated += onHit;
        StateManager.instance.currentMiss.propertyUpdated += onMiss;
    }

    // Update the Score card to the score 
    void onHit(int v){
        ScoreCard.text = v.ToString();
        HitCard.text = "Nice Shot!!";
        Invoke("ScoreUpdate", 1);
    }

    void onMiss(int v){
        HitCard.text = "Miss";
        Invoke("ScoreUpdate", 1);
    }

    void ScoreUpdate()
    {
        HitCard.text = "";
    }
}

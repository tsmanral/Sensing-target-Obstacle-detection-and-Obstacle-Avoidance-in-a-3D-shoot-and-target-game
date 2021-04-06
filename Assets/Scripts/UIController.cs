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
        Invoke("ScoreUpdate", 0.1f);
    }

    // Update the Score card to the score 
    void onHit(int v){
        ScoreCard.text = v.ToString();
        HitCard.text = "Nice Shot!!";
        Invoke("ScoreUpdate", 0.5f);
    }

    void onMiss(int v){
        ScoreCard.text = "0";
        HitCard.text = "Miss";
        Invoke("ScoreUpdate", 0.5f);
    }

    void ScoreUpdate()
    {
        ScoreCard.text = "0";
        HitCard.text = "";
    }
}

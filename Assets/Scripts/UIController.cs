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
    }

    void onHit(int v){
        ScoreCard.text = v.ToString();
        HitCard.text = "Nice Shot!!";
    }
}

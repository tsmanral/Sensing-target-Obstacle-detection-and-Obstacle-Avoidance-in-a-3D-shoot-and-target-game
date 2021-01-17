using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour{

    public TextMeshProUGUI ScoreCard;
    public TextMeshProUGUI HitCard;

    void Start(){
        StateManager.instance.currentShoots.propertyUpdated += onShoot;
        StateManager.instance.currentScore.propertyUpdated += onHit;
    }

    void onShoot(int v){
        ScoreCard.text = v.ToString();
    }

    void onHit(int v){
        HitCard.text = "Nice Shot!!";
    }
}

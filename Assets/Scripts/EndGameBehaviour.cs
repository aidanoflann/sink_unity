using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameBehaviour : MonoBehaviour {

    private StatManagerBehaviour statManagerBehaviour;
    private Text scoreNumberText;

	// Use this for initialization
	void Start () {
        this.statManagerBehaviour = FindObjectOfType<StatManagerBehaviour>();
        GameObject scoreNumberTextGO = GameObject.Find("ScoreNumber");
        this.scoreNumberText = scoreNumberTextGO.GetComponent<Text>();
        this.scoreNumberText.text = this.statManagerBehaviour.GetTotalScore().ToString();
        this.statManagerBehaviour.Log();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

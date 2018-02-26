using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameManagerBehaviour : MonoBehaviour {

    private StatManagerBehaviour statManagerBehaviour;

	// Use this for initialization
	void Start () {
        this.statManagerBehaviour = FindObjectOfType<StatManagerBehaviour>();
        this.statManagerBehaviour.Log();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

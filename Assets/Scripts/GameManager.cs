using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public LevelManager levelManager;

	void Awake () {
		levelManager = GetComponent<LevelManager> ();
		InitGame ();
	}

	void InitGame()
	{
		levelManager.SetupScene (10);
	}

}

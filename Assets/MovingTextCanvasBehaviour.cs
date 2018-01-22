using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTextCanvasBehaviour : MonoBehaviour {

    public GameObject movingTextPrefab;
    private List<MovingTextBehaviour> movingTextBehaviours = new List<MovingTextBehaviour>();
    private List<GameObject> movingTextGameObjects = new List<GameObject>();

	// Use this for initialization
	void Start () {
        SpawnMovingTexts(1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void SpawnMovingTexts(int numToSpawn)
    {
        for (int i = 0; i < numToSpawn; i++)
        {
            // instansiate the movingText gameobject and set it as a child of this transform
            GameObject newMovingText = GameObject.Instantiate(this.movingTextPrefab);
            MovingTextBehaviour movingTextBehaviour = newMovingText.GetComponent<MovingTextBehaviour>();
            newMovingText.transform.SetParent(this.transform);

            // store references to the gameobject and its behaviour
            this.movingTextGameObjects.Add(newMovingText);
            this.movingTextBehaviours.Add(movingTextBehaviour);

            // Set start and end points
            movingTextBehaviour.SetStartAndEndPoints(new Vector3(1000, 1000, 1), new Vector3(0, 0, 1));
        }
    }

    public void ResetAnimations()
    {
        for (int i = 0; i < this.movingTextBehaviours.Count; i++)
        {
            this.movingTextBehaviours[i].Restart();
        }
    }
}

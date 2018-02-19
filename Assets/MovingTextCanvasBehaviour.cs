using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTextCanvasBehaviour : MonoBehaviour {

    public GameObject movingTextPrefab;

    private List<MovingTextBehaviour> movingTextBehaviours = new List<MovingTextBehaviour>();
    private List<GameObject> movingTextGameObjects = new List<GameObject>();
    private string[] wordsToDisplay;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        // update children text's positions
        for (int i = 0; i < this.movingTextBehaviours.Count; i++)
        {
            movingTextBehaviours[i].UpdatePosition(Time.deltaTime);
        }
    }

    private void SpawnMovingTexts(int numToSpawn)
    {
        for (int i = 0; i < numToSpawn; i++)
        {
            // instansiate the movingText gameobject and set it as a child of this transform
            // NOTE: set parent on instantiate to avoid your positions getting jiggled.
            GameObject newMovingText = GameObject.Instantiate(this.movingTextPrefab, this.transform);
            MovingTextBehaviour movingTextBehaviour = newMovingText.GetComponent<MovingTextBehaviour>();

            // store references to the gameobject and its behaviour
            this.movingTextGameObjects.Add(newMovingText);
            this.movingTextBehaviours.Add(movingTextBehaviour);
        }
    }

    public void SetText(string textToDisplay)
    {
        // break the text into substrings
        this.wordsToDisplay = textToDisplay.Split(' ');
        // maybe move this inside SpawnMovingText
        int amountToSpawn = this.wordsToDisplay.Length - this.movingTextBehaviours.Count;
        // spawn more if we need
        this.SpawnMovingTexts(amountToSpawn);
        for (int i = 0; i < this.wordsToDisplay.Length; i++)
        {
            if (i < this.movingTextBehaviours.Count)
            {
                this.movingTextBehaviours[i].SetText(this.wordsToDisplay[i]);
            }
        }
    }

    public void AnimateToPoint(Vector3 point)
    // Set up animation params (and start animation) towards given game-space point
    {
        for(int i = 0; i < this.movingTextBehaviours.Count; i++)
        {
            // TODO: randomise start point? currently just comes from the blackhole.
            movingTextBehaviours[i].SetStartAndEndPoints(new Vector3(1000, 1000, -1), point);
            movingTextBehaviours[i].SetWarmUpTime((float)i);
            movingTextBehaviours[i].SetSitStillTime((float)(i + 1f));
        }
    }

    private void ResetAnimations()
    {
        for (int i = 0; i < this.movingTextBehaviours.Count; i++)
        {
            this.movingTextBehaviours[i].Restart();
        }
    }

    public void SetTextColor(Color colour)
    {
        for (int i = 0; i < this.movingTextBehaviours.Count; i++)
        {
            this.movingTextBehaviours[i].SetTextColor(colour);
        }
    }
}

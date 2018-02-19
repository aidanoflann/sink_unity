using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingTextBehaviour : MonoBehaviour {

    public delegate void TextArrivedAction();
    public static event TextArrivedAction OnTextArrive;

    private string TextToDisplay = "!NN!";

    // Animation
    private Vector3 animationStartingPoint;
    private Vector3 animationTargetPoint;
    private static float startingAnimationSpeed = 3.0f;
    private float animationSpeed = startingAnimationSpeed;
    private float animationAcceleration = 2f;
    private float fracDistanceCovered = 0f;
    private bool animationComplete = true;

    // components of parent text UI object - it's this behaviour's job to dynamically manipulate these.
    private Text textComponent;
    private RectTransform rectTransform;

    // Use this for initialization
    void Start() {
        // fetch sibling components
        this.textComponent = transform.parent.GetComponentInChildren<Text>();
        this.rectTransform = transform.parent.GetComponentInChildren<RectTransform>();

        // set values
        this.textComponent.text = this.TextToDisplay;
        this.rectTransform.anchoredPosition = this.animationStartingPoint;
    }
	
	public void UpdatePosition (float timeElapsed) {
        if (!this.animationComplete && this.fracDistanceCovered <= 1f)
        {
            this.animationSpeed += this.animationAcceleration * timeElapsed;
            this.fracDistanceCovered += this.animationSpeed * timeElapsed;
            this.transform.position = Vector3.Lerp(this.animationStartingPoint, this.animationTargetPoint, fracDistanceCovered);
            Debug.LogFormat("MovingText position: {0}.", this.transform.position.ToString());
        }
        else if (!this.animationComplete && this.fracDistanceCovered > 1f)
        {
            OnTextArrive();
            this.animationComplete = true;
        }
	}

    public void Restart()
    {
        //this.transform.position = this.animationStartingPoint;
        this.animationComplete = false;
        this.fracDistanceCovered = 0f;
        this.animationSpeed = startingAnimationSpeed;
    }

    public void SetStartAndEndPoints(Vector3 animationStartingPoint, Vector3 animationTargetPoint)
    // Set new start and end point and refresh any variables required to start a new animation
    {
        this.animationStartingPoint = animationStartingPoint;
        this.animationTargetPoint = animationTargetPoint;
        this.Restart();
    }

    public void SetTextColor(Color colour)
    {
        this.textComponent.color = colour;
    }

    public void SetText(string text)
    {
        this.TextToDisplay = text;
    }
}

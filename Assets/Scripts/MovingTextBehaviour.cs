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

    // tracking startup time
    private float warmUpTime = 0f;  // Time before starting first animation
    private float sitStillTime = 0f; // Time after arriving at target and before exiting
    private float timeElapsed = 0f;

    // components of parent text UI object - it's this behaviour's job to dynamically manipulate these.
    private Text textComponent;
    private RectTransform rectTransform;

    // Use this for initialization
    void Awake() {
        // fetch sibling components
        this.textComponent = GetComponent<Text>();
        this.rectTransform = GetComponent<RectTransform>();

        // set values
        this.textComponent.text = this.TextToDisplay;
        this.rectTransform.anchoredPosition = this.animationStartingPoint;
    }

    public void SetWarmUpTime(float warmUpTime)
    {
        this.warmUpTime = warmUpTime;
    }

    public void SetSitStillTime(float sitStillTime)
    {
        this.sitStillTime = sitStillTime;
    }
	
	public void UpdatePosition (float timeDelta) {
        this.timeElapsed += timeDelta;
        if (this.timeElapsed < this.warmUpTime)
        {
            // don't start any animations yet, the moving text is in its warmup period
            return;
        }
        if (!this.animationComplete && this.fracDistanceCovered <= 1f)
        {
            this.animationSpeed += this.animationAcceleration * timeDelta;
            this.fracDistanceCovered += this.animationSpeed * timeDelta;
            this.rectTransform.anchoredPosition = Vector3.Lerp(this.animationStartingPoint, this.animationTargetPoint, fracDistanceCovered);
        }
        else if (!this.animationComplete && this.fracDistanceCovered > 1f)
        {
            OnTextArrive();
            this.animationComplete = true;
        }
        else if (this.timeElapsed > this.sitStillTime)
        {
            this.fracDistanceCovered += this.animationSpeed * timeDelta;
            this.rectTransform.anchoredPosition = Vector3.Lerp(this.animationTargetPoint, this.animationStartingPoint, fracDistanceCovered - 1f);
        }
	}

    public void Restart()
    {
        //this.transform.position = this.animationStartingPoint;
        this.animationComplete = false;
        this.fracDistanceCovered = 0f;
        this.animationSpeed = startingAnimationSpeed;
        this.timeElapsed = 0f;
        this.rectTransform.anchoredPosition = this.animationStartingPoint;
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
        this.textComponent.text = text;
    }
}

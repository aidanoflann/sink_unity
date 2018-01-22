using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingTextBehaviour : MonoBehaviour {

    public delegate void TextArrivedAction();
    public static event TextArrivedAction OnTextArrive;

    public string TextToDisplay;

    // Animation
    private Vector3 animationStartingPoint;
    private Vector3 animationTargetPoint;
    private static float startingAnimationSpeed = 1.0f;
    private float animationSpeed = startingAnimationSpeed;
    private float animationAcceleration = 1f;
    private float fracDistanceCovered = 0f;
    private bool animationComplete = false;

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
        this.transform.position = this.animationStartingPoint;
	}
	
	// Update is called once per frame
	void Update () {
        if (!this.animationComplete && this.fracDistanceCovered <= 1f)
        {
            this.animationSpeed += this.animationAcceleration * Time.deltaTime;
            this.fracDistanceCovered += this.animationSpeed * Time.deltaTime;
            this.transform.position = Vector3.Lerp(this.animationStartingPoint, this.animationTargetPoint, fracDistanceCovered);
            Debug.LogFormat("Setting movingText pivot to : {0}", this.transform.position.ToString());
        }
        else if (!this.animationComplete && this.fracDistanceCovered > 1f)
        {
            OnTextArrive();
            this.animationComplete = true;
        }
	}

    public void Restart()
    {
        this.rectTransform.pivot = this.animationStartingPoint;
        this.animationComplete = false;
        this.fracDistanceCovered = 0f;
        this.animationSpeed = startingAnimationSpeed;
    }

    public void SetStartAndEndPoints(Vector3 animationStartingPoint, Vector3 animationTargetPoint)
    {
        this.animationStartingPoint = animationStartingPoint;
        this.animationTargetPoint = animationTargetPoint;
    }
}

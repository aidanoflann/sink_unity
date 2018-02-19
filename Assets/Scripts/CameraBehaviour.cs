using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

	public Camera cameraObject;
	private Player player;
    private float panSpeed = 5f;
    private float shakeSpeed = 3f;
    private float? shakeStartTime = null; // null means not shaking
    private float shakeDuration = 0.2f;

    private Vector3 currentShakeTarget = new Vector3();
    private float shakeStepDuration = 0.005f;  // currently unused

    private void OnEnable()
    {
        MovingTextBehaviour.OnTextArrive += StartShake;
    }

    private void OnDisable()
    {
        MovingTextBehaviour.OnTextArrive -= StartShake;
    }

    // Use this for initialization
    void Awake () {
		cameraObject = Camera.main;
	}

    void Start()
    {
        this.FindPlayer();
    }

    private void Update()
    {
        // shake screen if needed
        this.Shake();
    }

    public void FollowPlayer (bool dynamicZoom = true, float? panSpeedOverride = null)
    // Sets the camera's current point to the player (offset to show level, etc.)
    {
        Vector3 newPosition = cameraObject.transform.position;

        // zoom to ensure the level and player are both visible
        if (dynamicZoom)
        {

            newPosition.x = player.transform.position.x * 0.67f;
            newPosition.y = player.transform.position.y * 0.67f - 5f;
            // TODO: also approach orthographicSize
            cameraObject.orthographicSize = 7.5f * Mathf.Max(0.001f, Mathf.Sqrt(Mathf.Abs(player.RPos)));
        }
        else
        {
            newPosition.x = player.transform.position.x;
            newPosition.y = player.transform.position.y;
            cameraObject.orthographicSize = 10f;
        }
        float panSpeedToUse = panSpeedOverride.GetValueOrDefault(this.panSpeed);
        this.ApproachPoint(newPosition, panSpeedToUse);
    }

    public void SnapToPlayer()
    // Snap to the player's current position and set an appropriate zoom level
    {
        Vector3 newPosition = cameraObject.transform.position;

        newPosition.x = player.transform.position.x;
        newPosition.y = player.transform.position.y;

        cameraObject.orthographicSize = 20f;
        cameraObject.transform.position = newPosition;
    }

    private void ApproachPoint(Vector3 pointToApproach, float speed)
    {
        cameraObject.transform.position += (pointToApproach - cameraObject.transform.position) * Time.deltaTime * speed;
        //cameraObject.transform.position = pointToApproach;
    }

    private void Shake()
    // if currently shaking, apply shake effect
    {
        if (this.shakeStartTime != null)
        {
            float? timeSinceShakeStart = Time.time - this.shakeStartTime;
            if (timeSinceShakeStart > this.shakeDuration)
            {
                this.StopShake();
            }
            else
            {
                float targetDistance = 1 * this.cameraObject.orthographicSize;

                this.currentShakeTarget.x = this.cameraObject.transform.position.x + Random.Range(- targetDistance, targetDistance);
                this.currentShakeTarget.y = this.cameraObject.transform.position.y + Random.Range(- targetDistance, targetDistance);
                this.currentShakeTarget.z = this.cameraObject.transform.position.z;

                this.ApproachPoint(this.currentShakeTarget, this.shakeSpeed);
            }
        }
    }

    public void StartShake()
    {
        this.shakeStartTime = Time.time;
    }

    private void StopShake()
    {
        this.shakeStartTime = null;
    }


    public void EndGame()
    {
        cameraObject.orthographicSize = 7f;
    }

    public void FindPlayer()
    {
        this.player = (Player)GameObject.FindObjectOfType(typeof(Player));
    }

    public void SetColour(Color backgroundColour)
    {
        this.cameraObject.backgroundColor = backgroundColour;
    }
}

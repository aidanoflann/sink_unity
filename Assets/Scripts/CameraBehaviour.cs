using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

	private Camera cameraObject;
	private Player player;
    private float panSpeed = 5f;
    private float shakeSpeed = 15f;
    private float? shakeStartTime = null; // null means not shaking
    private float shakeDuration = 0.2f;

    private Vector3 currentShakeTarget = new Vector3();
    private float shakeStepDuration = 0.005f;


	// Use this for initialization
	void Awake () {
		cameraObject = Camera.main;
	}

    void Start()
    {
        this.FindPlayer();
    }
	
	public void FollowPlayer ()
    // Sets the camera's current point to the player (offset to show level, etc.)
    {
        Vector3 newPosition = cameraObject.transform.position;

		newPosition.x = player.transform.position.x * 0.67f;
		newPosition.y = player.transform.position.y * 0.67f - 5f;

        // zoom to ensure the level and player are both visible
        cameraObject.orthographicSize = 7.5f * Mathf.Max(0.001f, Mathf.Sqrt(Mathf.Abs(player.RPos)));
        
        this.ApproachPoint(newPosition, this.panSpeed);
        this.Shake();
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
                //if (timeSinceShakeStart % this.shakeDuration < Time.deltaTime)
                //{
                    this.currentShakeTarget.x = this.cameraObject.transform.position.x + Random.Range(-20f, 20f);
                    this.currentShakeTarget.y = this.cameraObject.transform.position.y + Random.Range(-20f, 20f);
                    this.currentShakeTarget.z = this.cameraObject.transform.position.z;
                //}
                this.ApproachPoint(this.currentShakeTarget, 3f);
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

using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

	private Camera cameraObject;
	private Player player;
    private float panSpeed = 5f;

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
        
        this.ApproachPoint(newPosition);
    }

    private void ApproachPoint(Vector3 pointToApproach)
    {
        cameraObject.transform.position += (pointToApproach - cameraObject.transform.position) * Time.deltaTime * this.panSpeed;
        //cameraObject.transform.position = pointToApproach;
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

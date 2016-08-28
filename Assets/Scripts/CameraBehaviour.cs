using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

	private Camera cameraObject;
	private Player player;

	// Use this for initialization
	void Start () {
		cameraObject = Camera.main;
        FindPlayer();
	}
	
	// Update is called once per frame
	public void FollowPlayer () {
        Vector3 newPosition = cameraObject.transform.position;

		newPosition.x = player.transform.position.x * 0.67f;
		newPosition.y = player.transform.position.y * 0.67f;
        cameraObject.transform.position = newPosition;

        cameraObject.orthographicSize = 5f * Mathf.Max(0.001f, Mathf.Sqrt(Mathf.Abs(player.RPos)));
    }

    public void EndGame()
    {
        cameraObject.orthographicSize = 7f;
    }

    public void FindPlayer()
    {
        this.player = (Player)GameObject.FindObjectOfType(typeof(Player));
    }
}

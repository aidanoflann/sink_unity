using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	private Camera camera;
	private Player player;

	// Use this for initialization
	void Start () {
		camera = Camera.main;
        FindPlayer();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPosition = camera.transform.position;

		newPosition.x = player.transform.position.x * 0.67f;
		newPosition.y = player.transform.position.y * 0.67f;
        camera.transform.position = newPosition;

        camera.orthographicSize = 5f * Mathf.Max(0.001f, Mathf.Sqrt(Mathf.Abs(player.r_pos)));
    }

    public void FindPlayer()
    {
        this.player = (Player)GameObject.FindObjectOfType(typeof(Player));
    }
}

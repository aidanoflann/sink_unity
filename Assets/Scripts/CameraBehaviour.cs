using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	private Camera camera;
	private Player player;

	// Use this for initialization
	void Start () {
		camera = Camera.main;
		player = (Player)GameObject.FindObjectOfType (typeof(Player));
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPosition = camera.transform.position;

		camera.orthographicSize += 0.0f;
		newPosition.x = player.transform.position.x * 0.67f;
		newPosition.y = player.transform.position.y * 0.67f;

		camera.transform.position = newPosition;
	}
}

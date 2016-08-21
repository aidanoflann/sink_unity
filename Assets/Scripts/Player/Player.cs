using UnityEngine;
using System.Collections;

public class Player : DynamicObject {

	private float size;
	public float r_pos;
	private float r_vel;
	private float w_pos;
	private float w_vel;
	private bool[] abovePlatform;
	private int platformIndex;
	private enum state
	{
		midair = 0,
		landed = 1
	}
	private state currentState;

    private Platform platform;

    public Player() {
		//static attributes
		size = 0.5f;

		//dynamic attributes
		r_pos = 10;
		r_vel = 0f;
		w_pos = 90f;
		w_vel = 0f;

		currentState = state.midair;
	}

	void Start() {
		//corner points
		Vector2[] points = new Vector2[4];
		points [0].x = size * 0.5f;
		points [0].y = size * 0.5f;

		points [1].x = size * 0.5f;
		points [1].y = -size * 0.5f;

		points [2].x = -size * 0.5f;
		points [2].y = -size * 0.5f;

		points [3].x = -size * 0.5f;
		points [3].y = size * 0.5f;

		createMesh (points, Resources.Load<Material>("PlayerMaterial"));

	}

	void Update() {
		// update position in polar coordinates
		float deltaTime = Time.deltaTime;

        // collisions
        if (currentState == state.midair)
        {
            platformIndex = CheckPlatformCollisions();
            applyCollisions(platformIndex);
        }
        else
        {
            // check that platform hasn't despawned
            if (platform == null)
            {
                currentState = state.midair;
            }
        }

        // states
        if (currentState == state.midair) {
			r_vel -= Globals.gravity * deltaTime;
			r_pos += r_vel * deltaTime;
		} else {
			r_pos = platform.r_pos + size * 0.51f + platform.r_size * 0.5f;
		}

		// inputs
		if (Input.GetKeyDown ("space")) {
			jump ();
		}
		w_pos = (w_pos + w_vel * deltaTime) % 360f;

		updateTransform (r_pos, w_pos);
	}

	private int CheckPlatformCollisions()
	{
		// index of platform the player is colliding with
		int collisionIndex = -1;

		// generate array of bools to compare with last tick.
		Platform[] platforms = (Platform[])GameObject.FindObjectsOfType(typeof(Platform));
		bool[] abovePlatformNew = new bool[platforms.Length];

		// iterate through platforms, update array and check if entry has changed
		for (int platformIndex = 0; platformIndex < platforms.Length; platformIndex++) {
			if (abovePlatform == null || abovePlatform [platformIndex]) {
				//check for colliding from above
				abovePlatformNew [platformIndex] = Mathf.Abs (r_pos - size * 0.5f) >= (platforms [platformIndex].r_pos + platforms[platformIndex].r_size * 0.5f);
			} else {
				//check for colliding from below
				abovePlatformNew [platformIndex] = Mathf.Abs (r_pos + size * 0.5f) >= (platforms [platformIndex].r_pos - platforms[platformIndex].r_size * 0.5f);
			}
			if (abovePlatform != null && abovePlatformNew [platformIndex] != abovePlatform [platformIndex]) {
				// check if outside of the angular range of the platform
				if (Mathf.Abs(Mathf.Abs ((w_pos - platforms [platformIndex].w_pos) + 180) % 360 - 180) < platforms [platformIndex].w_size * 0.5) {
					collisionIndex = platformIndex;
					//set the abovePlatform value back to its original
					abovePlatformNew [platformIndex] = abovePlatform [platformIndex];
				}
			}
		}

		// only overwrite the array if there wasn't a collision
		abovePlatform = abovePlatformNew;
		return collisionIndex;
	}

	private void applyCollisions (int collisionIndex)
	{
		if (collisionIndex != -1) {
			platform = (Platform)GameObject.FindObjectsOfType (typeof(Platform)) [collisionIndex];

            r_vel = 0;
			//check if collision was from above or below
			if (abovePlatform [collisionIndex]) {
				w_vel = platform.w_vel;
				r_pos = platform.r_pos + (platform.r_size * 0.5f) + (size * 0.501f);
				currentState = state.landed;
			} else {
				r_pos = platform.r_pos - (platform.r_size * 0.5f) - (size * 0.501f);
			}

		}
	}

	private void jump() {
		currentState = state.midair;
		r_vel = 14;
		w_vel = 0;
	}
}

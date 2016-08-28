﻿using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Player : DynamicObject {

	private float size;
	private float r_pos;
	private float r_vel;
	private float w_pos;
	private float w_vel;
	private List<bool> abovePlatform;
	private int platformIndex;
	private enum state
	{
		midair = 0,
		landed = 1
	}
	private state currentState;

    private Platform platform;

    void Awake() {
		//static attributes
		size = 0.5f;

        //dynamic attributes
        float[] wPosRange = Enumerable.Range(0, 359).Select(i => (float)i).ToArray();
        w_pos = wPosRange[Random.Range(0, wPosRange.Length - 1)];

        r_pos = 100;
		r_vel = 0f;
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

	public void UpdatePosition(bool needsToJump, List<Platform> platforms, float jumpSpeedModifier = 1f) {
		// update position in polar coordinates
		float deltaTime = Time.deltaTime;

        // collisions
        if (currentState == state.midair)
        {
            platformIndex = CheckPlatformCollisions(platforms);
            applyCollisions(platformIndex, platforms);
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
		if (needsToJump) {
			Jump (jumpSpeedModifier);
		}
		w_pos = (w_pos + w_vel * deltaTime) % 360f;

		updateTransform (r_pos, w_pos);
	}

    public void SetWPos(float wPos)
    {
        this.w_pos = wPos;
    }

	private int CheckPlatformCollisions(List<Platform> platforms)
	{
		// index of platform the player is colliding with
		int collisionIndex = -1;

		// generate array of bools to compare with last tick.
		List<bool> abovePlatformNew = new List<bool>();

        // trim abovePlatform to the same length as platforms (i.e. one has despawned)
        if (abovePlatform != null && abovePlatform.Count != platforms.Count)
        {
            abovePlatform.RemoveAt(0);
        }

		// iterate through platforms, update array and check if entry has changed
		for (int platformIndex = 0; platformIndex < platforms.Count; platformIndex++) {
			if (abovePlatform == null || abovePlatform [platformIndex]) {
				//check for colliding from above
				abovePlatformNew.Add(Mathf.Abs (r_pos - size * 0.5f) >= (platforms [platformIndex].r_pos + platforms[platformIndex].r_size * 0.5f));
			} else {
				//check for colliding from below
				abovePlatformNew.Add(Mathf.Abs (r_pos + size * 0.5f) >= (platforms [platformIndex].r_pos - platforms[platformIndex].r_size * 0.5f));
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

	private void applyCollisions (int collisionIndex, List<Platform> platforms)
	{
		if (collisionIndex != -1) {
			platform = platforms[collisionIndex];

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

    private void Jump( float jumpSpeedModifier )
    {
        if (currentState != state.midair)
        {
            currentState = state.midair;
            r_vel = 14 * jumpSpeedModifier;
            w_vel = 0;
        }
    }

    public bool IsLanded
    {
        get
        {
            return currentState == state.landed;
        }
    }
    
    public float WPos
    {
        get
        {
            return w_pos;
        }
    }

    public float RPos
    {
        get
        {
            return r_pos;
        }
    }

    public bool IsOnTopPlatform
    {
        get
        {
            if (currentState == state.landed)
            {
                if (abovePlatform.All(c => c == true))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

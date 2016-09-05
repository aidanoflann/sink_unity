using UnityEngine;
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

    private enum jumpState
    {
        holding = 0,
        letGo = 1,
    }
    private jumpState currentJumpState;

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

	public void UpdatePosition(bool needsToJump, bool needsToDeJump, List<Platform> platforms, float jumpSpeedModifier = 1f) {
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
            if (this.platform == null)
            {
                currentState = state.midair;
            }
        }

        // states
        if (currentState == state.midair) {
			r_vel -= Globals.gravity * deltaTime;
			r_pos += r_vel * deltaTime;
		} else {
			r_pos = this.platform.r_pos + size * 0.51f + this.platform.r_size * 0.5f;
		}

		// inputs
		if (needsToJump) {
			Jump (jumpSpeedModifier);
		}
        if (needsToDeJump)
        {
            DeJump();
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
				if (Mathf.Abs(Mathf.Abs ((this.w_pos - platforms [platformIndex].w_pos) + 180) % 360 - 180) < platforms [platformIndex].w_size * 0.5) {
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
			//check if collision was from above or below
			if (abovePlatform [collisionIndex])
            {
                this.r_vel = 0;
                this.platform = platforms[collisionIndex];
                this.platform.SetColour(this.meshRenderer.material.GetColor("_Color"));
                w_vel = this.platform.w_vel;
				r_pos = this.platform.r_pos + (this.platform.r_size * 0.5f) + (size * 0.5f);
				currentState = state.landed;
			} else {
                Platform unattachedPlatform = platforms[collisionIndex];
                // bounce back from the platform, adding its momentum if moving down
                if (unattachedPlatform.r_vel < 0)
                {
                    this.r_vel = (-unattachedPlatform.r_vel - this.r_vel);
                }
                else
                {
                    this.r_vel = -this.r_vel;
                }
                
                r_pos = unattachedPlatform.r_pos - (unattachedPlatform.r_size * 0.5f) - (size * 0.5f);
			}

		}
	}

    private void Jump( float jumpSpeedModifier )
    {
        if (currentState != state.midair)
        {
            this.currentState = state.midair;
            this.currentJumpState = jumpState.holding;
            this.r_pos += 0.1f;
            this.r_vel = 17 * jumpSpeedModifier;
            this.w_vel = 0;
            this.platform.ResetColour();
        }
    }

    private void DeJump()
    {
        if (currentState == state.midair && currentJumpState == jumpState.holding )
        {
            currentJumpState = jumpState.letGo;
            if (this.r_vel > 0)
            {
                this.r_vel /= 5f;
            }
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

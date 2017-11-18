using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Assets.Utils;

public class Player : DynamicObject {

    public Tail tail;

	private float size;
	private float r_pos;
	private float r_vel;
	private Angle w_pos;
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
    private float platformPosition;
    private float startingRPos;

    void Awake() {
		//static attributes
		this.size = 1f;

        //dynamic attributes
        float[] wPosRange = Enumerable.Range(0, 359).Select(i => (float)i).ToArray();
        this.w_pos = new Angle(wPosRange[Random.Range(0, wPosRange.Length - 1)]);

        this.startingRPos = 100f;
        this.r_pos = this.startingRPos;
        this.r_vel = 0f;

        this.currentState = state.midair;
        this.currentColour = new Color(0.1f, 0.2f, 0.9f);
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
			this.r_vel -= Globals.gravity * deltaTime;
            this.r_pos += r_vel * deltaTime;
		} else {
			this.r_pos = this.platform.r_pos + this.size * 0.5f + this.platform.r_size * 0.5f;
            this.w_pos = this.platform.w_pos + this.platformPosition * this.platform.w_size;
        }

		// inputs
		if (needsToJump) {
			Jump (jumpSpeedModifier);
		}
        if (needsToDeJump)
        {
            DeJump();
        }

		updateTransform (r_pos, w_pos);
        this.tail.UpdateTail();
    }

    public void SetWPos(Angle wPos)
    {
        this.w_pos = wPos;
    }

    public void Reset()
    {
        this.r_pos = startingRPos;
        this.r_vel = 0;
        this.abovePlatform = null;
        if (this.meshRenderer != null)
        {
            this.meshRenderer.enabled = true;
        }
        this.currentState = state.midair;
    }

    public void Hide()
    {
        this.meshRenderer.enabled = false;
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
				if (this.w_pos - platforms [platformIndex].w_pos < platforms [platformIndex].w_size * 0.5f) {
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
                // TODO: What if w_pos is 350 and this.platform.w_pos is 5?
                this.platformPosition = (this.w_pos - this.platform.w_pos).GetValue()/this.platform.w_size.GetValue();
                this.platform.CatchPlayer(this);
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
            this.r_pos += 0.5f;
            this.r_vel = 17 * jumpSpeedModifier + this.platform.r_vel;
            this.platform.ReleasePlayer();
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
    
    public Angle WPos
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

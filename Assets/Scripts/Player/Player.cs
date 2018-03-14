using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Assets.Utils;


public class PlayerUpdate
{
    public bool jumped;
    public bool hitPlatform;
}

public class Player : DynamicObject {

    public Tail tail;

	private float size;
	private float r_pos;
	private float r_vel;
	private Angle w_pos;
	private List<bool> abovePlatform;
	private int platformIndex;
    private bool needsToJump;
    private bool needsToDeJump;
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
    private TrailRenderer trailRenderer;

    void Awake() {
		//static attributes
		this.size = 1f;

        //dynamic attributes
        float[] wPosRange = Enumerable.Range(0, 359).Select(i => (float)i).ToArray();
        this.w_pos = new Angle(wPosRange[Random.Range(0, wPosRange.Length - 1)]);

        this.startingRPos = 400f;
        this.r_pos = this.startingRPos;
        this.r_vel = 0f;

        this.currentState = state.midair;
        this.currentColour = new Color(0.1f, 0.2f, 0.9f);

        this.trailRenderer = GetComponent<TrailRenderer>();
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

	public PlayerUpdate UpdatePosition(List<Platform> platforms, float jumpSpeedModifier = 1f) {
        // create an update "report" so the level manager can react to events
        PlayerUpdate playerUpdate = new PlayerUpdate();

        // update position in polar coordinates - return True if a collision occurred
        bool collisionOccured = false;
		float deltaTime = Time.deltaTime;

        // inputs
        if (this.needsToJump)
        {
            Jump(jumpSpeedModifier);
            playerUpdate.jumped = true;
        }
        if (this.needsToDeJump)
        {
            DeJump();
        }

        // collisions
        if (currentState == state.midair)
        {
            platformIndex = CheckPlatformCollisions(platforms);
            playerUpdate.hitPlatform = (platformIndex != -1);
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
        return playerUpdate;
    }

    public void UpdateVisuals()
    {
        this.UpdateTransform(this.r_pos, this.w_pos);
        //this.tail.UpdateTail();
    }

    public void SetWPos(float wPos)
    {
        this.w_pos.SetValue(wPos);
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
        this.trailRenderer.Clear();
    }

    public void ResetTail()
    {
        this.trailRenderer.Clear();
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
                Platform platform = platforms[platformIndex];
				if (this.w_pos.IsWithin(platform.w_pos, platform.w_size)) {
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

    public void Jump( float jumpSpeedModifier )
    {
        if (IsLanded)
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

    public void AddRPos(float amountToAdd)
    {
        this.r_pos += amountToAdd;
    }

    public Angle GetPlatformWSize()
    {
        if (this.platform == null)
        {
            return null;
        }
        return this.platform.w_size;
    }

    public int PlatformIndex
    {
        get
        {
            return this.platformIndex;
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

    public void HandleInputs(bool inCompletingState)
    {
        this.needsToJump = Input.GetKeyDown("space") || Input.GetMouseButtonDown(0);
        if (!inCompletingState)
        {
            needsToDeJump = Input.GetKeyUp("space") || Input.GetMouseButtonUp(0);
        }
    }

    public bool IsReadyToEndGame
    {
        get
        {
            return this.IsOnTopPlatform && this.needsToJump;
        }
    }

    public void Log()
    {
        Debug.LogFormat("size: {0}, r_pos: {1}, r_vel: {2}, w_pos: {3}, platformIndex: {4}, currentState: {5}",
            this.size, this.r_pos, this.r_vel, this.w_pos.GetValue(), this.platformIndex, this.currentState);
        Debug.Log("-abovePlatform");
        for (int i = 0; i < this.abovePlatform.Count; i++)
        {
            Debug.LogFormat("abovePlatform index: {0}, abovePlatform value: {1}", i, this.abovePlatform[i]);
        }
    }
}

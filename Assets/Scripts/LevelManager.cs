using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LevelManager : MonoBehaviour {

    #region [Public fields]

    // prepare the player and platform prefabs
    public GameObject playerPrefab;
	public GameObject platformPrefab;

    //level status
    public enum state
    {
        needsRestart = 0,
        starting = 1,
        main = 2,
        paused = 3
    }
    public state currentState;

    #endregion

    #region [Private fields]

    // cached GameObjects
    private List<GameObject> platformList;
    private List<Platform> platforms;
    private GameObject playerObject;
    private Player player;

	// this is used to child all of the gameObjects for better control/organisation in the inspector.
	private Transform levelHolder;

    // platform parameter ranges
    private float[] wPosRange;
    private float[] wVelRange;
    private float[] wSizeRange;


    private state prepausedState;
    private int numPlatforms;
    private CameraBehaviour cameraBehaviour;
    #endregion

    #region [Private Methods]
    private void GeneratePlatformRanges()
    {
        wVelRange = Enumerable.Range(6, 12).Select(i => (float)i * 10f).ToArray();
        wSizeRange = Enumerable.Range(3, 30).Select(i => (float)i*10f).ToArray();
    }

	private void SpawnPlatforms(int numPlatforms, float playerPosition)
	{
		bool clockwise = true;
		for (int p=0; p < numPlatforms; p++) {
			
			GameObject toInstantiate = platformPrefab;
			GameObject instance = Instantiate (toInstantiate) as GameObject;

			// grab the script
			Platform platform = instance.GetComponent<Platform> ();

            // TODO: Find a way to get these into an init function - doing so as normal changes the values of the prefab, not the instance
            // TODO: Seems to be that public attributes are assumed to be accessible in-editor only - use properties where required.
            platform.w_vel = wVelRange[Random.Range(0, wVelRange.Length - 1)];
            if (clockwise)
				platform.w_vel *= 1f;
			else
				platform.w_vel *= -1f;
            if (p == 0)
            {
                platform.w_pos = playerPosition;
            }
            else
            {
                platform.w_pos = playerPosition + 180f;
            }

            platform.w_size = wSizeRange[Random.Range(0, wSizeRange.Length - 1)];
            platform.r_pos = 2f * (float)(p + 1);
            platform.r_vel = -0.5f;
            clockwise = !clockwise;

			instance.transform.SetParent (levelHolder);
            platformList.Add(instance);
            platforms.Add(platform);
        }
	}

	private void SpawnPlayer()
	{
		GameObject toInstantiate = playerPrefab;
		playerObject = Instantiate (toInstantiate);
        playerObject.transform.SetParent (levelHolder);
        player = playerObject.GetComponent<Player>();
    }

    private void Clear()
    {
        // clear out all platforms & player
        for (int i = 0; i < platformList.Count; i++)
        {
            GameObject platformObject = platformList[i];
            Destroy(platformObject);
        }
        platformList.Clear();
        platforms.Clear();
        Destroy(playerObject);
    }
    #endregion

    #region [Public methods]
    
    public void SetupScene(int numPlatforms = -1, CameraBehaviour cameraBehaviour = null)
	{
        if (numPlatforms != -1)
        {
            this.numPlatforms = numPlatforms;
        }

        if (cameraBehaviour != null)
        {
            this.cameraBehaviour = cameraBehaviour;
        }

        currentState = state.starting;
        SpawnPlayer();
        SpawnPlatforms (this.numPlatforms, player.WPos);
        
        // reassign the camera's player
        this.cameraBehaviour.FindPlayer();
    }

    public void Restart()
    {
        this.Clear();
        this.SetupScene();
    }

    public void Pause()
    {
        if (currentState != state.paused)
        {
            prepausedState = currentState;
            currentState = state.paused;
        }
        else
        {
            currentState = prepausedState;
        }
    }
    #endregion

    #region [Unity methods]

    public void Awake()
    {
        levelHolder = new GameObject("Level").transform;
        platformList = new List<GameObject>();
        platforms = new List<Platform>();
        GeneratePlatformRanges();
    }

    public void Update()
    {
        if (currentState != state.paused)
        {
            // restart game if player has died
            player.UpdatePosition(Input.GetKeyDown("space"), platforms);
            if (player.RPos <= 0)
            {
                currentState = state.needsRestart;
            }

            // check if player has landed for the first time
            if (currentState == state.starting && player.IsLanded)
            {
                currentState = state.main;
            }
            else if (currentState == state.main)
            {
                for (int i = 0; i < platformList.Count; i++)
                {
                    GameObject platformObject = platformList[i];
                    Platform platform = platformObject.GetComponent<Platform>();
                    platform.UpdatePosition();
                    if (platform.r_pos <= 0)
                    {
                        Destroy(platformObject);
                        Destroy(platform);
                        platformList.Remove(platformObject);
                        platforms.Remove(platform);
                        i -= 1;
                    }
                }
            }
        }
    }
    #endregion
}

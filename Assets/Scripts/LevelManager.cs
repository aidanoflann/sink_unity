using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Utils;

public class LevelManager : MonoBehaviour {

    #region [Public fields]

    // prepare the player and platform prefabs
    public GameObject playerPrefab;
	public GameObject platformPrefab;
    public BackgroundCircleFactory backgroundCircleFactory;
    public int numStackedTemplates;

    //level status
    public enum state
    {
        needsRestart = 0,
        starting = 1,
        main = 2,
        paused = 3,
        ending = 4,
        completing = 5,
        nextLevel = 6,
        showingLevelText = 7,
    }
    public state currentState;

    #endregion

    #region [Private fields]

    // cached GameObjects
    private List<GameObject> platformList;
    private List<Platform> platforms;
    public Player player;

	// this is used to child all of the gameObjects for better control/organisation in the inspector.
	private Transform levelHolder;

    // platform parameter ranges
    private float[] wPosRange;
    private float[] wVelRange;
    private float[] wSizeRange;

    private float platformRSpeedMultiplier;

    private state prepausedState;
    private int numPlatforms;
    private CameraBehaviour cameraBehaviour;
    private List<LevelTemplate> levelTemplates;
    private int numBaseTemplates;
    #endregion

    #region [Private Methods]
    private void GeneratePlatformRanges()
    {
        wVelRange = Enumerable.Range(6, 12).Select(i => (float)i * 10f).ToArray();
        wSizeRange = Enumerable.Range(3, 30).Select(i => (float)i*10f).ToArray();
    }

	private void SpawnPlatforms(int numPlatforms, Angle playerPosition)
	{
		bool clockwise = true;
		for (int p=0; p < numPlatforms; p++) {
			
			GameObject toInstantiate = platformPrefab;
			GameObject instance = Instantiate (toInstantiate) as GameObject;

			// grab the script
			Platform platform = instance.GetComponent<Platform> ();

            // TODO: Find a way to get these into an init function - doing so as normal changes the values of the prefab, not the instance
            // TODO: Seems to be that public attributes are assumed to be accessible in-editor only - use properties where required.
            platform.w_vel = new Angle(wVelRange[Random.Range(0, wVelRange.Length - 1)]);
            platform.w_size = new Angle(wSizeRange[Random.Range(0, wSizeRange.Length - 1)]);
            if (clockwise)
				platform.w_vel *= 1f;
			else
				platform.w_vel *= -1f;
            if (p == 0)
            {
                platform.w_pos = playerPosition;
                platform.w_size.SetValue(359.9999f);
            }
            else
            {
                platform.w_pos = new Angle(playerPosition.GetValue() + 180f);
            }

            platform.r_pos = 10f + 3f * (float)p;
            platform.r_vel = -0.7f;
            platform.SetColour(this.levelTemplates[this.levelTemplates.Count() - 1].PlatformColor);
            clockwise = !clockwise;

			instance.transform.SetParent (levelHolder);
            platformList.Add(instance);
            platforms.Add(platform);

            for (int j = 0; j < levelTemplates.Count; j++)
            {
                levelTemplates[j].SetPlatformParameters(platform, p, numPlatforms);
            }

            // update all original values
            platform.SetOriginalValues();
        }
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
        //Destroy(playerObject);
    }
    #endregion

    #region [Public methods]
    
    public void SetupScene(Angle playerWPos = null)
    {
        // TODO: Trigger text animation here
        // TODO: Needs to move texxt around on a canvas that moves with the camera
        platformRSpeedMultiplier = 1f;

        currentState = state.starting;

        player.Reset();
        if (playerWPos != null)
        {
            player.SetWPos(playerWPos.GetValue());
        }
        player.UpdateVisuals();
        player.ResetTail();

        SpawnPlatforms (this.numPlatforms, player.WPos);

        // generate the background
        this.backgroundCircleFactory.GenerateBackgroundCircles(this.levelTemplates[this.levelTemplates.Count - 1].CircleColor);

        // update camera
        this.cameraBehaviour.SetColour(this.levelTemplates[this.levelTemplates.Count - 1].BackgroundColor);
        this.cameraBehaviour.FindPlayer();
    }

    public void SetBaseTemplates(List<LevelTemplate> levelTemplates)
    {
        this.levelTemplates.Clear();
        this.levelTemplates.AddRange(levelTemplates);
        this.numBaseTemplates = this.levelTemplates.Count();
    }

    public void CycleTemplate(LevelTemplate levelTemplate)
    // remove the 3rd template and add the given one
    {
        if (this.levelTemplates.Count > this.numBaseTemplates + this.numStackedTemplates)
        {
            this.levelTemplates.Remove(this.levelTemplates[this.numBaseTemplates]);
        }
        this.levelTemplates.Add(levelTemplate);
    }

    public void AddTemplate(LevelTemplate levelTemplate)
    {
        this.levelTemplates.Add(levelTemplate);
    }

    public void RemoveTemplate(LevelTemplate levelTemplate)
    {
        this.levelTemplates.Remove(levelTemplate);
    }

    public void SetNumPlatforms(int numPlatforms)
    {
        this.numPlatforms = numPlatforms;
    }

    public void SetCameraBehaviour(CameraBehaviour cameraBehaviour)
    {
        this.cameraBehaviour = cameraBehaviour;
    }

    public void Restart(Angle newPlayerWpos = null)
    {
        if (newPlayerWpos == null)
        {
            newPlayerWpos = new Angle(player.WPos.GetValue() + 180f);
        }
        for (int i = 0; i < this.levelTemplates.Count; i++)
        {
            this.levelTemplates[i].Reload();
        }
        this.Clear();
        this.SetupScene(newPlayerWpos);
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
        this.levelHolder = new GameObject("Level").transform;
        this.platformList = new List<GameObject>();
        this.platforms = new List<Platform>();
        this.levelTemplates = new List<LevelTemplate>();
        GeneratePlatformRanges();
    }

    public void Update()
    {
        // if paused, don't do anything
        if (currentState == state.paused)
        {
            return;
        }

        // restart game if player has died
        if (player.RPos <= 0 && currentState != state.ending)
        {
            currentState = state.ending;
            platformRSpeedMultiplier = 24f;
            cameraBehaviour.EndGame();
            player.Hide();
        }

        // restart game if in completing state and player gets over a certain height
        if (currentState == state.completing && player.RPos > 400f)
        {
            this.currentState = state.nextLevel;
        }

        // check if player has landed for the first time
        if (currentState == state.starting && player.IsLanded)
        {
            currentState = state.main;
        }
        else if (currentState == state.main || currentState == state.ending || currentState == state.completing)
        {
            // update all platform positions based on level templates
            for (int i = 0; i < platformList.Count; i++)
            {
                GameObject platformObject = platformList[i];
                Platform platform = platformObject.GetComponent<Platform>();

                for( int j = 0; j < levelTemplates.Count; j++)
                {
                    levelTemplates[j].UpdatePlatformPosition(i, this.platforms, platformRSpeedMultiplier);
                }

                if (platform.r_pos <= 0)
                {
                    Destroy(platformObject);
                    Destroy(platform);
                    platformList.Remove(platformObject);
                    platforms.Remove(platform);
                    i -= 1;
                }
                else
                {
                    platform.RecalculateMesh();
                }
            }

            if (platformList.Count == 0 && currentState != state.completing)
            {
                this.currentState = state.needsRestart;
                return;
            }

        }
        if (currentState != state.ending)
        {
            player.HandleInputs(this.currentState == state.completing);

            if (player.IsReadyToEndGame)
            {
                currentState = state.completing;
                player.UpdatePosition(platforms, 20f);
            }

            // This needs to happen before the standard UpdatePosition
            for (int j = 0; j < levelTemplates.Count; j++)
            {
                levelTemplates[j].UpdateTemplate();
                levelTemplates[j].UpdatePlayer(player);
            }

            bool playerCollided = player.UpdatePosition(platforms);
            if (playerCollided)
            {
                this.ShakeCamera();
            }
            player.UpdateVisuals();
            cameraBehaviour.FollowPlayer();
        }
    }
    #endregion
    
    public void Log()
    {
        Debug.Log("--- LOGGING LEVEL MANAGER ---");
        Debug.Log("--Player--");
        this.player.Log();
        for (int i = 0; i < this.platforms.Count; i++)
        {
            Debug.LogFormat("--Plaftorm {0}--", i);
            platforms[i].Log();
        }
        Debug.Log("------");
    }

    public void ShakeCamera()
    {
        this.cameraBehaviour.StartShake();
    }
}

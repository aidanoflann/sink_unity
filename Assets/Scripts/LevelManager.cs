using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Utils;

public class LevelManager : MonoBehaviour {

    #region [Public fields]

    // prepare the player and platform prefabs
    public GameObject playerPrefab;
	public GameObject platformPrefab;
    public GameObject statManagerGameObject;
    public BackgroundCircleFactory backgroundCircleFactory;
    public int numStackedTemplates;

    //level status
    public enum state
    {
        needsRestart = 0,
        preStartAnimation = 1,
        starting = 2,
        main = 3,
        paused = 4,
        ending = 5,
        completing = 6,
        nextLevel = 7,
    }
    public state currentState;

    #endregion

    #region [Private fields]

    // cached GameObjects
    private List<GameObject> platformList;
    private List<Platform> platforms;
    private AnimationManager animationManager;
    private AudioManager audioManager;
    private StatManagerBehaviour statManagerBehaviour;
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

    private Camera _camera
    {
        get
        {
            return this.cameraBehaviour.cameraObject;
        }
    }

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
        platformRSpeedMultiplier = 1f;

        currentState = state.preStartAnimation;

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
        this.SetupAnimationManager();
    }

    private void SetupAnimationManager()
    {
        this.animationManager.Reset();
        string stringToSet = "SINK";
        int maxIndex = Mathf.Min(this.levelTemplates.Count, this.numStackedTemplates + this.numBaseTemplates);
        for (int i = this.numBaseTemplates; i < maxIndex; i++)
        {
            if (i != this.numStackedTemplates - 1)
            {
                stringToSet += " AND ";
            }
            stringToSet += (this.levelTemplates[i].Word);
        }
        this.animationManager.SetWords(stringToSet);
        this.animationManager.SetTextColour(this.levelTemplates[this.levelTemplates.Count - 1].PlatformColor);
    }

    public void SetBaseTemplates(List<LevelTemplate> levelTemplates)
    {
        this.levelTemplates.Clear();
        this.levelTemplates.AddRange(levelTemplates);
        this.numBaseTemplates = this.levelTemplates.Count();
        this.statManagerBehaviour.Reset();
    }

    public List<LevelTemplate> CycleTemplate(LevelTemplate levelTemplate)
    // remove the 3rd template and add the given one
    {
        List<LevelTemplate> templatesRemoved = new List<LevelTemplate>();
        this.levelTemplates.Add(levelTemplate);
        while (this.levelTemplates.Count > this.numBaseTemplates + this.numStackedTemplates)
        {
            // should only happen once, but keep removing leveltemplates from the back of the stacked dynamic templates (i.e. the oldest)
            LevelTemplate templateToRemove = this.levelTemplates[this.numBaseTemplates];
            this.levelTemplates.Remove(templateToRemove);
            templatesRemoved.Add(templateToRemove);
        }
        return templatesRemoved;
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
        this.animationManager.SetCameraBehaviour(cameraBehaviour);
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
        this.animationManager = new AnimationManager(FindObjectOfType<MovingTextCanvasBehaviour>());
        this.statManagerBehaviour = this.statManagerGameObject.GetComponent<StatManagerBehaviour>();

        GeneratePlatformRanges();
    }

    public void Start()
    {
        AudioManager[] allAudioManagers = FindObjectsOfType<AudioManager>();
        if (allAudioManagers.Length != 1)
        {
            Debug.LogErrorFormat("LevelManager found more than one AudioManager on startup. Attaching last one in array...");
        }
        this.audioManager = allAudioManagers[allAudioManagers.Length - 1];
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
            this.UpdateStats();
            platformRSpeedMultiplier = 24f;
            cameraBehaviour.EndGame();
            player.Hide();
        }

        // restart game if in completing state and player gets over a certain height
        if (currentState == state.completing && player.RPos > 400f)
        {
            this.UpdateStats();
            this.currentState = state.nextLevel;
        }

        if (currentState == state.preStartAnimation)
        {
            if (this.animationManager.HandleStartingAnimation())
            {
                this.currentState = state.starting;
            }
        }

        // check if player has landed for the first time
        if (currentState == state.starting && player.IsLanded)
        {
            this.statManagerBehaviour.SetStartTime();
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
                LevelUpdate levelUpdate = levelTemplates[j].UpdateTemplate();
                if (levelUpdate.triggerBeatSound)
                {
                    this.audioManager.Play("BeatSound");
                }
                levelTemplates[j].UpdatePlayer(player);
            }

            PlayerUpdate playerUpdate = player.UpdatePosition(platforms);
            if (playerUpdate.hitPlatform)
            {
                this.ShakeCamera();
                this.PlaySoundScaledToGameState(0.08f, "LandSound");
            }
            if (playerUpdate.jumped)
            {
                this.PlaySoundScaledToGameState(0.08f, "JumpSound");
            }
            player.UpdateVisuals();
            if (currentState != state.preStartAnimation)
            {
                cameraBehaviour.FollowPlayer();
            }
        }
    }

    private void UpdateStats()
    {
        this.statManagerBehaviour.UpdateStatsOnLevelEnd(this.numBaseTemplates, this.levelTemplates, this.numPlatforms);
    }

    private void PlaySoundScaledToGameState(float scaleFactor, string soundName)
    {
        // scale to player's RPos
        float pitchOverride = 1f + (1f/12f * this.player.PlatformIndex); // (scaleFactor * 30f) * this.player.PlatformIndex;
        this.audioManager.Play(soundName, pitchOverride);
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
        Debug.Log("--Stats--");
        this.statManagerBehaviour.Log();
        Debug.Log("------");
    }

    public void ShakeCamera()
    {
        this.cameraBehaviour.StartShake();
    }
}

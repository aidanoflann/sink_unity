using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using Assets.Utils;
using Assets.Scripts.Menus;

public class GameManager : MonoBehaviour {

    public Camera mainCamera;
    public Canvas pauseMenu;
    public PauseMenuBehaviour pauseMenuBehaviour;

    private LevelManager levelManager;
    private CameraBehaviour cameraBehaviour;
    private List<LevelTemplate> baseTemplates;
    private List<LevelTemplate> dynamicTemplates;
    private List<int> availableDynamicTemplateIndices;
    private RandomNumberManager randomNumberManager;

    public int numPlatforms = 5;
    public bool showFPS;

    void Awake ()
    {
        cameraBehaviour = mainCamera.GetComponent<CameraBehaviour>();
        levelManager = GetComponent<LevelManager> ();
        this.randomNumberManager = SingletonBehaviour.GetSingletonBehaviour<RandomNumberManager>();
        this.pauseMenu.enabled = false;

        // base templates, common to all levels
        this.baseTemplates = new List<LevelTemplate>();
        this.baseTemplates.Add(new RotateTemplate());
        this.baseTemplates.Add(new StickToPlatformTemplate());
        this.baseTemplates.Add(new FallTemplate());

        this.SetDynamicTemplates();

        this.levelManager.SetBaseTemplates(baseTemplates);
        this.levelManager.SetNumPlatforms(this.numPlatforms);
        this.levelManager.SetCameraBehaviour(cameraBehaviour);
        this.levelManager.SetupScene();
	}

    private void SetDynamicTemplates()
    {
        // Each dynamic template added will be used as a new level
        this.dynamicTemplates = new List<LevelTemplate>();
        this.dynamicTemplates.Add(new PulseTemplate()); // love it
        this.dynamicTemplates.Add(new StretchTemplate()); // like it
        this.dynamicTemplates.Add(new TickTemplate()); // like it but bit buggy
        this.dynamicTemplates.Add(new ReverseTemplate()); // love it
        this.dynamicTemplates.Add(new StutterTemplate()); // love it
        this.dynamicTemplates.Add(new SnapTemplate()); // love it
        this.dynamicTemplates.Add(new DropTemplate());
        this.dynamicTemplates.Add(new TwirlTemplate()); // fun but super hard
        this.dynamicTemplates.Add(new SlipTemplate());
        this.dynamicTemplates.Add(new VanishTemplate());
        this.dynamicTemplates.Add(new ConfuseTemplate()); // motion sickness?

        // Experimental templates (usually either low quality or very very difficult)
        //this.dynamicTemplates.Add(new BounceTemplate());
        //this.dynamicTemplates.Add(new WaveTemplate()); // ok
        //this.dynamicTemplates.Add(new SinkTemplate()); // hard to communicate, solution not that interesting
        //this.dynamicTemplates.Add(new PinchTemplate()); // no change in gameplay
        //this.dynamicTemplates.Add(new FattenTemplate()); // meh

        this.availableDynamicTemplateIndices = new List<int>();
        for (int i = 0; i < this.dynamicTemplates.Count; i++)
        {
            this.availableDynamicTemplateIndices.Add(i);
        }
    }

    float deltaTime = 0.0f;

    void Update()
    {
        if (levelManager.currentState == LevelManager.state.needsRestart)
        {
            SceneManager.LoadScene("EndGame");
        }
        else if (levelManager.currentState == LevelManager.state.nextLevel)
        {
            this.NextLevel();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //player-facing pause (bring up window, etc)
            this.PauseGame();
        }

#if UNITY_EDITOR
        // Level skip cheat
        if (Input.GetKeyDown("s"))
        {
            this.NextLevel();
        }

        if (Input.GetKeyDown("x"))
        {
            // debug pause
            levelManager.Pause();
        }

        if (Input.GetKeyDown("r"))
        {
            this.RestartGame();
        }

        if (Input.GetKeyDown("q"))
        {
            Debug.Log("SHAKETIME");
            levelManager.ShakeCamera();
        }

        if (Input.GetKeyDown("l"))
        {
            this.Log();
        }
#endif

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void NextLevel()
    {
        int indexToAdd = this.availableDynamicTemplateIndices[this.randomNumberManager.Range(0, this.availableDynamicTemplateIndices.Count)];
        this.availableDynamicTemplateIndices.Remove(indexToAdd);
        LevelTemplate templateToAdd = this.dynamicTemplates[indexToAdd];
        List<LevelTemplate> templatesRemoved = this.levelManager.CycleTemplate(templateToAdd);
        for(int i = 0; i < templatesRemoved.Count; i++)
        {
            this.availableDynamicTemplateIndices.Add(this.dynamicTemplates.IndexOf(templatesRemoved[i]));
        }
        this.levelManager.Restart();
    }

    private bool isPaused = false;
    void PauseGame()
    {
        if (this.isPaused)
        {
            this.levelManager.Pause();
            this.pauseMenu.enabled = false;
            this.isPaused = false;
        }
        else
        {
            this.levelManager.Pause();
            this.pauseMenuBehaviour.UpdateData();
            this.pauseMenu.enabled = true;
            this.isPaused = true;
        }
    }

    void RestartGame()
    {
        // must do this before restarting the level in order to reuse the same sequence of random commands.
        this.randomNumberManager.Reset();
        this.levelManager.SetBaseTemplates(this.baseTemplates);
        this.levelManager.Restart();
        this.SetDynamicTemplates();
    }

    // quick FPS script shamelessly copied from http://wiki.unity3d.com/index.php?title=FramesPerSecond
    void OnGUI()
    {
        if (this.showFPS)
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(rect, text, style);
        }
    }

#if UNITY_EDITOR
    private void Log()
    {
        Debug.LogError("=== BEGINNING LOG DUMP ===");
        this.levelManager.Log();
        Debug.LogError("=== ENDING LOG DUMP ===");
    }
#endif
}

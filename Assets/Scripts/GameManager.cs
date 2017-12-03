using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	private LevelManager levelManager;
    private CameraBehaviour cameraBehaviour;
    private Camera mainCamera;
    private List<LevelTemplate> baseTemplates;
    private List<LevelTemplate> dynamicTemplates;

    public int numPlatforms = 5;

    void Awake ()
    {
        mainCamera = GameObject.FindObjectOfType<Camera>();
        cameraBehaviour = mainCamera.GetComponent<CameraBehaviour>();
        levelManager = GetComponent<LevelManager> ();

        // base templates, common to all levels
        this.baseTemplates = new List<LevelTemplate>();
        this.baseTemplates.Add(new RotateTemplate());
        this.baseTemplates.Add(new FallTemplate());

        // Each dynamic template added will be used as a new level
        this.dynamicTemplates = new List<LevelTemplate>();
        this.dynamicTemplates.Add(new WaveTemplate());
        this.dynamicTemplates.Add(new PulseTemplate());
        this.dynamicTemplates.Add(new DilateTemplate());
        this.dynamicTemplates.Add(new TickTemplate());
        this.dynamicTemplates.Add(new TockTemplate());
        this.dynamicTemplates.Add(new FattenTemplate());
        this.dynamicTemplates.Add(new ReverseTemplate());
        this.dynamicTemplates.Add(new StutterTemplate());
        this.dynamicTemplates.Add(new SnapTemplate());
        this.dynamicTemplates.Add(new PinchTemplate());

        // Experimental templates (usually either low quality or very very difficult)
        //this.dynamicTemplates.Add(new BounceTemplate());
        //this.dynamicTemplates.Add(new TwirlTemplate());
        //this.dynamicTemplates.Add(new SinkTemplate());

        this.levelManager.SetBaseTemplates(baseTemplates);
        this.levelManager.SetNumPlatforms(this.numPlatforms);
        this.levelManager.SetCameraBehaviour(cameraBehaviour);
        this.levelManager.SetupScene();
	}

    float deltaTime = 0.0f;

    void Update()
    {
        if (Input.GetKeyDown("r") || levelManager.currentState == LevelManager.state.needsRestart ||
            ((Input.GetKeyDown("space") || Input.GetMouseButtonDown(0)) && levelManager.currentState == LevelManager.state.ending))
        {
            this.RestartGame();
        }
        else if (levelManager.currentState == LevelManager.state.nextLevel)
        {
            this.NextLevel();
        }

#if CHEATS_ENABLED
        // Level skip cheat
        if (Input.GetKeyDown("s"))
        {
            this.NextLevel();
        }

        if (Input.GetKeyDown("x"))
        {
            levelManager.Pause();
        }

#endif
#if EDITOR
        if (Input.GetKeyDown("l"))
        {
            this.Log();
        }
#endif

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    void NextLevel()
    {
        // TODO: randomly select template
        LevelTemplate templateToAdd = this.dynamicTemplates[Random.Range(0, this.dynamicTemplates.Count)];
        this.levelManager.CycleTemplate(templateToAdd);
        this.levelManager.Restart();
    }

    void RestartGame()
    {
        this.levelManager.SetBaseTemplates(this.baseTemplates);
        this.levelManager.Restart();
    }

    // quick FPS script shamelessly copied from http://wiki.unity3d.com/index.php?title=FramesPerSecond
    void OnGUI()
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

#if EDITOR
    private void Log()
    {
        Debug.LogError("=== BEGINNING LOG DUMP ===");
        this.levelManager.Log();
        Debug.LogError("=== ENDING LOG DUMP ===");
    }
#endif
}

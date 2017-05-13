using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	private LevelManager levelManager;
    private CameraBehaviour cameraBehaviour;
    private Camera mainCamera;
    private List<LevelTemplate> baseTemplates;
    private List<LevelTemplate> dynamicTemplates;

    void Awake ()
    {
        mainCamera = GameObject.FindObjectOfType<Camera>();
        cameraBehaviour = mainCamera.GetComponent<CameraBehaviour>();
        levelManager = GetComponent<LevelManager> ();

        this.baseTemplates = new List<LevelTemplate>();
        this.baseTemplates.Add(new RotateTemplate());
        this.baseTemplates.Add(new FallTemplate());

        this.dynamicTemplates = new List<LevelTemplate>();
        this.dynamicTemplates.Add(new PulseTemplate());
        this.dynamicTemplates.Add(new TickTemplate());
//        this.dynamicTemplates.Add(new SinkTemplate());

        this.levelManager.SetTemplates(baseTemplates);
        this.levelManager.SetNumPlatforms(5);
        this.levelManager.SetCameraBehaviour(cameraBehaviour);
        this.levelManager.SetupScene();
	}

    // quick FPS script shamelessly copied from http://wiki.unity3d.com/index.php?title=FramesPerSecond
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

        if (Input.GetKeyDown("x"))
        {
            levelManager.Pause();
        }

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
        this.levelManager.SetTemplates(this.baseTemplates);
        this.levelManager.Restart();
    }

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

}

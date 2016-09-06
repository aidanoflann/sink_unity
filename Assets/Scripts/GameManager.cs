using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	private LevelManager levelManager;
    private CameraBehaviour cameraBehaviour;
    private Camera mainCamera;
    private List<LevelTemplate> levelTemplates;

	void Awake ()
    {
        mainCamera = GameObject.FindObjectOfType<Camera>();
        cameraBehaviour = mainCamera.GetComponent<CameraBehaviour>();
        levelManager = GetComponent<LevelManager> ();

        levelTemplates = new List<LevelTemplate>();
        levelTemplates.Add(new RotateTemplate());
        levelTemplates.Add(new FallTemplate());
        levelTemplates.Add(new PulseTemplate());

        levelManager.SetTemplates(levelTemplates);
        levelManager.SetNumPlatforms(10);
        levelManager.SetCameraBehaviour(cameraBehaviour);
        levelManager.SetupScene();
	}

    // quick FPS script shamelessly copied from http://wiki.unity3d.com/index.php?title=FramesPerSecond
    float deltaTime = 0.0f;

    void Update()
    {
        if (Input.GetKeyDown("r") || levelManager.currentState == LevelManager.state.needsRestart || (Input.GetKeyDown("space") && levelManager.currentState == LevelManager.state.ending))
        {
            this.levelManager.Restart();
        }

        if (Input.GetKeyDown("x"))
        {
            levelManager.Pause();
        }

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
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

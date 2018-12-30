using UnityEngine;
using System.Collections.Generic;
using Assets.Utils;

public class ConfuseTemplate : LevelTemplate
{
    private static float rotateSpeed = 0.5f;

    public ConfuseTemplate()
    {
        this.BackgroundColor = new Color(0.63f, 0.22f, 0.54f);
        this.CircleColor = new Color(0.66f, 0.25f, 0.57f);
        this.PlatformColor = new Color(0.8f, 0.11f, 0.92f);
    }

    public override void UpdateCameraParams(CameraBehaviour cameraBehaviour)
    {
        base.UpdateCameraParams(cameraBehaviour);
        cameraBehaviour.cameraObject.transform.Rotate(new Vector3(0, 0, rotateSpeed));
    }

    public override string Word
    {
        get
        {
            return "CONFUSE";
        }
    }
}

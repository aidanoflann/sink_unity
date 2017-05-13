using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RotateTemplate : LevelTemplate
{
    public RotateTemplate()
    {
        this.BackgroundColor = Color.white;
        this.PlatformColor = Color.black;
        this.CircleColor = new Color(0.93f, 0.93f, 0.93f);
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        platform.w_pos = platform.w_pos + platform.w_vel * Time.deltaTime;
    }

    public override void Reload()
    {
        base.Reload();
    }
}


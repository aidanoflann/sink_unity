using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FallTemplate: LevelTemplate
{
    public FallTemplate()
    {
        this.BackgroundColor = Color.white;
        this.PlatformColor = Color.black;
        this.CircleColor = new Color(0.93f, 0.93f, 0.93f);
    }


    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        platform.r_pos = platform.r_pos + platform.r_vel * rSpeedMultiplier * Time.deltaTime;

    }

    public override void Reload()
    {
        base.Reload();
    }
}


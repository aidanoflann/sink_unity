using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FallTemplate: LevelTemplate
{
    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        platform.r_pos = platform.r_pos + platform.r_vel * rSpeedMultiplier * Time.deltaTime;

    }

    public override void Reload()
    {
        base.Reload();
    }

    public override Color PlatformColor
    {
        get
        {
            return Color.black;
        }
    }
}


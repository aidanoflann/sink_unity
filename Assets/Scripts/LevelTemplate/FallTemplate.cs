using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FallTemplate: LevelTemplate
{
    public override void UpdatePlatformPosition(Platform platform, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
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


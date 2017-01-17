using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FallTemplate: LevelTemplate
{
    public override void UpdatePlatformPosition(Platform platform, float rSpeedMultiplier)
    {
        platform.r_pos = platform.r_pos + platform.r_vel * rSpeedMultiplier * Time.deltaTime;
    }

    public override Color PlatformColor
    {
        get
        {
            return Color.black;
        }
    }
}


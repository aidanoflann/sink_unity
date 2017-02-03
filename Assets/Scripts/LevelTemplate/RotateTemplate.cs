using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RotateTemplate : LevelTemplate
{
    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        platform.w_pos = platform.w_pos + platform.w_vel * Time.deltaTime;
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


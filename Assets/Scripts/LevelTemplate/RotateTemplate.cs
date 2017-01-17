using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class RotateTemplate : LevelTemplate
{
    public override void UpdatePlatformPosition(Platform platform, float rSpeedMultiplier)
    {
        platform.w_pos = platform.w_pos + platform.w_vel * Time.deltaTime;
    }

    public override Color PlatformColor
    {
        get
        {
            return Color.black;
        }
    }
}


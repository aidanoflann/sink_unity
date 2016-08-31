using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LevelTemplate
{
    public void UpdatePlatformPosition(Platform platform, float rSpeedMultiplier)
    {
        // update position in polar coordinates
        float deltaTime = Time.deltaTime;
        platform.r_pos += platform.r_vel * rSpeedMultiplier * deltaTime;
        platform.w_pos = ((platform.w_pos + platform.w_vel * deltaTime) + 360f) % 360f;
    }
}


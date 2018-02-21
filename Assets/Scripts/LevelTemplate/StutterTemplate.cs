using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class StutterTemplate : SquareWaveTemplate
{
    private float timeSinceLastTick;
    private static float upRVel = +40f;
    private static float downRVel = -5f;

    public StutterTemplate()
    {
        this.BackgroundColor = new Color(0.3f, 0.2f, 0.2f);
        this.PlatformColor = new Color(0.2f, 0.1f, 0.16f);
        this.CircleColor = new Color(0.4f, 0.2f, 0.3f);
        this.tickDuration = 0.1f;
        this.tickPeriod = 1f;
    }

    public override void SetPlatformParameters(Platform platform, int platformIndex, int numPlatforms)
    {
        platform.r_vel = downRVel;
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];

        if (this.IsTicking)
        {
            platform.r_vel = upRVel;
        }
        else
        {
            platform.r_vel = downRVel;
        }
    }

    public override string Word
    {
        get
        {
            return "STUTTER";
        }
    }
}



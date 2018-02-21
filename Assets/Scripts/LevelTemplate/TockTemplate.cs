using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class TockTemplate : SquareWaveTemplate
{
    private static float maxRVel = -4f;
    private static float minRVel = 0f;

    public TockTemplate()
    {
        this.BackgroundColor = new Color(0.8f, 0.8f, 0.2f);
        this.PlatformColor = new Color(0.5f, 0.5f, 0.16f);
        this.CircleColor = new Color(0.9f, 0.9f, 0.3f);
        this.tickDuration = 0.3f;
        this.tickPeriod = 1f;
    }

    public override void SetPlatformParameters(Platform platform, int platformIndex, int numPlatforms)
    {
        platform.r_vel = minRVel;
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];

        if (this.IsTicking)
        {
            platform.r_vel = maxRVel;
        }
        else
        {
            platform.r_vel = minRVel;
        }
    }

    public override string Word
    {
        get
        {
            return "TOCK";
        }
    }
}



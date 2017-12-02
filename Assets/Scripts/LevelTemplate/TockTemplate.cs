using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class TockTemplate : SquareWaveTemplate
{
    private float timeSinceLastTick;
    private static float tickDuration = 0.3f; // time during which the tick is actually happening
    private static float tickPeriod = 1f; // how often the tick occurs
    private static float maxRVel = -5f;
    private static float minRVel = 0f;

    public TockTemplate()
    {
        this.BackgroundColor = new Color(0.8f, 0.8f, 0.2f);
        this.PlatformColor = new Color(0.5f, 0.5f, 0.16f);
        this.CircleColor = new Color(0.9f, 0.9f, 0.3f);
    }

    public override void SetPlatformParameters(Platform platform, int platformIndex, int numPlatforms)
    {
        //platform.r_vel *= 2;
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
}



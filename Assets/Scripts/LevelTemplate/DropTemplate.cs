using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class DropTemplate : SquareWaveTemplate
{
    private static float maxRVel = -28f;
    private static float minRVel = +4f;

    public DropTemplate()
    {
        this.BackgroundColor = new Color(0.15f, 0.09f, 0.02f);
        this.PlatformColor = new Color(0.35f, 0.29f, 0.22f);
        this.CircleColor = new Color(0.17f, 0.11f, 0.05f);
        this.tickDuration = 0.2f;
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
            return "DROP";
        }
    }
}



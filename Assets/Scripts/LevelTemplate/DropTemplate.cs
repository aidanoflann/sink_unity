using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class DropTemplate : SquareWaveTemplate
{
    private static float rVelDelta = -32f;
    private static float bonusRVel = 4.7f; // This is added at the start to account for the drop.

    public DropTemplate()
    {
        this.BackgroundColor = new Color(0.15f, 0.09f, 0.02f);
        this.PlatformColor = new Color(0.35f, 0.29f, 0.22f);
        this.CircleColor = new Color(0.17f, 0.11f, 0.05f);
        this.tickDuration = 0.2f;
    }

    public override void SetPlatformParameters(Platform platform, int platformIndex, int numPlatforms)
    {
        platform.r_vel += bonusRVel;
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];

        if (this.TickedThisUpdate)
        {
            platform.r_vel += rVelDelta;
        }
        if (this.UntickedThisUpdate)
        {
            platform.r_vel -= rVelDelta;
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



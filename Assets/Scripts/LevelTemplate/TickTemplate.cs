using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class TickTemplate : SquareWaveTemplate
{
    private static float wVelocityMaxMultiplier = 250f;
    private static float wVelocityMinMultiplier = 0.01f;

    public TickTemplate()
    {
        this.tickDuration = 0.25f;
        this.tickPeriod = 1f;
        this.BackgroundColor = new Color(0.8f, 0.48f, 0.2f);
        this.PlatformColor = new Color(0.95f, 0.88f, 0.16f);
        this.CircleColor = new Color(0.9f, 0.58f, 0.3f);
    }

    public override void SetPlatformParameters(Platform platform, int platformIndex, int numPlatforms)
    {
        // Remove the constant background speed from all platforms
        platform.w_vel *= wVelocityMinMultiplier;
        // consider widening w_size, this one's tricky
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];

        if (this.IsTicking)
        {
            platform.w_vel.SetValue(Mathf.Sign(platform.w_vel.GetValue()) * wVelocityMaxMultiplier);
        }
        else
        {
            platform.w_vel.SetValue(Mathf.Sign(platform.w_vel.GetValue()) * wVelocityMinMultiplier);
        }
    }

    public override string Word
    {
        get
        {
            return "TICK";
        }
    }
}



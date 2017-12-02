using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class ReverseTemplate : SquareWaveTemplate
{
    private static float wVelocityMaxMultiplier = 250f;
    private static float wVelocityMinMultiplier = 0.01f;
    private bool ticked;

    public ReverseTemplate()
    {
        tickDuration = 0.5f;
        tickPeriod = 1.5f;
        this.BackgroundColor = new Color(0.2f, 0.2f, 0.2f);
        this.PlatformColor = new Color(0.8f, 0.8f, 0.8f);
        this.CircleColor = new Color(0.3f, 0.3f, 0.3f);
    }

    public override void SetPlatformParameters(Platform platform, int platformIndex, int numPlatforms)
    {
        base.SetPlatformParameters(platform, platformIndex, numPlatforms);
        platform.w_vel *= 1.4f;
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        if (this.IsTicking && !this.ticked)
        {    
            platform.w_vel.SetValue(-platform.w_vel.GetValue());
            if(platformIndex == allPlatforms.Count - 1)
            {
                // all platforms have been flipped, don't repeat until next tick
                this.ticked = true;
            }
        }
        if (!this.IsTicking)
        {
            this.ticked = false;
        }
    }
}



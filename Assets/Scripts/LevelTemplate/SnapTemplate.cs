using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class SnapTemplate : SquareWaveTemplate
{
    private static float speedDuringSnap = 2000f;
    private static float maxSizeMultiplier = 2f;
    private static float platformWSizeModifier = 0.5f;

    public SnapTemplate()
    {
        this.tickDuration = 1f;
        this.tickPeriod = 2f;
        this.BackgroundColor = new Color(0.5f, 0.5f, 1f);
        this.PlatformColor = new Color(0.3f, 0.9f, 1f);
        this.CircleColor = new Color(0.5f, 0.5f, 0.9f);
    }

    public override void SetPlatformParameters(Platform platform, int platformIndex, int numPlatforms)
    {
        base.SetPlatformParameters(platform, platformIndex, numPlatforms);
        if(platformIndex != 0)
        {
            platform.w_size *= platformWSizeModifier;
        }
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];

        if (this.IsTicking && platform.w_size < platform.OriginalWsize * maxSizeMultiplier)
        {
            platform.w_size.SetValue(platform.w_size.GetValue() + speedDuringSnap * Time.deltaTime);
        }
        if (!this.IsTicking && platform.w_size > platform.OriginalWsize)
        {
            platform.w_size.SetValue(platform.w_size.GetValue() - speedDuringSnap * Time.deltaTime);
        }
    }

    public override string Word
    {
        get
        {
            return "SNAP";
        }
    }
}



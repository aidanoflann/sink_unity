using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class FattenTemplate : SinWaveTemplate
{
    public FattenTemplate()
    {
        this.BackgroundColor = new Color(1, 0.9f, 0.7f);
        this.PlatformColor = new Color(0.9f, 0.4f, 0.1f);
        this.CircleColor = new Color(1f, 0.9f, 0.6f);
        this.amplitude = 0.2f;
    }

    public override void SetPlatformParameters(Platform platform, int platformIndex, int numPlatforms)
    {
        base.SetPlatformParameters(platform, platformIndex, numPlatforms);
        platform.w_size *= 0.4f;
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        platform.r_size += this.amplitude * this.SignedSinValue;
    }

    public override string Word
    {
        get
        {
            return "FATTEN";
        }
    }
}

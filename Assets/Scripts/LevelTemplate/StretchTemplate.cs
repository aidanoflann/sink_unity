using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class StretchTemplate : SinWaveTemplate
{
    public StretchTemplate()
    {
        this.BackgroundColor = new Color(0.2f, 0.9f, 0.7f);
        this.PlatformColor = new Color(0, 0.4f, 0.3f);
        this.CircleColor = new Color(0.1f, 8f, 0.6f);
        this.amplitude = 5f;
    }

    public override void SetPlatformParameters(Platform platform, int platformIndex, int numPlatforms)
    {
        platform.w_size *= 0.4f;
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        platform.w_size.SetValue(platform.w_size.GetValue() + this.SignedSinValue);
    }

    public override string Word
    {
        get
        {
            return "STRETCH";
        }
    }
}



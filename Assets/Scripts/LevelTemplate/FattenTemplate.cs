using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class FattenTemplate : SinWaveTemplate
{
    private static float RAmplitude = 0.7f;
    private static float WAmplitude = 2f;

    private float? originalRSize;

    public FattenTemplate()
    {
        this.BackgroundColor = new Color(1, 0.9f, 0.7f);
        this.PlatformColor = new Color(0.9f, 0.4f, 0.1f);
        this.CircleColor = new Color(1f, 0.9f, 0.6f);
    }

    public override void SetPlatformParameters(Platform platform, int platformIndex, int numPlatforms)
    {
        base.SetPlatformParameters(platform, platformIndex, numPlatforms);
        platform.w_size *= 0.4f;
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        if ( this.originalRSize == null )
        {
            this.originalRSize = platform.r_size;
        }
        platform.r_size = this.originalRSize.Value + RAmplitude * (Mathf.Sin(this.currentAngle) + 1f);
        platform.w_size.SetValue(platform.w_size.GetValue() + WAmplitude * (Mathf.Sin(this.currentAngle)));
    }
}

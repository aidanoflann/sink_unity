using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class WaveTemplate : SinWaveTemplate
{
    private static float phaseOffSet = 90f / 360f * Globals.twoPi;

    private float? originalRVel;

    public WaveTemplate()
    {
        this.BackgroundColor = new Color(0.47f, 0.64f, 0.98f);
        this.PlatformColor = new Color(0.23f, 0.4f, 0.74f);
        this.CircleColor = new Color(0.43f, 0.6f, 0.94f);
        this.angularSpeed = 3.0f;
        this.amplitude = 1.5f;
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        if ( this.originalRVel == null)
        {
            this.originalRVel = platform.r_vel;
        }
        float angleWithOffset = (this.currentAngle + platformIndex * phaseOffSet) % Globals.twoPi;
        platform.r_vel = this.originalRVel.Value + amplitude * Mathf.Sin(angleWithOffset);
    }

    public override string Word
    {
        get
        {
            return "WAVE";
        }
    }
}

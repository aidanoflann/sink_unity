using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class PulseTemplate : SinWaveTemplate
{
    private float? originalRVel;

    public PulseTemplate()
    {
        this.BackgroundColor = new Color(1, 0.77f, 0.7f);
        this.PlatformColor = new Color(0.9f, 0.1f, 0.1f);
        this.CircleColor = new Color(1f, 0.7f, 0.6f);
        this.angularSpeed = 6f;
        this.amplitude = 8f;
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        if ( this.originalRVel == null)
        {
            this.originalRVel = platform.r_vel;
        }
        platform.r_vel = this.originalRVel.Value +  amplitude * Mathf.Sin(this.currentAngle);
    }

    public override string Word
    {
        get
        {
            return "PULSE";
        }
    }
}

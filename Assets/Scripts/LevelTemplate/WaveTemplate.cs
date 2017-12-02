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
        angularSpeed = 3.0f;
        amplitude = 4f;
    }

    public override void UpdateTemplate()
    // Update the template's internally-stored currentAngle
    {
        base.UpdateTemplate();
        this.currentAngle = (this.currentAngle + angularSpeed * Time.deltaTime) % Globals.twoPi;
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        if ( this.originalRVel == null)
        {
            this.originalRVel = platform.r_vel;
        }
        float angleWithOffset = (this.currentAngle + platformIndex * phaseOffSet) % Globals.twoPi;
        platform.r_vel = this.originalRVel.Value +  amplitude * Mathf.Sin(angleWithOffset);
    }
}

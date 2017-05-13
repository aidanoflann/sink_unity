using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class PulseTemplate : LevelTemplate
{
    private float angularSpeed;
    private float currentAngle;
    private float amplitude;

    private float? originalRVel;

    public PulseTemplate()
    {
        this.BackgroundColor = new Color(1, 0.77f, 0.7f);
        this.PlatformColor = new Color(0.9f, 0.1f, 0.1f);
        this.CircleColor = new Color(1f, 0.7f, 0.6f);
        this.currentAngle = 0.0f;
        this.angularSpeed = 1.0f;
        this.amplitude = 12.0f;
    }
    
    public override void Reload()
    {
        base.Reload();
        this.currentAngle = 0.0f;
    }


    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        if ( this.originalRVel == null)
        {
            this.originalRVel = platform.r_vel;
        }
        this.currentAngle = (this.currentAngle + this.angularSpeed * Time.deltaTime) % Globals.twoPi;
        platform.r_vel = this.originalRVel.Value +  this.amplitude * Mathf.Sin(this.currentAngle);
    }
}

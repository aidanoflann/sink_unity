using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class DilateTemplate : LevelTemplate
{
    private float angularSpeed;
    private float currentAngle;
    private float amplitude;

    public DilateTemplate()
    {
        this.BackgroundColor = new Color(0.2f, 0.9f, 0.7f);
        this.PlatformColor = new Color(0, 0.4f, 0.3f);
        this.CircleColor = new Color(0.1f, 8f, 0.6f);
        this.currentAngle = 0.0f;
        this.angularSpeed = 1f;
        this.amplitude = 1.0f;
    }

    public override void Reload()
    {
        this.currentAngle = 0.0f;
    }

    public override void SetPlatformParameters(int platformIndex, List<Platform> allPlatforms)
    {
        Platform platform = allPlatforms[platformIndex];
        platform.w_size *= 1f;
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        this.currentAngle = (this.currentAngle + this.angularSpeed * Time.deltaTime) % Globals.twoPi;
        platform.w_size += this.amplitude * (0.5f + Mathf.Sin(this.currentAngle));
        if (platform.w_size > 359.0f)
        {
            platform.w_size = 359.0f;
        }
    }
}



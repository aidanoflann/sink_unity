using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class TickTemplate : LevelTemplate
{
    private float angularSpeed;
    private float currentAngle;
    private float amplitude;

    public TickTemplate()
    {
        this.BackgroundColor = new Color(0.6f, 0.9f, 0.6f);
        this.PlatformColor = new Color(0.1f, 0.9f, 0.1f);
        this.currentAngle = 0.0f;
        this.angularSpeed = 1f;
        this.amplitude = 4f;
    }

    public override void Reload()
    {
        this.currentAngle = 0.0f;
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        this.currentAngle = (this.currentAngle + this.angularSpeed * Time.deltaTime) % Globals.twoPi;
        platform.w_pos += this.amplitude * Mathf.Sin(this.currentAngle);
    }
}



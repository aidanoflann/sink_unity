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

    public PulseTemplate()
    {
        this.BackgroundColor = new Color(0.9f, 0.6f, 0.6f);
        this.PlatformColor = new Color(0.9f, 0.1f, 0.1f);
        this.currentAngle = 0.0f;
        this.angularSpeed = 0.7f;
        this.amplitude = 0.15f;
    }

    public override void UpdatePlatformPosition(Platform platform, float rSpeedMultiplier)
    {
        this.currentAngle = (this.currentAngle + this.angularSpeed * Time.deltaTime) % Globals.twoPi;
        platform.r_pos += this.amplitude * Mathf.Sin(this.currentAngle);
    }
}

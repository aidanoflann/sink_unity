using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class TwirlTemplate : LevelTemplate
// Previously called TickTemplate which was altered to actually tick, this template uses a sin oscillation of wpos. Very difficult.
{
    private float angularSpeed;
    private float currentAngle;
    private float amplitude;

    public TwirlTemplate()
    {
        this.BackgroundColor = new Color(0.6f, 0.9f, 0.6f);
        this.PlatformColor = new Color(0, 0.4f, 0);
        this.CircleColor = new Color(0.7f, 1f, 0.7f);
        this.currentAngle = 0.0f;
        this.angularSpeed = 3f;
        this.amplitude = 4f;
    }

    public override void Reload()
    {
        this.currentAngle = 0.0f;
    }

    public override void UpdateTemplate()
    {
        base.UpdateTemplate();
        this.currentAngle = (this.currentAngle + this.angularSpeed * Time.deltaTime) % Globals.twoPi;
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        platform.w_pos += this.amplitude * Mathf.Sin(this.currentAngle);
    }
}



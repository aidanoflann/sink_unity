using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class TwirlTemplate : SinWaveTemplate
// Previously called TickTemplate which was altered to actually tick, this template uses a sin oscillation of wpos. Very difficult.
{
    public TwirlTemplate()
    {
        this.BackgroundColor = new Color(0.6f, 0.9f, 0.6f);
        this.PlatformColor = new Color(0, 0.4f, 0);
        this.CircleColor = new Color(0.7f, 1f, 0.7f);
        this.angularSpeed = 3.0f;
        this.amplitude = 4f;
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        platform.w_pos += amplitude * Mathf.Sin(this.currentAngle);
    }

    public override string Word
    {
        get
        {
            return "TWIRL";
        }
    }
}



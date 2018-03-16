using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class PinchTemplate : SquareWaveTemplate
{
    private static float speedDuringSnap = 1.6f;

    public PinchTemplate()
    {
        this.tickDuration = 0.75f;
        //this.tickPeriod = 1.5f;
        this.BackgroundColor = new Color(0.9f, 0.5f, 1f);
        this.PlatformColor = new Color(0.9f, 0.9f, 1f);
        this.CircleColor = new Color(0.9f, 0.5f, 0.9f);
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        // platforms moving left jump down during tick, and up during non-tick
        float rVelSign = Mathf.Sign(platform.w_vel.GetValue());

        if (this.IsTicking)
        {
            platform.r_pos += speedDuringSnap * rVelSign * Time.deltaTime;
        }
        if (!this.IsTicking)
        {
            platform.r_pos -= speedDuringSnap * rVelSign * Time.deltaTime;
        }
    }

    public override string Word
    {
        get
        {
            return "PINCH";
        }
    }
}



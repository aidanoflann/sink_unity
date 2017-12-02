using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class TockTemplate : LevelTemplate
{
    private float timeSinceLastTick;
    private static float tickDuration = 0.3f; // time during which the tick is actually happening
    private static float tickPeriod = 1f; // how often the tick occurs
    private static float maxRVel = -5f;
    private static float minRVel = 0f;
    private bool isTicking = false;

    public TockTemplate()
    {
        this.BackgroundColor = new Color(0.8f, 0.8f, 0.2f);
        this.PlatformColor = new Color(0.5f, 0.5f, 0.16f);
        this.CircleColor = new Color(0.9f, 0.9f, 0.3f);
    }

    public override void Reload()
    {
        this.timeSinceLastTick = 0f;
        this.isTicking = false;
    }

    public override void SetPlatformParameters(Platform platform, int platformIndex, int numPlatforms)
    {
        //platform.r_vel *= 2;
    }

    public override void UpdateTemplate()
    {
        base.UpdateTemplate();
        // modulo the time since last tick by the total period
        this.timeSinceLastTick = (this.timeSinceLastTick + Time.deltaTime) % tickPeriod;
        // if the platform is not ticking but the time falls within the tick duration, start ticking
        if (this.timeSinceLastTick < tickDuration && !this.isTicking)
        {
            this.isTicking = true;
        }
        // if the platform is ticking but the time no longer falls within the tick duration, stop ticking
        if (this.timeSinceLastTick > tickDuration && this.isTicking)
        {
            this.isTicking = false;
        }
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];

        if (this.isTicking)
        {
            platform.r_vel = maxRVel;
        }
        else
        {
            platform.r_vel = minRVel;
        }
    }
}



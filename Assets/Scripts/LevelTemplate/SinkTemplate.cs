using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SinkTemplate : LevelTemplate
{
    private float sinkSpeed = 1.0f;
    private float riseSpeed = 3.0f;
    private float platformFallBuffer = 1f;
    private float platformRiseBuffer = 2f;

    public SinkTemplate()
    {
        this.BackgroundColor = new Color(0.92f, 0.85f, 0.77f);
        this.CircleColor = new Color(0.87f, 0.74f, 0.62f);
        this.PlatformColor = new Color(0.52f, 0.34f, 0.17f);
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        //platform.w_pos = platform.w_pos + platform.w_vel * Time.deltaTime;
        Platform platform = allPlatforms[platformIndex];
        if (platform.hasPlayer)
        {
            if (this.NeedsToFall(platformIndex, allPlatforms))
            {
                platform.r_pos = platform.r_pos - this.sinkSpeed * Time.deltaTime;
            }
        }
        else if (platform.hadPlayer)
        {
            if (this.NeedsToRise(platformIndex, allPlatforms))
            {
                platform.r_pos = platform.r_pos + this.riseSpeed * Time.deltaTime;
            }
        }
    }

    private bool NeedsToRise(int platformIndex, List<Platform> allPlatforms)
    // helper function, calculates if given platform is closer to platform below than above
    {
        if (platformIndex == allPlatforms.Count - 1)
        {
            // never rise if it's the top platform
            return false;
        }
        else
        {
            Platform currentPlatform = allPlatforms[platformIndex];
            Platform platformAbove = allPlatforms[platformIndex + 1];
            return (platformAbove.r_pos - currentPlatform.r_pos) > this.platformRiseBuffer;
        }
    }

    private bool NeedsToFall(int platformIndex, List<Platform> allPlatforms)
    // helper function, only fall if above a certain buffer from the platform below
    {
        if (platformIndex == 0)
        {
            // always keep falling if the given platform is the lowest
            return true;
        }
        else
        {
            Platform currentPlatform = allPlatforms[platformIndex];
            Platform platformBelow = allPlatforms[platformIndex - 1];
            return (currentPlatform.r_pos - platformBelow.r_pos) > this.platformFallBuffer;
        }
    }

    public override void Reload()
    {
        base.Reload();
    }
}


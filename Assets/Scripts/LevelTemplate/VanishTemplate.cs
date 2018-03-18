using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class VanishTemplate: SquareWaveTemplate
{
    public VanishTemplate()
    {
        this.BackgroundColor = new Color(0.4f, 0.7f, 1f);
        this.PlatformColor = new Color(0.05f, 0.3f, 0.8f); 
        this.CircleColor = new Color(0.3f, 0.6f, 1f);

        this.tickDuration = 0.6f;
    }


    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        if (this.IsTicking)
        {
            if (platform.IsVisible)
            {
                platform.SetInvisible();
            }
        }
        else
        {
            if (!platform.IsVisible)
            {
                platform.SetVisible();
            }
        }
    }

    public override string Word
    {
        get
        {
            return "VANISH";
        }
    }
}


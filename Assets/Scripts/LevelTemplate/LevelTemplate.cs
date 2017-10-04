using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LevelTemplate
{
    public virtual void SetPlatformParameters(int platformIndex, List<Platform> allPlatforms)
    // Called once at start of level - use to set w_size, r_size, etc
    {
    }

    public virtual void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
    }

    public virtual void Reload()
    {
    }

    public virtual Color PlatformColor
    {
        get;
        set;
    }

    public virtual Color BackgroundColor
    {
        get;
        set;
    }

    public virtual Color CircleColor
    {
        get;
        set;
    }
}


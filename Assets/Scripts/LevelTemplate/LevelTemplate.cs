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
    // called every update, primarily for adjusting platform motion for the level mechanic
    {
    }

    public virtual void Reload()
    // reset any variables that are stored on the class instance
    {
    }

    public virtual void UpdatePlayer(Player player)
    // called every update, for forcing player movement, bouncing them on land, etc.
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


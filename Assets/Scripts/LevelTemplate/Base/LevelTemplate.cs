using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LevelUpdate
{
    public LevelUpdate(bool triggerBeatSound, string soundToPlay)
    {
        this.triggerBeatSound = triggerBeatSound;
        this.soundToPlay = soundToPlay;
    }

    public bool triggerBeatSound = false;
    public string soundToPlay = "";
}

public class LevelTemplate
{
    public virtual void SetPlatformParameters(Platform platform, int platformIndex, int numPlatforms)
    // Called once at start of level - use to set w_size, r_size, etc
    {
    }

    public virtual void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    // called every update for each platform, primarily for adjusting platform motion for the level mechanic
    // IMPORTANT: Best practice is for this function to ADD TO parameters only. DO NOT set them.
    // Reason: any number of UpdatePlatformPositions can be called per frame, and their effects need to be additive.
    {
    }

    public virtual void UpdateCameraParams(CameraBehaviour cameraBehaviour)
    // called every update
    {

    }

    public void OverridePlatformParams(int platformIndex, List<Platform> allPlatforms, float? rSpeedOverride)
    {
        if (rSpeedOverride.HasValue)
        {
            allPlatforms[platformIndex].r_vel = rSpeedOverride.Value;
        }
    }

    public virtual void Reload()
    // reset any variables that are stored on the class instance
    {
    }

    public virtual void UpdatePlayer(Player player)
    // called every update, for forcing player movement, bouncing them on land, etc.
    {
    }

    public virtual LevelUpdate UpdateTemplate()
    // called every update, for updating params on the template (not specific to player or platforms)
    {
        return new LevelUpdate(false, "");
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

    public virtual string Word  // Descriptive action word, e.g. "sink", "tick", etc.
    {
        get;
        set;
    }
}


using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LevelTemplate
{
    public virtual void UpdatePlatformPosition(Platform platform, float rSpeedMultiplier)
    {
    }

    public Color PlatformColor
    {
        get;
        set;
    }

    public Color BackgroundColor
    {
        get;
        set;
    }
}


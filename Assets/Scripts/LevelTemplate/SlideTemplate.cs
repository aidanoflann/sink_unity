using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SlideTemplate : LevelTemplate
{
    public SlideTemplate()
    {
        this.BackgroundColor = new Color(0.6f, 0.2f, 0.6f);
        this.PlatformColor = new Color(0.9f, 0.1f, 0.9f);
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        //platform.w_pos = platform.w_pos + platform.w_vel * Time.deltaTime;
    }

    public override void Reload()
    {
        base.Reload();
    }

    public override string Word
    {
        get
        {
            return "SLIDE";
        }
    }
}


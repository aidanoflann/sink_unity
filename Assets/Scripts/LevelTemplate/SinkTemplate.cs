using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SinkTemplate : LevelTemplate
{
    private float sinkSpeed = 0.5f;

    public SinkTemplate()
    {
        this.BackgroundColor = new Color(0.6f, 0.2f, 0.6f);
        this.PlatformColor = new Color(0.9f, 0.1f, 0.9f);
    }

    public override void UpdatePlatformPosition(Platform platform, float rSpeedMultiplier)
    {
        //platform.w_pos = platform.w_pos + platform.w_vel * Time.deltaTime;
        if (platform.hasPlayer)
        {
            platform.r_pos = platform.r_pos - this.sinkSpeed * Time.deltaTime;
        }
    }

    public override void Reload()
    {
        base.Reload();
    }
}


using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TickTemplate: LevelTemplate
{
    public override void UpdatePlatformPosition(Platform platform, float rSpeedMultiplier)
    {
        //TODO: re-add tick behaviour
    }

    public override Color PlatformColor
    {
        get
        {
            return Color.green;
        }
    }
}


using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class SlipTemplate : LevelTemplate
{
    private static float maxRVel = -28f;
    private static float minRVel = +4f;

    public SlipTemplate()
    {
        this.BackgroundColor = new Color(0.67f, 0.84f, 1f);
        this.PlatformColor = new Color(0.43f, 0.6f, 0.94f);
        this.CircleColor = new Color(0.63f, 0.8f, 0.94f);
    }

    public override void UpdatePlayer(Player player)
    {
        base.UpdatePlayer(player);
        float offset = 0.1f * Time.deltaTime;
        if (player.MovingClockwise)
        {
            offset *= -1f;
        }

        if (player.IsLanded)
        {
            player.SetPlatformPosition(player.GetPlatformPosition() + offset);
        }
    }

    public override string Word
    {
        get
        {
            return "SLIP";
        }
    }
}



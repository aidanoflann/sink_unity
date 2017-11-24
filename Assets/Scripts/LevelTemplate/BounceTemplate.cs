using UnityEngine;
using System.Collections.Generic;
using Assets.Utils;

public class BounceTemplate : LevelTemplate
{

    public BounceTemplate()
    {
        this.BackgroundColor = new Color(0.92f, 0.92f, 0.92f);
        this.CircleColor = new Color(0.52f, 0.52f, 0.52f);
        this.PlatformColor = new Color(0.1f, 0.1f, 0.1f);
    }

    public override void UpdatePlayer(Player player)
    {
        base.UpdatePlayer(player);
        if(player.IsLanded)
        {
            player.Jump(1.0f);
        }
    }

    public override void Reload()
    {
        base.Reload();
    }
}


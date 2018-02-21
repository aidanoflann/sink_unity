using UnityEngine;
using System.Collections.Generic;
using Assets.Utils;

public class BounceTemplate : LevelTemplate
{
    private static int numBouncesPerJump = 3;
    private int numBouncesRemaining = numBouncesPerJump;

    public BounceTemplate()
    {
        this.BackgroundColor = new Color(0.92f, 0.92f, 0.92f);
        this.CircleColor = new Color(0.52f, 0.52f, 0.52f);
        this.PlatformColor = new Color(0.1f, 0.1f, 0.1f);
    }

    public override void UpdatePlayer(Player player)
    {
        base.UpdatePlayer(player);
        if(player.IsLanded && this.numBouncesRemaining > 1)
        {
            player.Jump(0.4f * this.numBouncesRemaining);
            this.numBouncesRemaining--;
        }
        else if (player.IsLanded && this.numBouncesRemaining == 1)
        {
            this.numBouncesRemaining--;
        }
        else if (!player.IsLanded && this.numBouncesRemaining == 0)
        {
            this.numBouncesRemaining = numBouncesPerJump;
        }
    }

    public override void Reload()
    {
        base.Reload();
    }

    public override string Word
    {
        get
        {
            return "BOUNCE";
        }
    }
}

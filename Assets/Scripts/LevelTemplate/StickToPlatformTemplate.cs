using UnityEngine;
using System.Collections.Generic;
using Assets.Utils;

public class StickToPlatformTemplate : LevelTemplate
// Base template, solely used for enforcing player follows platform motion
{

    public override void UpdatePlayer(Player player)
    {
        base.UpdatePlayer(player);
        player.SetPostToLandedPlatform();
    }
}

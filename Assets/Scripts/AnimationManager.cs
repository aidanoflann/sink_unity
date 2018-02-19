using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Utils;

public class AnimationManager {

    private MovingTextCanvasBehaviour movingTextCanvasBehaviour;
    private List<LevelTemplate> levelTemplates;
    private Player player;
    private CameraBehaviour cameraBehaviour;

    private bool animationStarted = false;

    public AnimationManager(MovingTextCanvasBehaviour mtcb, List<LevelTemplate> lts, Player p)
    {
        this.movingTextCanvasBehaviour = mtcb;
        this.levelTemplates = lts;
        this.player = p;
    }

    public void SetCameraBehaviour(CameraBehaviour cameraBehaviour)
    {
        this.cameraBehaviour = cameraBehaviour;
    }

    public void Reset()
    {
        this.animationStarted = false;
    }

    // This function is responsible for all behaviour from the level load to the initial fall of the player onto the level.
    // It needs to trigger all text animations, focus the camera correctly, then return True when completed.
    public bool HandleStartingAnimation()
    {
        // prepare the level entry text animation
        if (!this.animationStarted)
        {
            this.cameraBehaviour.SnapToPlayer();

            this.movingTextCanvasBehaviour.SetTextColor(this.levelTemplates[this.levelTemplates.Count - 1].PlatformColor);
            this.movingTextCanvasBehaviour.SetText("HEY THERE");
            this.movingTextCanvasBehaviour.AnimateToPoint(this.player.transform.position);
            this.animationStarted = true;
        }
        this.cameraBehaviour.FollowPlayer(false, 20f);

        // if higher than 100f, keep animating - THIS MIGHT GO BAD
        if (this.player.RPos < 100f)
        {
            this.Reset();
            return true;
        }
        else
        {
            return false;
        }
    }
}

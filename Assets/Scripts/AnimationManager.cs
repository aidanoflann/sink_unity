using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Utils;

public class AnimationManager {

    private MovingTextCanvasBehaviour movingTextCanvasBehaviour;
    private MovingTextCanvasBehaviour movingScoreTextCanvasBehaviour;
    private Player player;
    private CameraBehaviour cameraBehaviour;
    private StatManagerBehaviour statManagerBehaviour;

    private bool animationStarted = false;
    private float timeSinceAnimationStart = 0;
    private float timeToSpendAnimating = 3; // seconds

    public AnimationManager(
        MovingTextCanvasBehaviour movingTextCanvasBehaviour,
        MovingTextCanvasBehaviour movingScoreTextCanvasBehaviour,
        StatManagerBehaviour statManagerBehaviour
        )
    {
        this.movingTextCanvasBehaviour = movingTextCanvasBehaviour;
        this.movingScoreTextCanvasBehaviour = movingScoreTextCanvasBehaviour;
        this.statManagerBehaviour = statManagerBehaviour;
    }

    public void SetCameraBehaviour(CameraBehaviour cameraBehaviour)
    {
        this.cameraBehaviour = cameraBehaviour;
    }

    public void Reset()
    {
        this.animationStarted = false;
        this.timeSinceAnimationStart = 0f;
    }

    public void SetWords(string words)
    {
        this.movingTextCanvasBehaviour.SetText(words);
        this.movingScoreTextCanvasBehaviour.SetText(string.Format("SCORE:{0}", this.statManagerBehaviour.GetTotalScore()));
    }

    public void SetTextColour(Color colour)
    {
        this.movingTextCanvasBehaviour.SetTextColor(colour);
        this.movingScoreTextCanvasBehaviour.SetTextColor(colour);
    }

    // This function is responsible for all behaviour from the level load to the initial fall of the player onto the level.
    // It needs to trigger all text animations, focus the camera correctly, then return True when completed.
    public bool HandleStartingAnimation()
    {
        // prepare the level entry text animation
        if (!this.animationStarted)
        {
            this.cameraBehaviour.SnapToPlayer();
            this.movingTextCanvasBehaviour.AnimateToPoint(new Vector3(0, 200, -1), new Vector3(1000, 1000, -1));
            this.movingScoreTextCanvasBehaviour.AnimateToPoint(new Vector3(0, -200, -1), new Vector3(-1000, -1000, -1));
            this.animationStarted = true;
        }
        else
        {
            this.timeSinceAnimationStart += Time.deltaTime;
        }
        this.cameraBehaviour.FollowPlayer(false, 20f);

        // if higher than 100f, keep animating - THIS MIGHT GO BAD
        if (this.timeSinceAnimationStart > this.timeToSpendAnimating)
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

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SquareWaveTemplate: LevelTemplate
{
    private float timeSinceLastTick;
    private bool isTicking;
    private bool tickedThisUpdate;
    private bool untickedThisUpdate;
    protected float tickDuration; // time during which the tick is actually happening
    protected float tickPeriod = 1.5f; // how often the tick occurs

    protected bool IsTicking
    {
        get
        {
            return this.isTicking;
        }
    }

    protected bool TickedThisUpdate
    {
        get
        {
            return this.tickedThisUpdate;
        }
    }

    protected bool UntickedThisUpdate
    {
        get
        {
            return this.untickedThisUpdate;
        }
    }

    public override LevelUpdate UpdateTemplate()
    {
        // these bools should only ever be true for one frame.
        if (this.tickedThisUpdate)
        {
            this.tickedThisUpdate = false;
        }
        if (this.untickedThisUpdate)
        {
            this.untickedThisUpdate = false;
        }

        LevelUpdate levelUpdate = base.UpdateTemplate();
        // modulo the time since last tick by the total period
        this.timeSinceLastTick = (this.timeSinceLastTick + Time.deltaTime) % tickPeriod;
        // if the platform is not ticking but the time falls within the tick duration, start ticking
        if (this.timeSinceLastTick < tickDuration && !this.isTicking)
        {
            this.isTicking = true;
            this.tickedThisUpdate = true;
            levelUpdate.triggerBeatSound = true;
            levelUpdate.soundToPlay = "SquareBeat";
        }
        // if the platform is ticking but the time no longer falls within the tick duration, stop ticking
        if (this.timeSinceLastTick > tickDuration && this.isTicking)
        {
            this.isTicking = false;
            this.untickedThisUpdate = true;
        }
        return levelUpdate;
    }

    public override void Reload()
    {
        base.Reload();
        this.timeSinceLastTick = 0f;
        this.isTicking = false;
    }
}


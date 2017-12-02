using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SquareWaveTemplate: LevelTemplate
{
    private float timeSinceLastTick;
    private bool isTicking;
    protected static float tickDuration; // time during which the tick is actually happening
    protected static float tickPeriod; // how often the tick occurs

    protected bool IsTicking
    {
        get
        {
            return this.isTicking;
        }
    }

    public override void UpdateTemplate()
    {
        base.UpdateTemplate();
        // modulo the time since last tick by the total period
        this.timeSinceLastTick = (this.timeSinceLastTick + Time.deltaTime) % tickPeriod;
        // if the platform is not ticking but the time falls within the tick duration, start ticking
        if (this.timeSinceLastTick < tickDuration && !this.isTicking)
        {
            this.isTicking = true;
        }
        // if the platform is ticking but the time no longer falls within the tick duration, stop ticking
        if (this.timeSinceLastTick > tickDuration && this.isTicking)
        {
            this.isTicking = false;
        }
    }

    public override void Reload()
    {
        base.Reload();
        this.timeSinceLastTick = 0f;
        this.isTicking = false;
    }
}


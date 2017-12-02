﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SinWaveTemplate: LevelTemplate
{
    protected float angularSpeed;
    protected float amplitude;
    public float currentAngle = 0f;

    public override void Reload()
    {
        base.Reload();
        this.currentAngle = 0.0f;
    }

    public override void UpdateTemplate()
    {
        base.UpdateTemplate();
        this.currentAngle = (this.currentAngle + angularSpeed * Time.deltaTime) % Globals.twoPi;
    }

    protected float NormalizedSinValue
    // value from 0 -> amplitude between which the template oscillates.
    {
        get
        {
            return amplitude * (0.5f + Mathf.Sin(this.currentAngle));
        }
    }

    protected float SignedSinValue
    // value from -amplitude -> amplitude between which the template oscillates.
    {
        get
        {
            return amplitude * (Mathf.Sin(this.currentAngle));
        }
    }
}


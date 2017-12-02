using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SinWaveTemplate: LevelTemplate
{
    protected static float angularSpeed = 1f;
    protected static float amplitude = 2.0f;
    public float currentAngle = 0f;

    public override void Reload()
    {
        base.Reload();
        this.currentAngle = 0.0f;
    }

    public override void UpdateTemplate()
    // Update the template's internally-stored currentAngle
    {
        base.UpdateTemplate();
        this.currentAngle = (this.currentAngle + angularSpeed * Time.deltaTime) % Globals.twoPi;
    }
}


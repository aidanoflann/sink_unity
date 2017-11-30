using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class FattenTemplate : LevelTemplate
{
    private float angularSpeed;
    private float currentAngle;
    private float amplitude;

    private float? originalRSize;

    public FattenTemplate()
    {
        this.BackgroundColor = new Color(1, 0.9f, 0.7f);
        this.PlatformColor = new Color(0.9f, 0.4f, 0.1f);
        this.CircleColor = new Color(1f, 0.9f, 0.6f);
        this.currentAngle = 0.0f;
        this.angularSpeed = 4.0f;
        this.amplitude = 0.7f;
    }
    
    public override void Reload()
    {
        base.Reload();
        this.currentAngle = 0.0f;
    }

    public override void UpdateTemplate()
    // Update the template's internally-stored currentAngle
    {
        base.UpdateTemplate();
        this.currentAngle = (this.currentAngle + this.angularSpeed * Time.deltaTime) % Globals.twoPi;
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        Platform platform = allPlatforms[platformIndex];
        if ( this.originalRSize == null)
        {
            this.originalRSize = platform.r_size;
        }
        platform.r_size = this.originalRSize.Value + this.amplitude * (Mathf.Sin(this.currentAngle) + 1f);
    }
}

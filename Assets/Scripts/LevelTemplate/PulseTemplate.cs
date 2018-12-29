using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class PulseTemplate : SinWaveTemplate
{

    public PulseTemplate()
    {
        this.BackgroundColor = new Color(1, 0.77f, 0.7f);
        this.PlatformColor = new Color(0.9f, 0.1f, 0.1f);
        this.CircleColor = new Color(1f, 0.7f, 0.6f);
        this.amplitude = 0.2f;
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        // Implement a sinusoidal variation in Rpos. Do this by altering Rpos directly rather than rvel. This way,
        // The rpos of the platform becomes a straightforward superposition of a sine wave plus any other effects (e.g. standard drop).
        Platform platform = allPlatforms[platformIndex];
        platform.r_pos += this.SignedSinValue;
    }

    public override string Word
    {
        get
        {
            return "PULSE";
        }
    }
}

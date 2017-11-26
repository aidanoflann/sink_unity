using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal class TickTemplate : LevelTemplate
{
    private float timeSinceLastTick;
    private static float tickDuration = 2.5f; // time during which the tick is actually happening
    private static float tickPeriod = 5f; // how often the tick occurs
    private static float wVelocityMaxMultiplier = 200f;
    private static float wVelocityMinMultiplier = 0.00f;
    private List<float> platformVelocitySigns = new List<float>();
    private List<bool> platformTickingStates = new List<bool>();

    public TickTemplate()
    {
        this.BackgroundColor = new Color(0.8f, 0.48f, 0.2f);
        this.PlatformColor = new Color(0.95f, 0.88f, 0.16f);
        this.CircleColor = new Color(0.9f, 0.58f, 0.3f);
    }

    public override void Reload()
    {
        this.timeSinceLastTick = 0f;
        this.platformTickingStates = new List<bool>();
        this.platformVelocitySigns = new List<float>();
    }

    public override void SetPlatformParameters(Platform platform, int platformIndex, int numPlatforms)
    {
        // Set up the velocity sign and isTicking trackers
        platformVelocitySigns.Add(Mathf.Sign(platform.w_vel.GetValue()));
        platformTickingStates.Add(false);
        // Remove the constant background speed from all platforms
        platform.w_vel *= wVelocityMinMultiplier;
    }

    public override void UpdatePlatformPosition(int platformIndex, List<Platform> allPlatforms, float rSpeedMultiplier)
    {
        // in case the game has removed a platform, remove the corresponding trackers
        if (allPlatforms.Count < this.platformTickingStates.Count)
        {
            this.platformVelocitySigns.RemoveAt(0);
            this.platformTickingStates.RemoveAt(0);
        }
        Platform platform = allPlatforms[platformIndex];

        // modulo the time since last tick by the total period
        this.timeSinceLastTick = (this.timeSinceLastTick + Time.deltaTime) % tickPeriod;
        // if the platform is not ticking but the time falls within the tick duration, start ticking
        bool isTicking = platformTickingStates[platformIndex];
        if (this.timeSinceLastTick < tickDuration && !isTicking)
        {
            platform.w_vel.SetValue(wVelocityMaxMultiplier * this.platformVelocitySigns[platformIndex]);
            platformTickingStates[platformIndex] = true;
        }
        // if the platform is ticking but the time no longer falls within the tick duration, stop ticking
        if (this.timeSinceLastTick > tickDuration && isTicking)
        {
            platform.w_vel.SetValue(wVelocityMinMultiplier * this.platformVelocitySigns[platformIndex]);
            platformTickingStates[platformIndex] = false;
        }
    }
}



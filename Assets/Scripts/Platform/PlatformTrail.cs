using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Utils;
using System.Linq;

public class PlatformTrail : AnnulusShapedObject
{
    private float timeSinceLastTailUpdate;
    private static float trailUpdateCoolDown = 0.1f;
    Vector2[][] previousAnnulusPoints;
    private int previousAnnulusPointsIndex = 0;
    private static int totalNumPreviousPoints = 20;

    // Use this for initialization
    void Awake() {
        // TODO: change to specific trail material
        this.material = Resources.Load<Material>("PlatformMaterial");
        this.meshFilter = GetComponent<MeshFilter>();
    }

    public void UpdateTrail(Vector2[] annulusPoints)
    {
        // if the set duration of refreshing the trail hasn't been passed, return
        if (Time.time - this.timeSinceLastTailUpdate < trailUpdateCoolDown)
        {
            return;
        }

        // Don't start the trail until a full array of previous points have been generated
        int numAnnulusPoints = annulusPoints.Length;
        this.previousAnnulusPoints[previousAnnulusPointsIndex] = annulusPoints;
        int nextIndex = (this.previousAnnulusPointsIndex + 1) % totalNumPreviousPoints;
        if(this.previousAnnulusPoints[nextIndex] == null)
        {
            this.previousAnnulusPointsIndex = nextIndex;
            return;
        }
        
        // reset the last refresh time and trigger the actual trail recalculation
        this.timeSinceLastTailUpdate = Time.time;
        // first half
        Vector2[] firstHalf = this.previousAnnulusPoints[this.previousAnnulusPointsIndex].Take(numAnnulusPoints / 2).ToArray();
        Vector2[] secondHalf = this.previousAnnulusPoints[nextIndex].Skip(numAnnulusPoints / 2).ToArray();
        Vector2[] trailPoints = (firstHalf).Concat(secondHalf).ToArray();
        this.updateMesh(trailPoints);
        this.previousAnnulusPointsIndex = nextIndex;
    }

    public void SetPlatform(Vector2[] initialAnnulusPoints)
    {
        this.createMesh(initialAnnulusPoints, this.material);
        this.previousAnnulusPoints = new Vector2[initialAnnulusPoints.Length][];
    }
}

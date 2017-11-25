using UnityEngine;

public class Platform : AnnulusShapedObject {
    public GameObject platformTrailPrefab;
    private PlatformTrail platformTrail;

    public bool hasPlayer;
    public bool hadPlayer;

    // Use this for initialization
    void Start () {
		//static attributes
		r_size = 0.6f;

		this.CalculateAnnulusPoints();
        this.material = Resources.Load<Material>("PlatformMaterial");

        createMesh (this.annulusPoints, this.material);

        // generate the tail gameobject
        GameObject toInstantiate = platformTrailPrefab;
        GameObject instance = Instantiate(toInstantiate) as GameObject;
        // grab the behaviour
        this.platformTrail = instance.GetComponent<PlatformTrail>();
        this.platformTrail.SetColour(new Color(this.currentColour.r, this.currentColour.g, this.currentColour.b, 0.3f));
        this.platformTrail.SetPlatform(this.annulusPoints);
        // set self as parent transform
        instance.transform.SetParent(this.transform);
    }

    public void RecalculateMesh() {
        // update annulus based on new points
        this.CalculateAnnulusPoints();
        this.updateMesh(this.annulusPoints);
        this.platformTrail.UpdateTrail(this.annulusPoints);
    }

    public void CatchPlayer(Player player)
    {
        this.SetColour(player.CurrentColour);
        this.hasPlayer = true;
        this.hadPlayer = true;
    }

    public void ReleasePlayer()
    {
        this.ResetColour();
        this.hasPlayer = false;
    }
}

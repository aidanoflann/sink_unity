using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	// prepare the player and platform prefabs
	public GameObject playerPrefab;
	public GameObject platformPrefab;

    // cached platforms
    private List<GameObject> platformList;

	// this is used to child all of the gameObjects for better control/organisation in the inspector.
	private Transform levelHolder;

	private void SpawnPlatforms(int numPlatforms)
	{
		bool clockwise = true;
		for (int p=0; p < numPlatforms; p++) {
			
			GameObject toInstantiate = platformPrefab;
			GameObject instance = Instantiate (toInstantiate) as GameObject;

			// grab the script
			Platform platform = instance.GetComponent<Platform> ();

			// TODO: Find a way to get these into an init function - doing so as normal changes the values of the prefab, not the instance
			platform.w_size = 180f;
			platform.r_pos = 2f * (float)(p + 1);
			platform.r_vel = -1f;
			platform.w_pos = 0f;
			if (clockwise)
				platform.w_vel = 100f;
			else
				platform.w_vel = -100f;
			clockwise = !clockwise;

			instance.transform.SetParent (levelHolder);
            platformList.Add(instance);
        }
	}

	private void SpawnPlayer()
	{
		GameObject toInstantiate = playerPrefab;
		GameObject instance = Instantiate (toInstantiate);
		instance.transform.SetParent (levelHolder);
	}

	public void SetupScene(int numPlatforms)
	{
		levelHolder = new GameObject ("Level").transform;
        platformList = new List<GameObject>(numPlatforms);

        SpawnPlatforms (numPlatforms);
		SpawnPlayer ();
	}

    public void Update()
    {
        // despawn platforms as required
        for (int i = 0; i < platformList.Count; i++)
        {
            GameObject platformObject = platformList[i];
            Platform platform = platformObject.GetComponent<Platform>();
            if (platform.r_pos <= 0)
            {
                Destroy(platformObject);
                platformList.Remove(platformObject);
            }
        }
    }
}

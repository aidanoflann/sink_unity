using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManagerBehaviour : MonoBehaviour {

    private GameObject statManager;
    private List<LevelStats> allLevelStats;

    private float startTime;

    private class LevelStats
    {
        private float levelDuration; // seconds
        private int numPlatforms;
        private int numDynamicTemplates; // can be 0 (first level)

        public LevelStats(float levelDuration, int numPlatforms, int numDynamicTemplates)
        {
            this.levelDuration = levelDuration;
            this.numPlatforms = numPlatforms;
            this.numDynamicTemplates = numDynamicTemplates;
        }
    }

	void Awake () {
        statManager = gameObject;
        DontDestroyOnLoad(statManager);
	}
	
    public void SetStartTime()
    {
        this.startTime = Time.time; // seconds?
    }

	public void UpdateStatsOnLevelEnd(List<LevelTemplate> baseLevelTemplates, List<LevelTemplate> dynamicLevelTemplates, int numPlatforms)
    {
        float levelDuration = Time.time - this.startTime;
        this.allLevelStats.Add(new LevelStats(levelDuration, numPlatforms, dynamicLevelTemplates.Count));
    }
}

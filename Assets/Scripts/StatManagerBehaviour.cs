using Assets.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManagerBehaviour : SingletonBehaviour {

    private GameObject statManager;
    private List<LevelStats> allLevelStats = new List<LevelStats>();

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

        public int CalculateScore()
        {
            // TODO: actually care about this equation.
            return (int)Mathf.Round(Mathf.Pow(this.numPlatforms, (1 + this.numDynamicTemplates)) / (this.levelDuration / 60f));
        }

        public void Log()
        {
            Debug.LogFormat("levelDuration: {0}, numPlatforms: {1}, numDynamicTemplates: {2}",
                this.levelDuration, this.numPlatforms, this.numDynamicTemplates);
        }
    }
	
    public void SetStartTime()
    {
        this.startTime = Time.time; // seconds?
    }

	public void UpdateStatsOnLevelEnd(int numBaseTemplates, List<LevelTemplate> levelTemplates, int numPlatforms)
    {
        float levelDuration = Time.time - this.startTime;
        int numDynamicTemplates = levelTemplates.Count - numBaseTemplates;
        this.allLevelStats.Add(new LevelStats(levelDuration, numPlatforms, numDynamicTemplates));
    }

    public int GetTotalScore()
    {
        int totalScore = 0;
        for (int i = 0; i < this.allLevelStats.Count; i++)
        {
            totalScore += allLevelStats[i].CalculateScore();
        }
        return totalScore;
    }

    public void Reset()
    {
        this.startTime = 0f;
        this.allLevelStats.Clear();
    }

    public void Log()
    {
        Debug.Log("--- LOGGING STAT MANAGER ---");
        Debug.LogFormat("number of levels: {0}", this.allLevelStats.Count);
        for (int i = 0; i < this.allLevelStats.Count; i++)
        {
            Debug.LogFormat("--LevelStats {0}--", i);
            allLevelStats[i].Log();
            Debug.LogFormat("--Score for level: {0} --", allLevelStats[i].CalculateScore());
        }
        Debug.Log("------");
    }
}

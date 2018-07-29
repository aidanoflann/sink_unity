using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Utils;
using Assets.Scripts;

public class EndGameBehaviour : MonoBehaviour {

    private StatManagerBehaviour statManagerBehaviour;
    private RandomNumberManager randomNumberManager;
    private Text scoreNumberText;
    private Text seedNumberText;

	// Use this for initialization
	void Start ()
    {
        // Set the score number based on the statmanager
        this.statManagerBehaviour = SingletonBehaviour.GetSingletonBehaviour<StatManagerBehaviour>();
        GameObject scoreNumberTextGO = GameObject.Find("ScoreNumber");
        this.scoreNumberText = scoreNumberTextGO.GetComponent<Text>();
        this.scoreNumberText.text = this.statManagerBehaviour.GetTotalScore().ToString();

        // Set the seed based on the randomnumbermanager
        this.randomNumberManager = SingletonBehaviour.GetSingletonBehaviour<RandomNumberManager>();
        GameObject seedNumberTextGO = GameObject.Find("SeedNumber");
        this.seedNumberText = seedNumberTextGO.GetComponent<Text>();
        this.seedNumberText.text = this.randomNumberManager.Seed.ToString();
    }

    public void StartLevel()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}

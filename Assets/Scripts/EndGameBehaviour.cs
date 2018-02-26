using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameBehaviour : MonoBehaviour {

    private StatManagerBehaviour statManagerBehaviour;
    private Text scoreNumberText;

	// Use this for initialization
	void Start () {
        this.statManagerBehaviour = FindObjectOfType<StatManagerBehaviour>();
        GameObject scoreNumberTextGO = GameObject.Find("ScoreNumber");
        this.scoreNumberText = scoreNumberTextGO.GetComponent<Text>();
        this.scoreNumberText.text = this.statManagerBehaviour.GetTotalScore().ToString();
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

using Assets.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Menus
{
    public class PauseMenuBehaviour : MonoBehaviour
    {
        public Text ScoreText;
        public Text SeedText;

        private RandomNumberManager randomNumberManager;
        private StatManagerBehaviour statManagerBehaviour;

        public void Awake()
        {
            this.randomNumberManager = SingletonBehaviour.GetSingletonBehaviour<RandomNumberManager>();
            this.statManagerBehaviour = SingletonBehaviour.GetSingletonBehaviour<StatManagerBehaviour>();
        }

        public void UpdateData()
        {
            this.ScoreText.text = "SCORE: " + this.statManagerBehaviour.GetTotalScore();
            this.SeedText.text = "SEED: " + this.randomNumberManager.Seed;
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("StartMenu");
        }

        public void Restart()
        {
            this.randomNumberManager.Reset(true);
            SceneManager.LoadScene("MainLevel");
        }

        public void RetrySeed()
        {
            this.randomNumberManager.Reset(false);
            SceneManager.LoadScene("MainLevel");
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}

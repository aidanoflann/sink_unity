using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    public Canvas howToPlayMenu;
    public Canvas startMenu;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        howToPlayMenu = howToPlayMenu.GetComponent<Canvas>();
        howToPlayMenu.enabled = false;
    }

    public void HowToPlayButtonPress()
    {
        startMenu.enabled = false;
        howToPlayMenu.enabled = true;
    }

    public void HowToPlayOkButtonPress()
    {
        howToPlayMenu.enabled = false;
        startMenu.enabled = true;
    }

    public void StartLevel()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

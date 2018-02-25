using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    public GameObject howToPlayMenu;
    public Canvas startMenu;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        //howToPlayMenu = GameObject.FindGameObjectWithTag("HowToPlay");
        howToPlayMenu.SetActive(false);
    }

    public void HowToPlayButtonPress()
    {
        startMenu.enabled = false;
        howToPlayMenu.SetActive(true);
    }

    public void HowToPlayOkButtonPress()
    {
        howToPlayMenu.SetActive(false);
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

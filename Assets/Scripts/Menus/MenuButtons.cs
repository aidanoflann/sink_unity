using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    public GameObject howToPlayMenu;
    public GameObject optionsMenu;
    public Canvas startMenu;
    public AudioManager audioManager;

    void Start()
    {
        //howToPlayMenu = GameObject.FindGameObjectWithTag("HowToPlay");
        howToPlayMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    public void HowToPlayButtonPress()
    {
        startMenu.enabled = false;
        howToPlayMenu.SetActive(true);
    }

    public void OptionsButtonPress()
    {
        startMenu.enabled = false;
        optionsMenu.SetActive(true);
    }

    public void OptionButtonsOKPress()
    {
        optionsMenu.SetActive(false);
        startMenu.enabled = true;
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

    public void EnableSound()
    {
        this.audioManager.soundEnabled = true;
    }
    
    public void DisableSound()
    {
        this.audioManager.soundEnabled = false;
    }
}

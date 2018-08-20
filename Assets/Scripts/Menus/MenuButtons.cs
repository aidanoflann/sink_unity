using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    public GameObject howToPlayMenu;
    public GameObject optionsMenu;
    public Canvas startMenu;
    public AudioManager audioManager;
    public Text audioText;

    void Start()
    {
        //howToPlayMenu = GameObject.FindGameObjectWithTag("HowToPlay");
        howToPlayMenu.SetActive(false);
        optionsMenu.SetActive(false);

        if (this.audioManager.SoundEnabled)
        {
            this.audioText.text = "SOUND: ON";
        }
        else
        {
            this.audioText.text = "SOUND: OFF";
        }
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

    public void ToggleSound()
    {
        if(this.audioManager.SoundEnabled)
        {
            this.audioText.text = "SOUND: OFF";
            PlayerPrefs.SetInt("SoundEnabled", 0);
        }
        else
        {
            this.audioText.text = "SOUND: ON";
            PlayerPrefs.SetInt("SoundEnabled", 1);
        }
    }
}

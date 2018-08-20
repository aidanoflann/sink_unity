using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    public GameObject howToPlayMenu;
    public GameObject optionsMenu;
    public Canvas startMenu;
    public AudioManager audioManager;

    public GameObject SeedGameObject;

    public Text audioText;
    public Text shakeText;
    public Text seedText;

    private ToggleableButton audioButton; // if on, sound is enabled
    private ToggleableButton shakeButton; // if on, screenshake is enabled
    private ToggleableButton seedButton; // if on, seed option is visible

    void Start()
    {
        //howToPlayMenu = GameObject.FindGameObjectWithTag("HowToPlay");
        howToPlayMenu.SetActive(false);
        optionsMenu.SetActive(false);
        this.audioButton = new ToggleableButton("SOUND: ON", "SOUND: OFF", "SoundEnabled", this.audioText);
        this.shakeButton = new ToggleableButton("SHAKE: ON", "SHAKE: OFF", "ShakeEnabled", this.shakeText);
        this.seedButton = new ToggleableButton("SEED: ON", "SEED: OFF", "SeedEnabled", this.seedText);

        this.SeedGameObject.SetActive(PlayerPrefs.GetInt("SeedEnabled") == 1);
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
        this.audioButton.Toggle();
    }

    public void ToggleShake()
    {
        this.shakeButton.Toggle();
    }

    public void ToggleSeed()
    {
        this.seedButton.Toggle();
        this.SeedGameObject.SetActive(PlayerPrefs.GetInt("SeedEnabled") == 1);
    }
}

public class ToggleableButton
{
    private string toggledOnText;
    private string toggledOffText;
    private string playerPrefKey;
    private Text textUI;

    public ToggleableButton(string toggledOnText, string toggledOffText, string playerPrefKey, Text textUI)
    {
        this.toggledOnText = toggledOnText;
        this.toggledOffText = toggledOffText;
        this.playerPrefKey = playerPrefKey;
        this.textUI = textUI;

        // assume default is on
        if(this.IsOn)
        {
            this.textUI.text = this.toggledOnText;
        }
        else
        {
            this.textUI.text = this.toggledOffText;
        }
    }

    public bool IsOn
    {
        get
        {
            return PlayerPrefs.GetInt(this.playerPrefKey) == 1;
        }
    }

    public void Toggle()
    {
        if (this.IsOn)
        {
            this.ToggleOff();
        }
        else
        {
            this.ToggleOn();
        }
    }

    public void ToggleOn()
    {
        this.textUI.text = toggledOnText;
        PlayerPrefs.SetInt(this.playerPrefKey, 1);
    }

    public void ToggleOff()
    {
        this.textUI.text = this.toggledOffText;
        PlayerPrefs.SetInt(this.playerPrefKey, 0);
    }
}


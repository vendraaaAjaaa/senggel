using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MainMenu : MonoBehaviour
{
    public int _selectedButton = 0;
    public float _timeBetweenButtonPress = 0.1f;
    private float _timeDelay;

    public Texture2D _mainMenuBackground;
    public Texture2D _mainMenuTitle;

    private AudioSource _mainMenuAudio;
    public AudioClip _mainMenuMusic;
    public AudioClip _mainMenuStartButtonAudio;
    public AudioClip _mainMenuQuitButtonAudio;

    public float _mainMenuFadeValue;
    public float _mainMenuFadeSpeed = 0.35f;

    public float _mainMenuButtonWidth = 100f;
    public float _mainMenuButtonHeight = 25f;
    public float _mainMenuGUIOffset = 10f;

    private bool _startingOnePlayerGame;
    private bool _startingTwoPlayerGame;
    private bool _quittingGame;
    
    private bool _ps4Controller;
    private bool _xBOXController;
    
    private string[] _mainMenuButtons = new string[] {
        "One Player",
        "Two Player",
        "Quit"
    };

    private MainMenuController _mainMenuController;

    private enum MainMenuController{
        MainMenuFadeIn = 0,
        MainMenuAtIdle = 1,
        MainMenuFadeOut = 2
    };

    void Start()
    {
        _startingOnePlayerGame = false;
        _startingTwoPlayerGame = false;
        _quittingGame = false;
        
        _ps4Controller = false;
        _xBOXController = false;

        _mainMenuFadeValue = 0;

        _mainMenuAudio = GetComponent<AudioSource>();

        _mainMenuAudio.volume = 0;
        _mainMenuAudio.clip = _mainMenuMusic;
        _mainMenuAudio.loop = true;
        _mainMenuAudio.Play();

        _mainMenuController = MainMenu.MainMenuController.MainMenuFadeIn;

        StartCoroutine("MainMenuManager");
    }

    void Update()
    {
        if(_mainMenuFadeValue < 1)
            return;
            
        string[] _joyStickNames = Input.GetJoystickNames();
        for (int _js = 0; _js < _joyStickNames.Length; _js++)
        {
            if (_joyStickNames[_js].Length == 19)
                _ps4Controller = true;
            if (_joyStickNames[_js].Length == 33)
                _xBOXController = true;
        }

        // Reduce the time delay between button presses
        _timeDelay -= Time.deltaTime;

        // Handle vertical input for changing selected button
        float verticalInput = Input.GetAxisRaw("Vertical");
        if (verticalInput > 0 && _timeDelay <= 0)
        {
            _selectedButton = (_selectedButton - 1 + _mainMenuButtons.Length) % _mainMenuButtons.Length;
            _timeDelay = _timeBetweenButtonPress;
        }
        else if (verticalInput < 0 && _timeDelay <= 0)
        {
            _selectedButton = (_selectedButton + 1) % _mainMenuButtons.Length;
            _timeDelay = _timeBetweenButtonPress;
        }

        // Handle button press
        if (Input.GetButtonDown("Fire1"))
        {
            MainMenuButtonPress();
        }
    }

    private IEnumerator MainMenuManager()
    {
        while (true)
        {
            switch(_mainMenuController){
                case MainMenuController.MainMenuFadeIn:
                    MainMenuFadeIn();
                    break;

                case MainMenuController.MainMenuAtIdle:
                    MainMenuAtIdle();
                    break;

                case MainMenuController.MainMenuFadeOut:
                    MainMenuFadeOut();
                    break;
            }
            yield return null;
        }
    }

    private void MainMenuFadeIn()
    {
        Debug.Log("MainMenuFadeIn");

        _mainMenuAudio.volume += _mainMenuFadeSpeed * Time.deltaTime;

        _mainMenuFadeValue += _mainMenuFadeSpeed * Time.deltaTime;

        if (_mainMenuFadeValue > 1)
        {
            _mainMenuFadeValue = 1;
        }

        if (_mainMenuFadeValue == 1)
        {
            _mainMenuController = MainMenu.MainMenuController.MainMenuAtIdle;
        }
    }

    private void MainMenuAtIdle()
    {
        Debug.Log("MainMenuAtIdle");

        if (_startingOnePlayerGame || _quittingGame == true)
        {
            _mainMenuController = MainMenu.MainMenuController.MainMenuFadeOut;
        }
    }

    private void MainMenuFadeOut()
    {
        Debug.Log("MainMenuFadeOut");

        _mainMenuAudio.volume -= _mainMenuFadeSpeed * Time.deltaTime;

        _mainMenuFadeValue -= _mainMenuFadeSpeed * Time.deltaTime;

        if (_mainMenuFadeValue < 0)
        {
            _mainMenuFadeValue = 0;
        }
        
        if (_mainMenuFadeValue == 0 && _startingOnePlayerGame == true)
        {
            SceneManager.LoadScene("ChooseCharacter");
        }
    }

    private void MainMenuButtonPress()
    {
        Debug.Log("MainMenuButtonPress");

        switch (_selectedButton)
        {
            case 0:
                _mainMenuAudio.PlayOneShot(_mainMenuStartButtonAudio);
                _startingOnePlayerGame = true;
                GameObject.FindGameObjectWithTag("OnePlayerManager").GetComponent<OnePlayerManager>().enabled = true;
                break;

            case 1:
                _mainMenuAudio.PlayOneShot(_mainMenuStartButtonAudio);
                _startingTwoPlayerGame = true;
                GameObject.FindGameObjectWithTag("TwoPlayerManager").GetComponent<TwoPlayerManager>().enabled = true;
                break;


            case 2:
                _mainMenuAudio.PlayOneShot(_mainMenuQuitButtonAudio);
                _quittingGame = true;
                break;
        }
    }

    void OnGUI()
    {
        // Draw background
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _mainMenuBackground);

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _mainMenuTitle);

        // Set GUI color
        GUI.color = new Color(1, 1, 1, _mainMenuFadeValue);

        // Begin group for buttons
        GUI.BeginGroup(new Rect(
            Screen.width / 2 - _mainMenuButtonWidth / 2,
            Screen.height / 1.5f,
            _mainMenuButtonWidth,
            _mainMenuButtonHeight * 3 + _mainMenuGUIOffset * 2));

        // Iterate through buttons and draw them
        for (int i = 0; i < _mainMenuButtons.Length; i++)
        {
            // Check if current button is selected
            bool isSelected = (_selectedButton == i);

            // Set button style based on selection
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            if (isSelected)
            {
                buttonStyle.normal.textColor = Color.red; // Change color for selected button
            }

            // Draw button
            if (GUI.Button(new Rect(
                0, i * (_mainMenuButtonHeight + _mainMenuGUIOffset),
                _mainMenuButtonWidth, _mainMenuButtonHeight),
                _mainMenuButtons[i], buttonStyle))
            {
                _selectedButton = i;
                MainMenuButtonPress();
            }
        }

        // End group for buttons
        GUI.EndGroup();

        // Focus control on selected button if using controller
        if (_ps4Controller || _xBOXController)
            GUI.FocusControl(_mainMenuButtons[_selectedButton]);
    }

}

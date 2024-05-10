using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class ChooseCharacter : ChooseCharacterManager
{
    public Texture2D _selectCharacterTextBackground;
    public Texture2D _selectCharacterTextForeground;
    public Texture2D _selectCharacterText;

    public Texture2D _selectCharacterXboxLeft;
    public Texture2D _selectCharacterXboxRight;
    public Texture2D _selectCharacterPS4Left;
    public Texture2D _selectCharacterPS4Right;

    private float _foregroundTextWidth;
    private float _foregroundTextHeight;
    private float _leftRightControllerIconSize;
    
    public float _chooseCharacterInputTimer;
    public float _chooseCharacterInputDelay = 1f;

    public AudioClip _cycleCharacterButtonPress;

    private GameObject _characterDemo;
    public static bool _demoPlayer;

    public int _characterSelectState;
    public int _yRot = 180;

    private enum CharacterSelectModels{
        BlackRobot = 0,
        WhiteRobot = 1,
        RedRobot = 2,
        BlueRobot = 3,
    }
    void Start()
    {
        CharacterSelectManager();

        _demoPlayer = true;

        _foregroundTextWidth = Screen.width / 1.5f;
        _foregroundTextHeight = Screen.height / 10f;
        _leftRightControllerIconSize = Screen.height / 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")){
            _demoPlayer = false;
            GameObject.FindGameObjectWithTag("BackgroundManager").
            GetComponent<BackgroundManager>().SendMessage("SceneBackgroundLoad");
        }
            

        if (_chooseCharacterInputTimer > 0)
            _chooseCharacterInputTimer -= 1f * Time.deltaTime;

        if (_chooseCharacterInputTimer > 0)
            return;

        if (Input.GetAxis("Horizontal") < -0.5f)
        {
            if(_characterSelectState == 0)
                return;

            GetComponent<AudioSource>().PlayOneShot(_cycleCharacterButtonPress);

            _characterSelectState--;
            CharacterSelectManager();

            _chooseCharacterInputTimer = _chooseCharacterInputDelay;
        }

        if (Input.GetAxis("Horizontal") > 0.5f)
        {
            if(_characterSelectState == 3)
                return;

            _characterSelectState++;
            CharacterSelectManager();

            _chooseCharacterInputTimer = _chooseCharacterInputDelay;
        }
    }

    private void CharacterSelectManager()
    {
        switch (_characterSelectState)
        {
            default:
            case 0:
                BlackRobot();
                break;

            case 1:
                WhiteRobot();
                break;

            case 2:
                RedRobot();
                break;

            case 3:
                BlueRobot();
                break;
        }
    }

    private void BlackRobot()
    {
        Debug.Log("BlackRobot");

        DestroyObject (_characterDemo);

        _characterDemo = Instantiate (Resources.Load("BlackRobot"))
            as GameObject;

        _characterDemo.transform.position = new Vector3 (-0.5f, 0,-7);

        _characterDemo.transform.eulerAngles = new Vector3(0,_yRot, 0);

        _robotBlack = true;
        _robotWhite = false;
        _robotRed = false;
        _robotBlue = false;
    }

    private void WhiteRobot()
    {
        Debug.Log("WhiteRobot");

        DestroyObject (_characterDemo);

        _characterDemo = Instantiate (Resources.Load("WhiteRobot"))
            as GameObject;

        _characterDemo.transform.position = new Vector3 (-0.5f, 0,-7);

        _characterDemo.transform.eulerAngles = new Vector3(0,_yRot, 0);

        _robotBlack = false;
        _robotWhite = true;
        _robotRed = false;
        _robotBlue = false;
    }

    private void RedRobot()
    {
        Debug.Log("RedRobot");

        DestroyObject (_characterDemo);

        _characterDemo = Instantiate (Resources.Load("RedRobot"))
            as GameObject;

        _characterDemo.transform.position = new Vector3 (-0.5f, 0,-7);

        _characterDemo.transform.eulerAngles = new Vector3(0,_yRot, 0);

        _robotBlack = false;
        _robotWhite = false;
        _robotRed = true;
        _robotBlue = false;
    }

    private void BlueRobot()
    {
        Debug.Log("BlueRobot");

        DestroyObject (_characterDemo);

        _characterDemo = Instantiate (Resources.Load("BlueRobot"))
            as GameObject;

        _characterDemo.transform.position = new Vector3 (-0.5f, 0,-7);

        _characterDemo.transform.eulerAngles = new Vector3(0,_yRot, 0);

        _robotBlack = false;
        _robotWhite = false;
        _robotRed = false;
        _robotBlue = true;
    }

    void OnGUI()
    {
        GUI.DrawTexture (new Rect (
            0, 0,
            Screen.width, Screen.height / 10),
            _selectCharacterTextBackground);

        GUI.DrawTexture (new Rect (
            Screen.width / 2 - (_foregroundTextWidth / 2),
            0,
            _foregroundTextWidth,_foregroundTextHeight),
            _selectCharacterTextForeground);

        GUI.DrawTexture (new Rect (
            Screen.width / 2 - (_foregroundTextWidth / 2),
            0,
            _foregroundTextWidth, _foregroundTextHeight),
            _selectCharacterText);

        if (GameObject.FindGameObjectWithTag("ControllerManager").GetComponent<ControllerManager>()._xBOXController == true)
        {
            GUI.DrawTexture (new Rect (
                Screen.width / 2 - (_foregroundTextWidth / 2) -
                _leftRightControllerIconSize,
                0,
                _leftRightControllerIconSize, _leftRightControllerIconSize),
                _selectCharacterXboxLeft);

            GUI.DrawTexture (new Rect (
                Screen.width / 2 + (_foregroundTextWidth / 2),
                0,
                _leftRightControllerIconSize, _leftRightControllerIconSize),
                _selectCharacterXboxRight);
        }

        if (GameObject.FindGameObjectWithTag("ControllerManager").GetComponent<ControllerManager>()._pS4Controller == true)
        {
            GUI.DrawTexture (new Rect (
                Screen.width / 2 - (_foregroundTextWidth / 2) -
                _leftRightControllerIconSize,
                0,
                _leftRightControllerIconSize, _leftRightControllerIconSize),
                _selectCharacterPS4Left);

            GUI.DrawTexture (new Rect (
                Screen.width / 2 + (_foregroundTextWidth / 2),
                0,
                _leftRightControllerIconSize, _leftRightControllerIconSize),
                _selectCharacterPS4Right);
        }
        
    }
}

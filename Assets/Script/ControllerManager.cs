using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ControllerManager : MonoBehaviour
{
    public Texture2D _controllerNotDetected;

    public bool _pS4Controller;
    public bool _xBOXController;
    public bool _controllerDetected;

    public static bool _startUpFinished;

    private AudioSource _cmAudio;
    public AudioClip _controllerDetectedAudioClip;

    void Awake()
    {
        _pS4Controller = false;
        _xBOXController = false;
        _controllerDetected = false;

        _startUpFinished = false;
    }
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (_controllerDetected == true)
            return;

        if (_startUpFinished == true)
            Time.timeScale = 0;
    }

    void LateUpdate()
    {
        if (_startUpFinished == true)
            _cmAudio = GetComponent<AudioSource>();
        
        string[] _joyStickNames = Input.GetJoystickNames();

        _controllerDetected = false; // Set ulang nilai _controllerDetected

        foreach (string joyStickName in _joyStickNames)
        {
            if (joyStickName.Length == 19)
                _pS4Controller = true;
            else if (joyStickName.Length == 33)
                _xBOXController = true;

            if (!string.IsNullOrEmpty(joyStickName))
                _controllerDetected = true; // Controller terdeteksi jika ada nama yang tidak kosong

            if (_startUpFinished == true)
                _cmAudio.PlayOneShot(_controllerDetectedAudioClip);
            Time.timeScale = 1;

            _controllerDetected = true;
        }
    }

    private void OnGUI()
    {
        if (_startUpFinished == false)
            return;

        if (_controllerDetected == true)
            return;

        if (_controllerDetected == false)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height),_controllerNotDetected);
    }
}

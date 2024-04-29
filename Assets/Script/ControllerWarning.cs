using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerWarning : ControllerManager
{
    public Texture2D _controllerWarningBackground;
    public Texture2D _controllerWarningText;
    public Texture2D _controllerDetectedText;

    public float _controllerWarningFadeValue;
    private float _controllerWarningFadeSpeed = 0.25f;
    private bool _controllerConditionsMet;

    void Start()
    {
        _controllerWarningFadeValue = 1;
        _controllerConditionsMet = false;
    }

    void Update()
    {
        // Jika controller terdeteksi, panggil fungsi WaitToLoadMainMenu
        if (_controllerDetected && !_controllerConditionsMet)
            StartCoroutine("WaitToLoadMainMenu");

        // Kondisi-kondisi untuk mengatur fading out
        if (_controllerConditionsMet)
        {
            _controllerWarningFadeValue -= _controllerWarningFadeSpeed * Time.deltaTime;

            if (_controllerWarningFadeValue <= 0)
            {
                _controllerWarningFadeValue = 0;
                _startUpFinished = true;
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    IEnumerator WaitToLoadMainMenu()
    {
        yield return new WaitForSeconds(2);

        _controllerConditionsMet = true;
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _controllerWarningBackground);

        GUI.color = new Color(1, 1, 1, _controllerWarningFadeValue);

        GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), _controllerWarningText);

        if (_controllerDetected == true)
            GUI.DrawTexture (new Rect(0, 0, Screen.width, Screen.height), _controllerDetectedText);
    }
}

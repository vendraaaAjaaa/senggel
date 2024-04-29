using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundManager : MonoBehaviour
{
    public string _selectedBackground = "";
    public int _backgroundCounter;

    public string[] _backgroundScenes = new string[] {
        "Scene0",
        "Scene1",
        "Scene2",
        "Scene3"
    };
    void Start()
    {
        DontDestroyOnLoad(this);

        _backgroundCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SceneBackgroundManager()
    {
        Debug.Log("SceneBackgroundManager");

        if (_backgroundCounter < _backgroundScenes.Length)
            _backgroundCounter++;

        if (_backgroundCounter == _backgroundScenes.Length)
            _backgroundCounter = 0;

        _selectedBackground =
            _backgroundScenes [_backgroundCounter];
    }

    private void SceneBackgroundLoad()
    {
        Debug.Log("SceneBackgroundLoad");

        SceneBackgroundManager();

        SceneManager.LoadScene(_selectedBackground);
    }
}

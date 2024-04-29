using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnePlayerManager : MonoBehaviour
{
    private GameObject _playerOneCharacter;

    private bool _returnRobotBlack;
    private bool _returnRobotWhite;
    private bool _returnRobotRed;
    private bool _returnRobotBlue;

    private int _yRot = 90;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadPlayerOneCharacter(){
        Debug.Log("LoadPlayerOneCharacter");

        if (_playerOneCharacter != null)
            return;

        _returnRobotBlack = ChooseCharacterManager._robotBlack;
        _returnRobotWhite = ChooseCharacterManager._robotWhite;
        _returnRobotRed = ChooseCharacterManager._robotRed;
        _returnRobotBlue = ChooseCharacterManager._robotBlue;

        if(_returnRobotBlack == true)
            _playerOneCharacter = Instantiate(Resources.Load("BlackRobot")) as GameObject;

        if(_returnRobotWhite == true)
            _playerOneCharacter = Instantiate(Resources.Load("WhiteRobot")) as GameObject;

        if(_returnRobotRed == true)
            _playerOneCharacter = Instantiate(Resources.Load("RedRobot")) as GameObject;

        if(_returnRobotBlue == true)
            _playerOneCharacter = Instantiate(Resources.Load("BlueRobot")) as GameObject;

        _playerOneCharacter.transform.position = new Vector3(-1, 0, -7);
        _playerOneCharacter.transform.eulerAngles = new Vector3(0, _yRot,0);
    }
}

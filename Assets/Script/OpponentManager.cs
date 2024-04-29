using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentManager : MonoBehaviour
{
    private bool _returnRobotBlack;
    private bool _returnRobotWhite;
    private bool _returnRobotRed;
    private bool _returnRobotBlue;
   
   public GameObject _currentOpponent;
    public string _selectedOpponent = "";

    public int _opponentCounter;
    private int _yRot = -90;

    public string[] _opponentOrder = new string[] {
        "BlackRobotOpponent",
        "WhiteRobotOpponent",
        "RedRobotOpponent",
        "BlueRobotOpponent"
    };


    void Start()
    {
        DontDestroyOnLoad(this);
        
        _opponentCounter = 0;

        _selectedOpponent = _opponentOrder[0];

        for(int op = 0; op < _opponentOrder.Length; op++){
            string _opTemp = _opponentOrder[op];
            int _randomOrder =
                Random.Range(op, _opponentOrder.Length);
            _opponentOrder[op] = _opponentOrder[_randomOrder];
            _opponentOrder[_randomOrder] = _opTemp;
        }

    /*    foreach (string op in _opponentOrder)
            print(op);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadCurrentOpponent(){
        
        _returnRobotBlack = ChooseCharacterManager._robotBlack;
        _returnRobotWhite = ChooseCharacterManager._robotWhite;
        _returnRobotRed = ChooseCharacterManager._robotRed;
        _returnRobotBlue = ChooseCharacterManager._robotBlue;
        
        if(_selectedOpponent == "BlackRobotOpponent" && _returnRobotBlack != true)
            _currentOpponent = Instantiate(Resources.Load("BlackRobot")) as GameObject;
        else if (_selectedOpponent == "BlackRobotOpponent" && _returnRobotBlack == true)
            _currentOpponent = Instantiate(Resources.Load("BlackRobotAlt")) as GameObject;

        if(_selectedOpponent == "WhiteRobotOpponent" && _returnRobotWhite != true)
            _currentOpponent = Instantiate(Resources.Load("WhiteRobot")) as GameObject;
        else if (_selectedOpponent == "WhiteRobotOpponent" && _returnRobotWhite == true)
            _currentOpponent = Instantiate(Resources.Load("WhiteRobotAlt")) as GameObject;

        if(_selectedOpponent == "RedRobotOpponent" && _returnRobotRed != true)
            _currentOpponent = Instantiate(Resources.Load("RedRobot")) as GameObject;
        else if (_selectedOpponent == "RedRobotOpponent" && _returnRobotRed == true)
            _currentOpponent = Instantiate(Resources.Load("RedRobotAlt")) as GameObject;

        if(_selectedOpponent == "BlueRobotOpponent" && _returnRobotBlue != true)
            _currentOpponent = Instantiate(Resources.Load("BlueRobot")) as GameObject;
        else if (_selectedOpponent == "BlueRobotOpponent" && _returnRobotBlue == true)
            _currentOpponent = Instantiate(Resources.Load("BlueRobotAlt")) as GameObject;
        
        _currentOpponent.transform.position = new Vector3(1, 0, -7);
        _currentOpponent.transform.eulerAngles = new Vector3(0, _yRot,0);
    }
}

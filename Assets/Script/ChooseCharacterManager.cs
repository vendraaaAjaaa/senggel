using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCharacterManager : MonoBehaviour
{
    public static bool _robotBlack;
    public static bool _robotWhite;
    public static bool _robotRed;
    public static bool _robotBlue;

    void Awake(){
        _robotBlack = false;
        _robotWhite = false;
        _robotRed = false;
        _robotBlue = false;
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

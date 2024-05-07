using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneMovement : MonoBehaviour
{
    
    private Transform _playerOneTransform;
    private CharacterController _playerController;

    public float _playerWalkSpeed = 1f;
    public float _playerRetreatSpeed = 0.75f;
    public float _playerJumpHeight = 5f;
    public float _playerJumpSpeed = 5f;
    public float _playerJumpHorizontal = 5f;
    private Animation _playerOneAnim;
    public AnimationClip _playerOneIdleAnim;
    public AnimationClip _playerOneWalkAnim;
    public AnimationClip _playerOneBlockAnim;
    public AnimationClip _playerOneJumpAnim;
    public AnimationClip _playerOneDemoAnim;
    public AnimationClip[] _playerAttackAnim;

    public float _controllerDeadZonePos = 0.1f;
    public  float _controllerDeadZoneNeg = -0.1f;

    public float _playersGravity = 20f;
    public float _playerGravityModifier = 5f;
    public float _playersSpeedYAxis;

    private bool _returnDemoState;
    private int _demoRotationValue = 75;

    private Vector3 _playerOneMoveDirection = Vector3.zero;
    private CollisionFlags _collisionFlags;

    private PlayerOneStates _playerOneStates = PlayerOneStates.PlayerOneIdle;


    private enum PlayerOneStates{
        PlayerOneIdle,
        PlayerWalkLeft,
        PlayerWalkRight,
        PlayerBlock,
        PlayerJump,
        PlayerJumpForwards,
        PlayerJumpBackwards,
        ComeDown,
        ComeDownForwards,
        ComeDownBackwards,
        PlayerJab,
        PlayerStraight,
        PlayerCross,
        PlayerHook,
        PlayerUppercut,
        WaitForAnimations,
        PlayerDemo
    }
    void Start()
    {
        _playerOneTransform = transform;
        _playerOneMoveDirection = Vector3.zero;
        _playersSpeedYAxis = 0;
        _playerController = GetComponent<CharacterController>();
        _playerOneAnim = GetComponent<Animation>();

        for (int a = 0; a < _playerAttackAnim.Length; a++)
            _playerOneAnim[_playerAttackAnim[a].name].wrapMode = WrapMode.Once;

        StartCoroutine(PlayerOneFSM());

        _returnDemoState = false;

        _returnDemoState = ChooseCharacter._demoPlayer;

        if (_returnDemoState == true)
            _playerOneStates = PlayerOneMovement.PlayerOneStates.PlayerDemo;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyGravity();

        for (int a = 0; a < _playerAttackAnim.Length; a++)
        {
            if (_playerOneAnim.IsPlaying(_playerAttackAnim[a].name))
                return;
        }

        if (PlayerIsGrounded()) 
        {
            HorizontalInputManager();
            AttackInputManager();
            StandartInputManager();
        }

        
    }

    private IEnumerator PlayerOneFSM()
    {
        while (true)
        {
            switch (_playerOneStates)
            {
                case PlayerOneStates.PlayerOneIdle:
                    PlayerOneIdle();
                    break;

                case PlayerOneStates.PlayerWalkLeft:
                    PlayerWalkLeft();
                    break;
                case PlayerOneStates.PlayerWalkRight:
                    PlayerWalkRight();
                    break;

                case PlayerOneStates.PlayerJump:
                    PlayerJump();
                    break;

                case PlayerOneStates.PlayerJumpForwards:
                    PlayerJumpForwards();
                    break;

                case PlayerOneStates.PlayerJumpBackwards:
                    PlayerJumpBackwards();
                    break;

                case PlayerOneStates.PlayerBlock:
                    PlayerBlock();
                    break;

                case PlayerOneStates.ComeDown:
                    ComeDown();
                    break;

                case PlayerOneStates.ComeDownForwards:
                    ComeDownForwards();
                    break;

                case PlayerOneStates.ComeDownBackwards:
                    ComeDown();
                    break;

                case PlayerOneStates.PlayerJab:
                    PlayerJab();
                    break;

                case PlayerOneStates.PlayerStraight:
                    PlayerStraight();
                    break;

                case PlayerOneStates.PlayerCross:
                    PlayerCross();
                    break;

                case PlayerOneStates.PlayerHook:
                    PlayerHook();
                    break;

                case PlayerOneStates.PlayerUppercut:
                    PlayerUppercut();
                    break;

                case PlayerOneStates.WaitForAnimations:
                    WaitForAnimations();
                    break;

                case PlayerOneStates.PlayerDemo:
                    PlayerDemo();
                    break;
            }
            yield return null; // Pause the coroutine to allow other operations
        }
    }

    private void PlayerOneIdle()
    {
        Debug.Log("PlayerOneIdle");

        if (_playerOneStates == PlayerOneStates.PlayerOneIdle)
        {
            PlayerOneIdleAnim();
        }

        if (PlayerIsGrounded())
        {
            return;
        }

        _playerOneMoveDirection = new Vector3(0, _playersSpeedYAxis, 0);

        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);

        _collisionFlags = _playerController.Move (_playerOneMoveDirection * Time.deltaTime);
    }
    private void PlayerWalkLeft()
    {
        Debug.Log("PlayerWalkLeft");
        PlayerOneRetreatAnim();

        _playerOneMoveDirection = new Vector3(+ _playerWalkSpeed, 0, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection).normalized;
        _playerOneMoveDirection *= _playerWalkSpeed;

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        if (Input.GetAxis("Horizontal") == 0 )
        {
            _playerOneStates = PlayerOneMovement.PlayerOneStates.PlayerOneIdle;
        }
    }

    private void PlayerWalkRight()
    {
        Debug.Log("PlayerWalkRight");
        PlayerOneWalkAnim();

        _playerOneMoveDirection = new Vector3(- _playerWalkSpeed, 0, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection).normalized;
        _playerOneMoveDirection *= _playerWalkSpeed;

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        if (Input.GetAxis("Horizontal") > 0f )
        {
            _playerOneStates = PlayerOneMovement.PlayerOneStates.PlayerOneIdle;
        }
    }

    private void PlayerJump()
    {
        Debug.Log("PlayerJump");

        PlayerJumpAnim();

        _playerOneMoveDirection = new Vector3(0, _playerJumpSpeed, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection).normalized;
        _playerOneMoveDirection *= _playerJumpSpeed;

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        if (_playerOneTransform.transform.position.y >= _playerJumpHeight)
        {
            _playerOneStates = PlayerOneMovement.PlayerOneStates.ComeDown;
        }
    }

    private void PlayerJumpForwards()
    {
        Debug.Log("PlayerJumpForwards");

        PlayerJumpAnim();

        _playerOneMoveDirection = new Vector3(-_playerJumpHorizontal, _playerJumpSpeed, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);
        _playerOneMoveDirection *= _playerJumpSpeed;

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        if (_playerOneTransform.transform.position.y >= _playerJumpHeight)
        {
            _playerOneStates = PlayerOneMovement.PlayerOneStates.ComeDownForwards;
        }
    }

    private void PlayerJumpBackwards()
    {
        Debug.Log("PlayerJumpBackwards");

        PlayerJumpAnim();

        _playerOneMoveDirection = new Vector3(+_playerJumpHorizontal, _playerJumpSpeed, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);
        _playerOneMoveDirection *= _playerJumpSpeed;

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        if (_playerOneTransform.transform.position.y >= _playerJumpHeight)
        {
            _playerOneStates = PlayerOneMovement.PlayerOneStates.ComeDownBackwards;
        }
    }

    private void ComeDown()
    {
        Debug.Log("ComeDown");

        _playerOneMoveDirection = new Vector3(0, _playersSpeedYAxis, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        if(PlayerIsGrounded())
            _playerOneStates =
            PlayerOneMovement.PlayerOneStates.PlayerOneIdle;
    }

    private void ComeDownForwards()
    {
        Debug.Log("ComeDownForwads");

        _playerOneMoveDirection = new Vector3(-_playerJumpHorizontal, _playersSpeedYAxis, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        if(PlayerIsGrounded())
            _playerOneStates =
            PlayerOneMovement.PlayerOneStates.PlayerOneIdle;
    }

    private void ComeDownBackwards()
    {
        Debug.Log("ComeDownBackwards");

        _playerOneMoveDirection = new Vector3(+_playerJumpHorizontal, _playersSpeedYAxis, 0);
        _playerOneMoveDirection = _playerOneTransform.TransformDirection(_playerOneMoveDirection);

        _collisionFlags = _playerController.Move(_playerOneMoveDirection * Time.deltaTime);

        if(PlayerIsGrounded())
            _playerOneStates =
            PlayerOneMovement.PlayerOneStates.PlayerOneIdle;
    }

    private void WaitForAnimations()
    {
        Debug.Log("WaitForAnimations");

        for (int pa = 0; pa < _playerAttackAnim.Length; pa++)
        {
            if (_playerOneAnim.IsPlaying(_playerAttackAnim[pa].name))
                return;
        }

        _playerOneStates =
            PlayerOneMovement.PlayerOneStates.PlayerOneIdle;
    }

    private void PlayerBlock()
    {
        Debug.Log("PlayerBlock");
        PlayerBlockAnim();
    }

    private void PlayerDemo()
    {
        Debug.Log("PlayerDemo");
        PlayerDemoAnimation();

        if (Input.GetAxis("LeftTrigger") > 0.1f)
            transform.Rotate(Vector3.up * _demoRotationValue * Time.deltaTime);

        if (Input.GetAxis("RightTrigger") > 0.1f)
            transform.Rotate(Vector3.down * _demoRotationValue * Time.deltaTime);
    }
    private void PlayerOneIdleAnim()
    {
        Debug.Log("PlayerOneIdleAnim");

        // Check if the animation clip is assigned
        if (_playerOneIdleAnim != null)
        {
            // Play the idle animation
            _playerOneAnim.CrossFade(_playerOneIdleAnim.name);
        }
        else
        {
            Debug.LogError("Idle animation clip is not assigned!");
        }
    }


    private void PlayerOneWalkAnim() 
    {
        Debug.Log("PlayerOneWalkAnim");

        _playerOneAnim.CrossFade(_playerOneWalkAnim.name); // Corrected from _playerOneIdleAnim to _playerOneWalkAnim

        if (_playerOneAnim[_playerOneWalkAnim.name].speed == _playerWalkSpeed)
            return;

        if (_playerOneAnim[_playerOneWalkAnim.name].speed < _playerWalkSpeed)
            _playerOneAnim[_playerOneWalkAnim.name].speed = _playerWalkSpeed;
    }

    private void PlayerOneRetreatAnim() 
    {
        Debug.Log("PlayerOneRetreatAnim");

        _playerOneAnim.CrossFade(_playerOneWalkAnim.name); // Corrected from _playerOneIdleAnim to _playerOneWalkAnim

        if (_playerOneAnim[_playerOneWalkAnim.name].speed == _playerRetreatSpeed)
            return;

        if (_playerOneAnim[_playerOneWalkAnim.name].speed > _playerRetreatSpeed)
            _playerOneAnim[_playerOneWalkAnim.name].speed = -_playerRetreatSpeed;
    }

    private void PlayerBlockAnim()
    {
        Debug.Log("PlayerBlockAnim");
        _playerOneAnim.CrossFade(_playerOneBlockAnim.name);
    }

    private void PlayerJumpAnim()
    {
        Debug.Log("PlayerJump");

        _playerOneAnim.CrossFade(_playerOneJumpAnim.name);
    }


    private void PlayerJabAnim()
    {
        Debug.Log("PlayerJabAnim");

        _playerOneAnim.CrossFade(_playerAttackAnim[0].name);
    }

    private void PlayerStraightAnim()
    {
        Debug.Log("PlayerHighPunchAnim");

        _playerOneAnim.CrossFade(_playerAttackAnim[1].name);
    }

    private void PlayerCrossAnim()
    {
        Debug.Log("PlayerCrossAnim");

        _playerOneAnim.CrossFade(_playerAttackAnim[2].name);
    }

    private void PlayerUppercutAnim()
    {
        Debug.Log("PlayerUppercutAnim");

        _playerOneAnim.CrossFade(_playerAttackAnim[3].name);
    }

    

    private void PlayerHookAnim()
    {
        Debug.Log("PlayerHookAnim");

        _playerOneAnim.CrossFade(_playerAttackAnim[4].name);
    }

    private void PlayerJab()
    {
        Debug.Log("PlayerJab");

        PlayerJabAnim();

        _playerOneStates =
            PlayerOneMovement.PlayerOneStates.WaitForAnimations;
    }

    private void PlayerStraight()
    {
        Debug.Log("PlayerStraight");

        PlayerStraightAnim();

        _playerOneStates =
            PlayerOneMovement.PlayerOneStates.WaitForAnimations;
    }

    private void PlayerCross()
    {
        Debug.Log("PlayerCross");

        PlayerCrossAnim();

        _playerOneStates =
            PlayerOneMovement.PlayerOneStates.WaitForAnimations;
    }

    private void PlayerUppercut()
    {
        Debug.Log("PlayerUppercut");

        PlayerUppercutAnim();

        _playerOneStates =
            PlayerOneMovement.PlayerOneStates.WaitForAnimations;
    }

    private void PlayerHook()
    {
        Debug.Log("PlayerHook");

        PlayerHookAnim();

        _playerOneStates =
            PlayerOneMovement.PlayerOneStates.WaitForAnimations;
    }

    private void PlayerDemoAnimation()
    {
        _playerOneAnim.CrossFade(_playerOneDemoAnim.name);
    }

    private void AttackInputManager()
    {
        Debug.Log("AttackInputManager");

        if (Input.GetButtonDown("Fire1"))
            _playerOneStates =
            PlayerOneMovement.PlayerOneStates.PlayerJab;

        if (Input.GetButtonDown("Fire2"))
            _playerOneStates =
            PlayerOneMovement.PlayerOneStates.PlayerUppercut;

        if (Input.GetButtonDown("Fire3"))
            _playerOneStates =
            PlayerOneMovement.PlayerOneStates.PlayerCross;

        if (Input.GetButtonDown("Fire4"))
            _playerOneStates =
            PlayerOneMovement.PlayerOneStates.PlayerHook;
    }

    private void HorizontalInputManager()
    {
        Debug.Log("HorizontalInputManager");

        if (Input.GetAxis("Vertical")> _controllerDeadZonePos && Input.GetAxis("Horizontal") > _controllerDeadZoneNeg)
            _playerOneStates =
            PlayerOneMovement.PlayerOneStates.PlayerJumpForwards;

        if (Input.GetAxis("Vertical")> _controllerDeadZonePos && Input.GetAxis("Horizontal") < _controllerDeadZoneNeg)
            _playerOneStates =
            PlayerOneMovement.PlayerOneStates.PlayerJumpBackwards;
    }

    private void StandartInputManager()
    {
        Debug.Log("StandartInputManager");

        if (Input.GetAxis("Horizontal") < _controllerDeadZoneNeg )
        {
            _playerOneStates = PlayerOneMovement.PlayerOneStates.PlayerWalkLeft;
        }

        if (Input.GetAxis("Horizontal") > _controllerDeadZonePos )
        {
            _playerOneStates = PlayerOneMovement.PlayerOneStates.PlayerWalkRight;
        }

        if (Input.GetAxis("Vertical") > _controllerDeadZonePos )
        {
            _playerOneStates = PlayerOneMovement.PlayerOneStates.PlayerBlock;
        }
    }

    private void ApplyGravity()
    {
        Debug.Log("ApplyGravity");

        if (PlayerIsGrounded())
            _playersSpeedYAxis = 0f;
        else
            _playersSpeedYAxis -= _playersGravity * _playerGravityModifier * Time.deltaTime;
    }

    public bool PlayerIsGrounded()
    {
        return(_collisionFlags & CollisionFlags.CollidedBelow) != 0;
    }

}

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
    public AnimationClip[] _playerAttackAnim;

    public float _controllerDeadZonePos = 0.1f;
    public  float _controllerDeadZoneNeg = 0.1f;

    public float _playersGravity = 20f;
    public float _playerGravityModifier = 5f;
    public float _playersSpeedYAxis;

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
        ComeDownBackwards
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
    }

    // Update is called once per frame
    void Update()
    {
        ApplyGravity();

        if (Input.GetAxis("Horizontal") < _controllerDeadZoneNeg )
        {
            _playerOneStates = PlayerOneMovement.PlayerOneStates.PlayerWalkLeft;
        }

        if (Input.GetAxis("Horizontal") > _controllerDeadZonePos )
        {
            _playerOneStates = PlayerOneMovement.PlayerOneStates.PlayerWalkLeft;
        }

        if (Input.GetAxis("Vertical") > _controllerDeadZonePos )
        {
            _playerOneStates = PlayerOneMovement.PlayerOneStates.PlayerBlock;
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
    }

    private void PlayerJumpBackwards()
    {
        Debug.Log("PlayerJumpBackwards");
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
    }

    private void ComeDownBackwards()
    {
        Debug.Log("ComeDownBackwards");
    }

    private void PlayerBlock()
    {
        Debug.Log("PlayerBlock");
        PlayerBlockAnim();
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
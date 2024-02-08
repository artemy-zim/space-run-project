using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private CharacterController _controller;

    [SerializeField] private float _jumpForce = 10.0f;
    [SerializeField] private float _gravityForce = 5.0f;
    
    public float RollDistance = 3.0f;

    private float _animationRollTime;
    private float _animationRollSpeed;
    private float _jumpSpeed;

    private int _currentPath = 2;
    private float _currentDistance = 0f;
    private float _currentDirection = 0f;

    private bool _isAlive;

    public static PlayerController Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();

        _animationRollTime = GetAnimationRollTime(_animator.runtimeAnimatorController.animationClips);
        _animationRollSpeed = RollDistance / _animationRollTime;

        StartCoroutine(OnPlayerMovementCheckCoroutine());
    }

    private float GetAnimationRollTime(AnimationClip[] clips)
    {
        foreach (var clip in clips)
        {
            if (clip.name.Equals("Left Roll") || clip.name.Equals("Right Roll"))
                return clip.length;
        }
        Debug.LogWarning("Анимация переката не найдена!");
        return 0f;
    }

    
    public void RunStart()
    {
        _animator.SetTrigger("Run");
    }

    void Update()
    {
        ApplyGravity();
        UpdateLifeStatus();
    }

    void UpdateLifeStatus()
    {
        _isAlive = SceneEngine.Instance.IsGameRunning;
    }

    private void ApplyGravity()
    {
        _controller.Move(Vector3.down * _gravityForce * Time.deltaTime);
    }

    private IEnumerator OnPlayerMovementCheckCoroutine()
    {
        while (true)
        {
            if (_isAlive)
            {
                float directionHor = Input.GetAxisRaw("Horizontal");
                float directionVer = Input.GetAxisRaw("Vertical");

                if ((directionHor > 0 && _currentPath < 3) || (directionHor < 0 && _currentPath > 1))
                {
                    _currentDirection = directionHor;
                    _currentDistance = RollDistance;
                    _animator.SetTrigger(directionHor < 0 ? "Left" : "Right");

                    yield return StartCoroutine(OnPlayerMoveCoroutine());
                }

                if (directionVer == 1)
                {
                    _jumpSpeed = _jumpForce;
                    _animator.SetBool("isJumping", true);

                    yield return StartCoroutine(OnPlayerJumpCoroutine());
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator OnPlayerJumpCoroutine()
    {
        while (true)
        {
            _jumpSpeed -= Time.deltaTime * _gravityForce;
            _controller.Move(_jumpSpeed * Time.deltaTime * Vector3.up);

            if (_controller.isGrounded)
            {
                _animator.SetBool("isJumping", false);
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator OnPlayerMoveCoroutine()
    {
        while (true)
        {
            if (_currentDistance <= 0)
            {
                _currentPath = _currentDirection < 0 ? _currentPath - 1 : _currentPath + 1;
                break;
            }

            if (!_isAlive)
            {
                break;
            }

            float tmpDistance = Time.deltaTime * _animationRollSpeed;

            _controller.Move(_currentDirection * tmpDistance * Vector3.right);
            _currentDistance -= tmpDistance;

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Danger"))
        {
            PlayerDeathEvent();
        }
    }

    private void PlayerDeathEvent()
    {
        SceneEngine.Instance.InitGameEnd();
        _animator.SetTrigger("Death");
    }
}

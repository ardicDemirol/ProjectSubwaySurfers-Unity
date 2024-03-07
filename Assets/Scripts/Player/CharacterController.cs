using UnityEngine;
using DG.Tweening;
using System.Collections;

public class CharacterController : MonoBehaviour
{
    #region Variables
    public enum PlayerSide
    {
        Left,
        Right,
        Middle
    }
    public enum RenderingMode
    {
        Opaque,
        Transparent
    }


    [SerializeField] private PlayerSide playerSide = PlayerSide.Middle;
    [SerializeField] private float jumpForce = 2.5f;
    [SerializeField] private float slideSpeed = 3f;
    [SerializeField] private float jumpSpeed = 2f;
    [SerializeField] private short playerHealth = 3;

    [Header("Material Settings")]
    [SerializeField] private Renderer characterRenderer;
    [SerializeField] private Material opaqueMaterial;
    [SerializeField] private Material transparentMaterial;


    private static Animator _animator;
    private CapsuleCollider _collider;

    private static readonly WaitForSeconds _waitForOneHalfSeconds = new(1.5f);
    private IEnumerator _slideTimer;

    private bool _canJump = true;
    private bool _canSlide = true;
    private bool _canDamage = true;
    private bool _isMoveComplete = true;
    private bool _isPlayerDead;
    private bool _isTouchingObstacleSide;
    private readonly float _moveDistance = 2f;


    private static readonly int _animatorHashIsJump = Animator.StringToHash("isJumping");
    private static readonly int _animatorHashIsDie = Animator.StringToHash("isDying");
    private static readonly int _animatorHashIsSlide = Animator.StringToHash("isSliding");

    private Touch touch;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider>();
    }
    private void Start()
    {
        Signals.Instance.OnPlayerTakeDamage?.Invoke(playerHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle") && _canDamage)
        {
            _canDamage = false;
            HealthController();
            FadeController();
        }
        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            Signals.Instance.OnCoinCollected?.Invoke();
        }
        if (other.gameObject.CompareTag("PieceGenerator"))
        {
            Signals.Instance.OnGenerateLevel?.Invoke();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _canJump = true;
            _animator.ResetTrigger(_animatorHashIsJump);
        }
        if (collision.gameObject.CompareTag("ObstacleLeftSide") && !_isTouchingObstacleSide)
        {
            Handheld.Vibrate();
            _isTouchingObstacleSide = true;
            if (playerSide == PlayerSide.Middle)
            {
                playerSide = PlayerSide.Left;
                transform.DOMoveX(1, 1 / slideSpeed).OnComplete(() => _isMoveComplete = true);
            }
            else if (playerSide == PlayerSide.Right)
            {
                playerSide = PlayerSide.Middle;
                transform.DOMoveX(3, 1 / slideSpeed).OnComplete(() => _isMoveComplete = true);
            }
        }
        if (collision.gameObject.CompareTag("ObstacleRightSide") && !_isTouchingObstacleSide)
        {
            Handheld.Vibrate();
            _isTouchingObstacleSide = true;
            if (playerSide == PlayerSide.Middle)
            {
                playerSide = PlayerSide.Right;
                transform.DOMoveX(5, 1 / slideSpeed).OnComplete(() => _isMoveComplete = true);
            }
            else if (playerSide == PlayerSide.Left)
            {
                playerSide = PlayerSide.Middle;
                transform.DOMoveX(3, 1 / slideSpeed).OnComplete(() => _isMoveComplete = true);
            }
        }
        if (collision.gameObject.CompareTag("ObstacleLeftInnerSide") || collision.gameObject.CompareTag("ObstacleRightInnerSide"))
        {
            Handheld.Vibrate();
            _isTouchingObstacleSide = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("ObstacleLeftSide") || collision.gameObject.CompareTag("ObstacleRightSide"))
        {
            _isTouchingObstacleSide = false;
        }
    }

    private void Update()
    {
        if (_isPlayerDead) return;
        GetInput();
    }

    #endregion

    #region Other Methods

    void GetInput()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                float deltaX = touch.deltaPosition.x;
                float deltaY = touch.deltaPosition.y;

                if (Mathf.Abs(deltaY) > Mathf.Abs(deltaX))
                {
                    if (deltaY > 0  && _canJump && _canSlide)
                    {
                        _canJump = false;
                        _animator.SetTrigger(_animatorHashIsJump);

                        transform.DOMoveY(transform.position.y + jumpForce, 1 / jumpSpeed);
                    }
                    else if (deltaY < 0 && _canJump && _canSlide)
                    {
                        _slideTimer = SlideTimer();
                        StartCoroutine(_slideTimer);
                        _animator.SetTrigger(_animatorHashIsSlide);
                    }
                }
                else
                {
                    if (deltaX < 0 && playerSide != PlayerSide.Left && _isMoveComplete)
                    {
                        _isMoveComplete = false;
                        transform.DOMoveX(transform.position.x - _moveDistance, 1 / slideSpeed).OnComplete(() =>
                        {
                            _isMoveComplete = true;
                        });

                        if (playerSide == PlayerSide.Middle) playerSide = PlayerSide.Left;
                        else playerSide = PlayerSide.Middle;

                    }
                    else if (deltaX > 0 && playerSide != PlayerSide.Right && _isMoveComplete)
                    {
                        _isMoveComplete = false;

                        transform.DOMoveX(transform.position.x + _moveDistance, 1 / slideSpeed).OnComplete(() =>
                        {
                            _isMoveComplete = true;
                        });

                        if (playerSide == PlayerSide.Middle) playerSide = PlayerSide.Right;
                        else playerSide = PlayerSide.Middle;
                    }
                }
            }
        }
    }

    private IEnumerator SlideTimer()
    {
        _canSlide = false;
        _collider.height = 1f;
        _collider.center = new Vector3(0, 0.5f, 0);
        yield return _waitForOneHalfSeconds;
        _canSlide = true;
        _collider.height = 1.8f;
        _collider.center = new Vector3(0, 0.9f, 0);
        StopCoroutine(_slideTimer);
    }

    void HealthController()
    {
        playerHealth--;
        Signals.Instance.OnPlayerTakeDamage?.Invoke(playerHealth);
        Handheld.Vibrate();
        if (playerHealth <= 0)
        {
            _isPlayerDead = true;
            _animator.SetTrigger(_animatorHashIsDie);
            Signals.Instance.OnPlayerDie?.Invoke();
        }
    }

    private void FadeController()
    {
        SetRenderingMode(RenderingMode.Transparent);
        characterRenderer.material.DOFade(0, 0.2f).OnComplete(() => characterRenderer.material.DOFade(1, 0.2f)).SetLoops(14, LoopType.Yoyo).OnComplete(() =>
        {
            SetRenderingMode(RenderingMode.Opaque);
            _canDamage = true;
        });
    }

    private void SetRenderingMode(RenderingMode mode)
    {
        switch (mode)
        {
            case RenderingMode.Opaque:
                characterRenderer.material = opaqueMaterial;
                break;
            case RenderingMode.Transparent:
                characterRenderer.material = transparentMaterial;
                break;
            default: break;
        }
    }


    #endregion
}

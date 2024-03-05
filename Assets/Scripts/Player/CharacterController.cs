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

    [SerializeField] private PlayerSide playerSide = PlayerSide.Middle;

    [SerializeField] private float jumpForce = 2.5f;
    [SerializeField] private float moveDistance = 2f;
    [SerializeField] private float slideSpeed = 3f;
    [SerializeField] private float jumpSpeed = 2f;
    [SerializeField] private short playerHealth = 3;
    [SerializeField] private Renderer characterRenderer;


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
            //Debug.Log("Player collided with " + other.gameObject.name);
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
                    if (deltaY > 0 && /*Input.GetKeyDown(KeyCode.W) &&*/ _canJump && _canSlide)
                    {
                        _canJump = false;
                        _animator.SetTrigger(_animatorHashIsJump);

                        transform.DOMoveY(transform.position.y + jumpForce, 1 / jumpSpeed);
                    }
                    else if (deltaY < 0 && /*Input.GetKeyDown(KeyCode.S) &&*/ _canJump && _canSlide)
                    {
                        _slideTimer = SlideTimer();
                        StartCoroutine(_slideTimer);
                        _animator.SetTrigger(_animatorHashIsSlide);
                    }
                }
                else
                {
                    if (deltaX < 0 && /*Input.GetKeyDown(KeyCode.A) &&*/ playerSide != PlayerSide.Left && _isMoveComplete)
                    {
                        _isMoveComplete = false;
                        transform.DOMoveX(transform.position.x - moveDistance, 1 / slideSpeed).OnComplete(() =>
                        {
                            _isMoveComplete = true;
                        });

                        if (playerSide == PlayerSide.Middle) playerSide = PlayerSide.Left;
                        else playerSide = PlayerSide.Middle;

                    }
                    else if (deltaX > 0 && /*Input.GetKeyDown(KeyCode.D) &&*/ playerSide != PlayerSide.Right && _isMoveComplete)
                    {
                        _isMoveComplete = false;

                        transform.DOMoveX(transform.position.x + moveDistance, 1 / slideSpeed).OnComplete(() =>
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
        if (playerHealth <= 0)
        {
            _isPlayerDead = true;
            _animator.SetTrigger(_animatorHashIsDie);
            Signals.Instance.OnPlayerDie?.Invoke();
        }
    }

    private void FadeController()
    {
        characterRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        characterRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        characterRenderer.material.SetInt("_ZWrite", 0);
        characterRenderer.material.EnableKeyword("_ALPHABLEND_ON");
        characterRenderer.material.renderQueue = 3000;


        characterRenderer.material.DOFade(0, 0.2f).OnComplete(() => characterRenderer.material.DOFade(1, 0.2f)).SetLoops(14, LoopType.Yoyo).OnComplete(() =>
        {
            characterRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            characterRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            characterRenderer.material.SetInt("_ZWrite", 1);
            characterRenderer.material.DisableKeyword("_ALPHABLEND_ON");
            characterRenderer.material.renderQueue = -1;

            _canDamage = true;

        });
    }
    #endregion
}

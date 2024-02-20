using UnityEngine;
using DG.Tweening;
using System.Collections;

public class CharacterController : MonoBehaviour
{
    public enum PlayerSide
    {
        Left,
        Right,
        Middle
    }

    [SerializeField] private PlayerSide playerSide = PlayerSide.Middle;

    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private float moveDistance = 4f;
    [SerializeField] private float slideSpeed = 2f;
    [SerializeField] private float jumpSpeed = 2f;

    [SerializeField] private short playerHealth = 3;


    private Animator _animator;
    private Rigidbody _rb;
    private CapsuleCollider _collider;
    private Renderer[] _renderer;

    private static readonly WaitForSeconds _waitForOneHalfSeconds = new(1.5f);
    private IEnumerator _slideTimer;

    private bool _canJump = true;
    private bool _canSlide = true;
    private bool _canDamage = true;
    private bool _isMoveComplete = true;
    private bool _isPlayerDead;


    private static readonly int _animatorHashIsJump = Animator.StringToHash("isJumping");
    private static readonly int _animatorHashIsDie = Animator.StringToHash("isDying");
    private static readonly int _animatorHashIsSlide = Animator.StringToHash("isSliding");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<CapsuleCollider>();
        _renderer = GetComponentsInChildren<Renderer>();
    }
    private void Start()
    {
        Signals.Instance.OnPlayerTakeDamage?.Invoke(playerHealth);
    }

    private void Update()
    {
        if (_isPlayerDead) return;
        GetInput();
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
        //Debug.Log("Player collided with " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Ground"))
        {
            _canJump = true;
            _animator.ResetTrigger(_animatorHashIsJump);
        }
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && _canJump && _canSlide)
        {
            _canJump = false;
            _animator.SetTrigger(_animatorHashIsJump);

            transform.DOMoveY(transform.position.y + jumpForce, 1 / jumpSpeed);

        }
        else if (Input.GetKeyDown(KeyCode.S) && _canSlide)
        {
            _slideTimer = SlideTimer();
            StartCoroutine(_slideTimer);
            _animator.SetTrigger(_animatorHashIsSlide);
        }
        else if (Input.GetKeyDown(KeyCode.A) && playerSide != PlayerSide.Left && _isMoveComplete)
        {
            _isMoveComplete = false;
            transform.DOMoveX(transform.position.x - moveDistance, 1 / slideSpeed).OnComplete(() => _isMoveComplete = true);

            if (playerSide == PlayerSide.Middle) playerSide = PlayerSide.Left;
            else playerSide = PlayerSide.Middle;
        }
        else if (Input.GetKeyDown(KeyCode.D) && playerSide != PlayerSide.Right && _isMoveComplete)
        {
            _isMoveComplete = false;

            transform.DOMoveX(transform.position.x + moveDistance, 1 / slideSpeed).OnComplete(() => _isMoveComplete = true);

            if (playerSide == PlayerSide.Middle) playerSide = PlayerSide.Right;
            else playerSide = PlayerSide.Middle;
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
        foreach (var _renderer in _renderer)
            _renderer.material.DOFade(0, 0.2f).OnComplete(() => _renderer.material.DOFade(1, 0.2f)).SetLoops(14, LoopType.Yoyo).OnComplete(() => _canDamage = true);
    }



}

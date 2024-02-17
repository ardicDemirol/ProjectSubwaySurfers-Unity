using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerSide
    {
        Left,
        Right,
        Middle
    }

    [SerializeField] private PlayerSide playerSide = PlayerSide.Middle;

    
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float moveDistance;
    

    private Rigidbody _rb;
    private Animator _animator;
    private bool _isJump;

    private static readonly int _animatorHashIsJump = Animator.StringToHash("isJumping");
    private static readonly int _animatorHashIsDie = Animator.StringToHash("isDying");
    private static readonly int _animatorHashIsSlide = Animator.StringToHash("isSliding");

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        GetInput();  
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.W) && _isJump)
        {
            _rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            _isJump = false;
            _animator.SetTrigger(_animatorHashIsJump);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _animator.SetTrigger(_animatorHashIsSlide);
        }
        else if (Input.GetKeyDown(KeyCode.A) && playerSide != PlayerSide.Left)
        {
            _rb.AddForce(new Vector3(-moveDistance, 0, 0), ForceMode.Impulse);
            if (playerSide == PlayerSide.Middle) playerSide = PlayerSide.Left;
            else playerSide = PlayerSide.Middle;
        }
        else if (Input.GetKeyDown(KeyCode.D) && playerSide != PlayerSide.Right)
        {
            _rb.AddForce(new Vector3(moveDistance, 0, 0), ForceMode.Impulse);

            if (playerSide == PlayerSide.Middle) playerSide = PlayerSide.Right;
            else playerSide = PlayerSide.Middle;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Player collided with " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Ground"))
        {
            _isJump = true;
            _animator.ResetTrigger(_animatorHashIsJump);

        }
    }
}

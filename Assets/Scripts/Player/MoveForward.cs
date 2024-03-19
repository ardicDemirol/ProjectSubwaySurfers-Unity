using UnityEngine;

public class MoveForward : MonoBehaviour
{
    #region Variables
    
    public Direction MoveDirection;

    [SerializeField] private float moveSpeed = 5f;

    private Vector3 _moveDirectionVector;
    private bool _speedCanIncrease;
    private const float SPEED_MULTIPLIER = 0.075f;

    #endregion

    #region Unity Callbacks
    private void OnEnable() => SubscribeEvents();

    private void Start()
    {
        switch (MoveDirection)
        {
            case Direction.Forward:
                _moveDirectionVector = Vector3.forward;
                break;
            case Direction.Backward:
                _moveDirectionVector = Vector3.back;
                break;
        }
    }
    private void Update()
    {
        if (!_speedCanIncrease) return;
        moveSpeed += Time.deltaTime * SPEED_MULTIPLIER;
        transform.position += _moveDirectionVector * (moveSpeed * Time.deltaTime);
    }
    private void OnDisable() => UnSubscribeEvents();

    #endregion

    #region Other Methods
    public void SubscribeEvents()
    {
        Signals.Instance.OnPlayerDie += OnPlayerDie;
        Signals.Instance.OnGameRunning += OnGameRunning;
    }

    public void UnSubscribeEvents()
    {
        Signals.Instance.OnPlayerDie -= OnPlayerDie;
        Signals.Instance.OnGameRunning -= OnGameRunning;
    }

    private void OnGameRunning()
    {
        Time.timeScale = 1;
        _speedCanIncrease = true;
    }

    private void OnPlayerDie()
    {
        _speedCanIncrease = false;
        moveSpeed = 0;
    }

    #endregion

}

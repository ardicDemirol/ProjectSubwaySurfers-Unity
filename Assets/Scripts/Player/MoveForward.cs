using UnityEngine;

public class MoveForward : MonoBehaviour
{
    #region Variables
    public enum Direction
    {
        Forward,
        Backward,
    }

    public Direction MoveDirection;

    [SerializeField] private float moveSpeed = 5f;

    private Vector3 _moveDirectionVector;

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
        moveSpeed += Time.deltaTime * 0.015f;
        transform.position += _moveDirectionVector * (moveSpeed * Time.deltaTime);
    }
    private void OnDisable() => UnSubscribeEvents();

    #endregion

    #region Other Methods
    public void SubscribeEvents()
    {
        Signals.Instance.OnPlayerDie += OnPlayerDie;
    }

    public void UnSubscribeEvents()
    {
        Signals.Instance.OnPlayerDie -= OnPlayerDie;
    }

    private void OnPlayerDie()
    {
        moveSpeed = 0;
    }

    #endregion

}

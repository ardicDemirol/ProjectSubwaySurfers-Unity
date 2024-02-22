using DG.Tweening;
using UnityEngine;

public class ObjectMovementController : MonoBehaviour
{
    [SerializeField] protected float rotationDuration = 1f;
    [SerializeField] protected float moveDistance = 0.2f;

    protected Vector3 _initPos;
    protected Tween _rotateTween;
    protected Tween _moveTween;

    protected virtual void Awake()
    {
        _initPos = transform.localPosition;
    }

    protected virtual void OnEnable()
    {
        transform.SetLocalPositionAndRotation(_initPos, Quaternion.identity);
        StartMovement();
    }

    protected virtual void StartMovement()
    {
        _rotateTween?.Kill();
        _moveTween?.Kill();
    }
}
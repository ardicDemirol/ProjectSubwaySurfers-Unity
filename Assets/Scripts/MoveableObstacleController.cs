using DG.Tweening;
using UnityEngine;

public class MoveableObstacleController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.75f;
    [SerializeField] private float moveDuration = 30f;
    [SerializeField] private float moveDistance = 100f;

    private Vector3 _initPos;
    private Tween _rotateTween;
    private Tween _moveTween;
    private void Awake()
    {
        _initPos = transform.localPosition;
    }


    private void OnEnable()
    {
        transform.SetLocalPositionAndRotation(_initPos, Quaternion.identity);
        StartMovement();
    }

    private void StartMovement()
    {
         _rotateTween?.Kill();
        _moveTween?.Kill();

        _rotateTween = transform.DOLocalRotate(new Vector3(-360, 0, 0), 1 / rotationSpeed, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        _moveTween = transform.DOLocalMoveZ(transform.localPosition.z - moveDistance, moveDuration);
    }

}

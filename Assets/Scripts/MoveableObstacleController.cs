using DG.Tweening;
using UnityEngine;

public class MoveableObstacleController : ObjectMovementController
{
    [SerializeField] private float moveDuration = 30f;
    protected override void StartMovement()
    {
        base.StartMovement();

        _rotateTween = transform.DOLocalRotate(new Vector3(-360, 0, 0), rotationDuration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        _moveTween = transform.DOLocalMoveZ(transform.localPosition.z - moveDistance, moveDuration);
    }
}



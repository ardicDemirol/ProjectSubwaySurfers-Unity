using DG.Tweening;
using UnityEngine;

public class CoinController : ObjectMovementController
{

    #region Unity Callbacks
    protected override void OnEnable()
    {
        base.OnEnable();
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    #endregion

    #region Other Methods
    protected override void StartMovement()
    {
        base.StartMovement();

        _rotateTween = transform.DOLocalRotate(new Vector3(0, 360, 0), rotationDuration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart).SetEase(Ease.InOutSine);
        _moveTween = transform.DOMoveY(transform.position.y + moveDistance, rotationDuration)
            .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    #endregion
}








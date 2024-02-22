using DG.Tweening;
using UnityEngine;

public class CoinController : MovementController
{
    protected override void OnEnable()
    {
        base.OnEnable();
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
    protected override void StartMovement()
    {
        base.StartMovement();

        _rotateTween = transform.DOLocalRotate(new Vector3(0, 360, 0), rotationDuration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart).SetEase(Ease.InOutSine);
        _moveTween = transform.DOMoveY(transform.position.y + moveDistance, rotationDuration)
            .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}

//using DG.Tweening;
//using UnityEngine;

//public class CoinController : MonoBehaviour
//{
//    [SerializeField] private float rotationDuration = 1f;
//    [SerializeField] private float moveDistance = 0.2f;

//    private Vector3 _initPos;
//    private Tween _rotateTween;
//    private Tween _moveTween;

//    private void Awake()
//    {
//        _initPos = transform.localPosition;
//    }

//    private void OnEnable()
//    {
//        transform.SetLocalPositionAndRotation(_initPos, Quaternion.identity);
//        StartMovement();
//        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
//    }

//    private void StartMovement()
//    {
//        _rotateTween?.Kill();
//        _moveTween?.Kill();

//        _rotateTween = transform.DOLocalRotate(new Vector3(0, 360, 0), rotationDuration, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.InOutSine);
//        _moveTween = transform.DOMoveY(transform.position.y + moveDistance, rotationDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
//    }

//}






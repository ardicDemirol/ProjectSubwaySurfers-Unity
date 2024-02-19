using DG.Tweening;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float moveDistance = 0.2f;

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
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }
    
    private void StartMovement()
    {
        _rotateTween?.Kill();
        _moveTween?.Kill();

        _rotateTween = transform.DOLocalRotate(new Vector3(0, 360, 0), 1 / rotationSpeed, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.InOutSine);
        _moveTween = transform.DOMoveY(transform.position.y + moveDistance, 1 / rotationSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

}

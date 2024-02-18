using DG.Tweening;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float moveDistance = 0.2f;

    void Start()
    {
        transform.DOLocalRotate(new Vector3(0, 360, 0), 1 / rotationSpeed, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.InOutSine);
        transform.DOMoveY(transform.position.y + moveDistance, 1 / rotationSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

}

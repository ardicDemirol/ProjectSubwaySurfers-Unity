using DG.Tweening;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float moveDistance;

    void Start()
    {
        transform.DOLocalRotate(new Vector3(0, 360, 0), 1 / speed, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        transform.DOMoveY(transform.position.y + moveDistance, 1 / speed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

}

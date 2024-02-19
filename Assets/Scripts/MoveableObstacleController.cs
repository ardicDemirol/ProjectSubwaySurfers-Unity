using DG.Tweening;
using UnityEngine;

public class MoveableObstacleController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.75f;
    [SerializeField] private float moveSpeed = 0.3f;
    [SerializeField] private float moveDistance = 30f;

    private Vector3 _initialPos;

    private void Awake()
    {
        _initialPos = transform.position;
    }

    void Start()
    {
        transform.DOLocalRotate(new Vector3(360, 0, 0), 1 / rotationSpeed, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        transform.DOMoveZ(transform.position.z - moveDistance, 1 / moveSpeed);
    }

    void OnDisable()
    {
        transform.position = _initialPos;
        transform.rotation = Quaternion.identity;
    }
}

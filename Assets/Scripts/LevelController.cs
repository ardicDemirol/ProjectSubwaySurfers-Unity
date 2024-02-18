using UnityEngine;

public class LevelController : MonoBehaviour
{

    [SerializeField] private GameObject[] pieces;
    [SerializeField] private int zPos = 150;

    private int _piecesIndex;

    private void OnEnable() => SubscribeEvents();
    private void OnDisable() => UnSubscribeEvents();

    private void SubscribeEvents()
    {
        Signals.Instance.OnGenerateLevel += GenerateLevel;
    }

    private void UnSubscribeEvents()
    {
        Signals.Instance.OnGenerateLevel -= GenerateLevel;
    }

    private void GenerateLevel()
    {
        _piecesIndex = Random.Range(0, pieces.Length);
        Instantiate(pieces[_piecesIndex], new Vector3(0, 0, zPos), Quaternion.identity);
        zPos += 150;
    }

}

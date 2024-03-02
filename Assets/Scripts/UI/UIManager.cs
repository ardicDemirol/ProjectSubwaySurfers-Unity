using System.Text;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private readonly GameObject inGamePanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private readonly GameObject finishPanel;
    [SerializeField] private TextMeshProUGUI levelScoreText;
    [SerializeField] private TextMeshProUGUI maxScoreText;

    private float _score;
    private float _maxScore;

    private bool _isPlayerDead;

    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        _maxScore = PlayerPrefs.GetFloat("Score", 0f);
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }


    private void Update()
    {
        if (_isPlayerDead) return;

        if (_score > _maxScore) scoreText.color = Color.yellow;

        _score += Time.deltaTime * 5f;

        StringBuilder stringBuilder = new();
        stringBuilder.Append("Score: " + (int)_score);
        scoreText.text = stringBuilder.ToString();
    }

    private void OnDisable() => UnSubscribeEvents();


    #endregion

    #region Other Methods
    private void SubscribeEvents()
    {
        Signals.Instance.OnPlayerTakeDamage += PlayerTakeDamage;
        Signals.Instance.OnPlayerDie += ControlScore;
        Signals.Instance.OnCoinCollected += CoinCollected;
    }

    private void UnSubscribeEvents()
    {
        Signals.Instance.OnPlayerTakeDamage -= PlayerTakeDamage;
        Signals.Instance.OnPlayerDie -= ControlScore;
        Signals.Instance.OnCoinCollected -= CoinCollected;
    }

    private void CoinCollected()
    {
        _score += 75;
    }

    private void PlayerTakeDamage(short arg0)
    {
        healthText.text = "Health: " + arg0;
    }

    private void ControlScore()
    {
        _isPlayerDead = true;
        if (_score > _maxScore)
        {
            _maxScore = _score;
            PlayerPrefs.SetFloat("Score", _maxScore);
        }

        inGamePanel.SetActive(false);
        finishPanel.SetActive(true);
        levelScoreText.text = "Level Score: " + (int)_score;
        maxScoreText.text = "Max Score: " + (int)_maxScore;
    }

    #endregion

}

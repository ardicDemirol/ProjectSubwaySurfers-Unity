using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Variables

    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject finishPanel;
    [SerializeField] private TextMeshProUGUI levelScoreText;
    [SerializeField] private TextMeshProUGUI maxScoreText;

    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private Button startButton;
    private bool _isPaused;


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
        Signals.Instance.OnGameRunning += CanvasController;

    }

    private void UnSubscribeEvents()
    {
        Signals.Instance.OnPlayerTakeDamage -= PlayerTakeDamage;
        Signals.Instance.OnPlayerDie -= ControlScore;
        Signals.Instance.OnCoinCollected -= CoinCollected;
        Signals.Instance.OnGameRunning -= CanvasController;

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

    private void CanvasController()
    {
        startPanel.SetActive(false);
        inGamePanel.SetActive(true);
    }

    public void TogglePause()
    {
        pauseButton.SetActive(_isPaused);
        pausePanel.SetActive(!_isPaused);
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0f : 1f;
    }



    #endregion

}

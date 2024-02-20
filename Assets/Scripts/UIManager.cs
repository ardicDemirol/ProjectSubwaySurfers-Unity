using System.Text;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI healthText; 
    
    private float _score;
    private float _maxScore;

    private bool _isPlayerDead;

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void Awake()
    {
        _maxScore = PlayerPrefs.GetFloat("Score",0f);
        //healthText.text = "Health: " + 3;
      
    }
    private void Update()
    {
        if (_isPlayerDead) return;

        if (_score > _maxScore) scoreText.color = Color.yellow;

        _score += Time.deltaTime * 10;

        StringBuilder stringBuilder = new();
        stringBuilder.Append("Score: " + (int)_score);
        scoreText.text = stringBuilder.ToString();
    }

    private void OnDisable() => UnSubscribeEvents();

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
        _score += 200;
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
    }

}

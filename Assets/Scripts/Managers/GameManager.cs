using NaughtyAttributes;
using UnityEngine;

internal class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private AudioClip _coinCollectClip;
    private AudioSource _audioSource;


    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
        Time.timeScale = 0;
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }


    #region Other Methods

   
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void StartGame()
    {
        _audioSource.Play();
        Signals.Instance.OnGameRunning?.Invoke();
    }

    void SubscribeEvents()
    {
        Signals.Instance.OnCoinCollected += PlayCoinCollectAudio;
    }

    void UnSubscribeEvents()
    {
        Signals.Instance.OnCoinCollected -= PlayCoinCollectAudio;
    }

    [Button]
    private void PlayCoinCollectAudio()
    {
        _audioSource.PlayOneShot(_coinCollectClip);
    }
    #endregion

}

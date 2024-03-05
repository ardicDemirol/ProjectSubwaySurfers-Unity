using UnityEngine;

internal class GameManager : MonoSingleton<GameManager>
{
    #region Other Methods

    private AudioSource _audioSource;

    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
        Time.timeScale = 0;
    }
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void StartGame()
    {
        _audioSource.Play();
        Signals.Instance.OnGameRunning?.Invoke();
    }


    #endregion

}

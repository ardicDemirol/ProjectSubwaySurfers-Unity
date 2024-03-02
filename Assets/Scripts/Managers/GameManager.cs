
internal class GameManager : MonoSingleton<GameManager>
{
    #region Other Methods
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    #endregion

}

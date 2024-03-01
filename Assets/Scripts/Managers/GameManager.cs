
internal class GameManager : MonoSingleton<GameManager>
{
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

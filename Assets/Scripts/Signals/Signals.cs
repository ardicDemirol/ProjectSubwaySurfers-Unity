using UnityEngine.Events;

public class Signals : MonoSingleton<Signals>
{
    public UnityAction OnPlayerDie = delegate { };
    public UnityAction<short> OnPlayerTakeDamage = delegate { };
    public UnityAction OnCoinCollected = delegate { };
    public UnityAction OnGenerateLevel = delegate { };
    public UnityAction OnGameRunning = delegate { };
}
using UnityEngine.Events;

public class Signals : MonoSingleton<Signals>
{
    public UnityAction OnPlayerDie = delegate { };
    public UnityAction<short> OnPlayerTakeDamage = delegate { };
}
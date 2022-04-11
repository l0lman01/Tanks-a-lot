using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent _event;
    [SerializeField] private UnityEvent _onEventRaised;

    private void OnEnable()
    {
        _event.AddListener(this);   
    }

    private void OnDisable()
    {
        _event.RemoveListener(this);
    }

    public void OnEventRaised()
    {
        _onEventRaised.Invoke();
    }
}

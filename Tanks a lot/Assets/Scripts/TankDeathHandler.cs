using UnityEngine;

/// <summary>
/// Handles tank death events (broadcasting to game systems)
/// Attach this to tanks alongside TankHealth
/// </summary>
public class TankDeathHandler : MonoBehaviour
{
    private TankHealth _tankHealth;
    private Unit _unit;

    private void Awake()
    {
        _tankHealth = GetComponent<TankHealth>();
        _unit = GetComponent<Unit>();

        if (_tankHealth != null)
        {
            _tankHealth.Death += OnTankDeath;
        }
    }

    private void OnDestroy()
    {
        if (_tankHealth != null)
        {
            _tankHealth.Death -= OnTankDeath;
        }
    }

    private void OnTankDeath(TeamAssignment killedBy)
    {
        Debug.Log($"[TankDeathHandler] Tank '{gameObject.name}' has been destroyed!");

        // Remove from unit selection
        if (_unit != null && UnitSelection.Instance != null)
        {
            UnitSelection.Instance.UnitDeath(_unit);
        }

        // Disable AI if this tank has one
        var aiTankController = GetComponent<Tanks.AIBehaviorTree.AITankController>();
        if (aiTankController != null)
        {
            aiTankController.enabled = false;
        }

        // Could broadcast a game event here for scoring
        // Example: GameEventBroadcaster.BroadcastTankKilled(gameObject, killedBy);

        // Tank stays disabled until respawned
        // The game respawn system will call TankHealth.Resurrect() when ready
    }
}

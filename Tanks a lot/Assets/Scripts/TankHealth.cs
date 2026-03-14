using UnityEngine;

/// <summary>
/// Health component for tanks
/// Handles damage, death, and broadcasts events
/// </summary>
public class TankHealth : MonoBehaviour
{
    private float _currentHealth;
    private float _maxHealth;
    private bool _isDead = false;
    private TeamAssignment _teamAssignment;

    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;
    public bool IsDead => _isDead;

    // Event broadcasted when tank takes damage
    public delegate void OnDamageTaken(float damage, float remainingHealth);
    public event OnDamageTaken DamageTaken;

    // Event broadcasted when tank dies
    public delegate void OnDeath(TeamAssignment killedBy);
    public event OnDeath Death;

    private void Awake()
    {
        _teamAssignment = GetComponent<TeamAssignment>();
        if (_teamAssignment == null)
        {
            Debug.LogWarning($"[TankHealth] Tank '{gameObject.name}' has no TeamAssignment component!");
        }

        // Load max health from GameParameters
        GameParameters parameters = Resources.Load<GameParameters>("GameParameters");
        if (parameters != null)
        {
            _maxHealth = parameters.TankHealth;
        }
        else
        {
            Debug.LogError("[TankHealth] GameParameters not found in Resources folder!");
            _maxHealth = 100f; // Fallback
        }

        _currentHealth = _maxHealth;
    }

    /// <summary>
    /// Apply damage to this tank
    /// </summary>
    /// <param name="damageAmount">Amount of damage to apply</param>
    /// <param name="damageSource">The team that dealt this damage (for tracking who killed this tank)</param>
    public void TakeDamage(float damageAmount, TeamAssignment damageSource = null)
    {
        if (_isDead)
            return;

        // Block friendly fire (tanks can't damage their own team)
        if (damageSource != null && !damageSource.IsEnemyOf(_teamAssignment))
        {
            Debug.Log($"[TankHealth] Friendly fire blocked on {gameObject.name}");
            return;
        }

        _currentHealth -= damageAmount;
        Debug.Log($"[TankHealth] {gameObject.name} took {damageAmount} damage. Health: {_currentHealth}/{_maxHealth}");

        DamageTaken?.Invoke(damageAmount, _currentHealth);

        if (_currentHealth <= 0)
        {
            Die(damageSource);
        }
    }

    /// <summary>
    /// Heal the tank
    /// </summary>
    public void Heal(float healAmount)
    {
        if (_isDead)
            return;

        _currentHealth = Mathf.Min(_currentHealth + healAmount, _maxHealth);
        Debug.Log($"[TankHealth] {gameObject.name} healed to {_currentHealth}/{_maxHealth}");
    }

    /// <summary>
    /// Fully restore health
    /// </summary>
    public void RestoreFullHealth()
    {
        _currentHealth = _maxHealth;
    }

    /// <summary>
    /// Kill the tank
    /// </summary>
    private void Die(TeamAssignment killedBy)
    {
        if (_isDead)
            return;

        _isDead = true;
        Debug.Log($"[TankHealth] {gameObject.name} died! Killed by team: {killedBy?.CurrentTeam ?? TeamAssignment.Team.Neutral}");

        Death?.Invoke(killedBy);

        // Disable the tank (can be respawned later)
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Resurrect the tank (for respawning)
    /// </summary>
    public void Resurrect()
    {
        _isDead = false;
        _currentHealth = _maxHealth;
        gameObject.SetActive(true);
        Debug.Log($"[TankHealth] {gameObject.name} resurrected!");
    }

    /// <summary>
    /// Get health as percentage (0-1)
    /// </summary>
    public float GetHealthPercent() => Mathf.Clamp01(_currentHealth / _maxHealth);
}

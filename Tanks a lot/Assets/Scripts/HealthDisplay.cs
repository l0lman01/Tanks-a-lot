using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays tank health as a bar above the tank
/// Requires a Canvas with Image component for the health bar
/// </summary>
public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Canvas healthCanvas;
    [SerializeField] private Image healthBarImage;
    private Text healthText;

    private TankHealth _tankHealth;
    private RectTransform _canvasRectTransform;

    private void Awake()
    {
        _tankHealth = GetComponent<TankHealth>();

        if (_tankHealth != null)
        {
            _tankHealth.DamageTaken += OnDamageTaken;
            _tankHealth.Death += OnTankDeath;
        }

        if (healthCanvas == null)
        {
            // Try to find health canvas in children
            healthCanvas = GetComponentInChildren<Canvas>();
        }

        if (healthCanvas != null)
        {
            _canvasRectTransform = healthCanvas.GetComponent<RectTransform>();
        }

        if (healthBarImage == null)
        {
            healthBarImage = GetComponentInChildren<Image>();
        }

        if (healthText == null)
        {
            healthText = GetComponentInChildren<Text>();
        }

        // Initial update
        UpdateHealthDisplay();
    }

    private void OnDestroy()
    {
        if (_tankHealth != null)
        {
            _tankHealth.DamageTaken -= OnDamageTaken;
            _tankHealth.Death -= OnTankDeath;
        }
    }

    private void OnDamageTaken(float damage, float remainingHealth)
    {
        UpdateHealthDisplay();
    }

    private void OnTankDeath(TeamAssignment killedBy)
    {
        // Hide health bar when tank dies
        if (healthCanvas != null)
        {
            healthCanvas.gameObject.SetActive(false);
        }
    }

    private void UpdateHealthDisplay()
    {
        if (_tankHealth == null)
            return;

        float healthPercent = _tankHealth.GetHealthPercent();

        // Update health bar fill
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = healthPercent;

            // Change color based on health
            if (healthPercent > 0.5f)
                healthBarImage.color = Color.green;
            else if (healthPercent > 0.25f)
                healthBarImage.color = Color.yellow;
            else
                healthBarImage.color = Color.red;
        }

        // Update text label
        if (healthText != null)
        {
            healthText.text = $"{_tankHealth.CurrentHealth:F0}/{_tankHealth.MaxHealth:F0}";
        }
    }

    private void LateUpdate()
    {
        // Keep health bar above tank (if using world space canvas)
        if (healthCanvas != null && healthCanvas.renderMode == RenderMode.WorldSpace)
        {
            // The canvas should already be positioned correctly as a child
            // This is just here for reference if you need dynamic positioning
        }
    }
}

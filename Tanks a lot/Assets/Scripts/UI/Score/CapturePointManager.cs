using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton that manages all capture points on the map
/// Used by AI tanks to navigate to strategic objectives
/// </summary>
public class CapturePointManager : MonoBehaviour
{
    public static CapturePointManager Instance { get; private set; }

    private List<Transform> _capturePoints = new List<Transform>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Auto-discover capture points in scene (optional - can also manually register)
        var captureZones = FindObjectsOfType<ScoreZone>();
        foreach (var zone in captureZones)
        {
            RegisterCapturePoint(zone.transform);
        }

        Debug.Log($"[CapturePointManager] Found {_capturePoints.Count} capture point(s).");
    }

    /// <summary>
    /// Register a capture point with the manager
    /// </summary>
    public void RegisterCapturePoint(Transform capturePoint)
    {
        if (!_capturePoints.Contains(capturePoint))
        {
            _capturePoints.Add(capturePoint);
        }
    }

    /// <summary>
    /// Get the nearest capture point to a given position
    /// </summary>
    public Transform GetNearestCapturePoint(Vector3 position)
    {
        if (_capturePoints.Count == 0)
        {
            Debug.LogWarning("[CapturePointManager] No capture points registered!");
            return null;
        }

        Transform nearest = _capturePoints[0];
        float minDistance = Vector3.Distance(position, nearest.position);

        for (int i = 1; i < _capturePoints.Count; i++)
        {
            float distance = Vector3.Distance(position, _capturePoints[i].position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = _capturePoints[i];
            }
        }

        return nearest;
    }

    /// <summary>
    /// Get all registered capture points
    /// </summary>
    public List<Transform> GetAllCapturePoints() => new List<Transform>(_capturePoints);

    /// <summary>
    /// Get capture point count
    /// </summary>
    public int CapturePointCount => _capturePoints.Count;
}

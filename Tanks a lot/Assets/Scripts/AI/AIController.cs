using System.Collections.Generic;
using UnityEngine;

namespace Tanks.AIBehaviorTree
{
    /// <summary>
    /// Singleton controller that manages all AI tanks and executes their behavior trees each frame
    /// </summary>
    public class AIController : MonoBehaviour
    {
        public static AIController Instance { get; private set; }

        [SerializeField] private float executionInterval = 0.1f; // Execute AI every 0.1 seconds for performance

        private List<AITankController> _aiTanks = new List<AITankController>();
        private float _executionTimer = 0f;

        public Transform CurrentTank { get; private set; }

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

        private void Update()
        {
            _executionTimer += Time.deltaTime;

            if (_executionTimer >= executionInterval)
            {
                ExecuteAIFrame();
                _executionTimer = 0f;
            }
        }

        /// <summary>
        /// Register an AI tank controller with this manager
        /// </summary>
        public void RegisterAITank(AITankController aiTank)
        {
            if (!_aiTanks.Contains(aiTank))
            {
                _aiTanks.Add(aiTank);
            }
        }

        /// <summary>
        /// Unregister an AI tank (e.g., when destroyed)
        /// </summary>
        public void UnregisterAITank(AITankController aiTank)
        {
            _aiTanks.Remove(aiTank);
        }

        /// <summary>
        /// Execute behavior trees for all active AI tanks
        /// </summary>
        private void ExecuteAIFrame()
        {
            for (int i = _aiTanks.Count - 1; i >= 0; i--)
            {
                if (_aiTanks[i] != null)
                {
                    _aiTanks[i].ExecuteTree();
                    CurrentTank = _aiTanks[i].TankTransform;
                }
                else
                {
                    _aiTanks.RemoveAt(i); // Clean up destroyed tanks
                }
            }
        }

        /// <summary>
        /// Get all registered AI tanks (for initialization purposes)
        /// </summary>
        public List<AITankController> GetAITanks() => new List<AITankController>(_aiTanks);
    }
}

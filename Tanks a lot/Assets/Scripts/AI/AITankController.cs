using UnityEngine;

namespace Tanks.AIBehaviorTree
{
    /// <summary>
    /// Component that manages behavior tree execution for a single AI tank
    /// Attach this to each AI-controlled tank
    /// </summary>
    public class AITankController : MonoBehaviour
    {
        [SerializeField] private TankController tankController;
        [SerializeField] private Transform[] enemyTankTransforms;
        [SerializeField] private float detectionRadius = 15f;
        [SerializeField] private float viewAngle = 90f;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float shootCooldown = 0.5f;

        private BehaviorNode _behaviorTreeRoot;
        private bool _isInitialized = false;

        public Transform TankTransform => transform;
        public BehaviorNode BehaviorTree => _behaviorTreeRoot;

        private void Awake()
        {
            // Auto-find tank controller if not assigned
            if (tankController == null)
                tankController = GetComponent<TankController>();

            if (tankController == null)
                Debug.LogError("[AITankController] TankController not found on this GameObject!");
        }

        private void OnEnable()
        {
            // Initialize and register with AIController when enabled
            if (!_isInitialized && tankController != null)
            {
                Initialize();
            }

            if (_isInitialized && AIController.Instance != null)
            {
                AIController.Instance.RegisterAITank(this);
            }
        }

        private void OnDisable()
        {
            // Unregister from AIController when disabled
            if (AIController.Instance != null)
            {
                AIController.Instance.UnregisterAITank(this);
            }
        }

        /// <summary>
        /// Initialize the behavior tree for this AI tank
        /// </summary>
        public void Initialize()
        {
            if (_isInitialized)
                return;

            // Find enemy tanks if not assigned
            if (enemyTankTransforms == null || enemyTankTransforms.Length == 0)
            {
                // Find all tanks except this one
                var allTanks = FindObjectsOfType<TankController>();
                var enemyList = new System.Collections.Generic.List<Transform>();
                foreach (var tank in allTanks)
                {
                    if (tank.gameObject != gameObject)
                    {
                        enemyList.Add(tank.transform);
                    }
                }
                enemyTankTransforms = enemyList.ToArray();

                if (enemyTankTransforms.Length == 0)
                    Debug.LogWarning("[AITankController] No enemy tanks found!");
            }

            // Create behavior tree
            _behaviorTreeRoot = AIBehaviorTreeFactory.CreateAIBehaviorTree(
                transform,
                tankController,
                enemyTankTransforms,
                CapturePointManager.Instance,
                detectionRadius,
                viewAngle,
                moveSpeed,
                shootCooldown
            );

            if (_behaviorTreeRoot == null)
            {
                Debug.LogError("[AITankController] Failed to create behavior tree!");
                return;
            }

            _isInitialized = true;
            Debug.Log($"[AITankController] AI tank '{gameObject.name}' initialized with behavior tree.");
        }

        /// <summary>
        /// Execute one tick of the behavior tree
        /// Called by AIController each frame
        /// </summary>
        public void ExecuteTree()
        {
            if (!_isInitialized || _behaviorTreeRoot == null)
                return;

            try
            {
                _behaviorTreeRoot.Execute();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[AITankController] Error executing behavior tree: {ex.Message}");
            }
        }

        /// <summary>
        /// Manually set enemy targets for this AI
        /// Useful for dynamic enemy spawn/death
        /// </summary>
        public void SetEnemyTargets(Transform[] newEnemies)
        {
            enemyTankTransforms = newEnemies;
            _isInitialized = false; // Force re-initialization on next frame
        }

        /// <summary>
        /// Get current behavior tree (for debugging)
        /// </summary>
        public BehaviorNode GetBehaviorTree()
        {
            return _behaviorTreeRoot;
        }
    }
}

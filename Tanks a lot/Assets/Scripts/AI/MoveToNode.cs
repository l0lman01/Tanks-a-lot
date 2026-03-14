using UnityEngine;

namespace Tanks.AIBehaviorTree
{
    /// <summary>
    /// Node that handles AI tank movement - moves to target or capture point
    /// </summary>
    public class MoveToNode : LeafBehaviorNode
    {
        [SerializeField] protected float moveSpeed = 5f;

        private Transform _tankTransform;
        private TankController _tankController;
        private Transform _target;
        private CapturePointManager _capturePoints;

        /// <summary>
        /// Set up the node with tank references, target and capture points manager
        /// </summary>
        public void Initialize(Transform tankTransform, TankController tankController, Transform target = null, CapturePointManager capturePoints = null)
        {
            _tankTransform = tankTransform;
            _tankController = tankController;
            _target = target;
            _capturePoints = capturePoints;
        }

        /// <summary>
        /// Update AI tank movement towards target or capture point
        /// </summary>
        public override BehaviorNode.State Execute()
        {
            OnEnter();

            if (_tankTransform == null || _tankController == null)
            {
                Debug.LogWarning("[AI] MoveToNode: Tank references not initialized!");
                return BehaviorNode.State.Failure;
            }

            Transform targetPosition = _target;

            // If no explicit target, find nearest capture point
            if (targetPosition == null && _capturePoints != null)
            {
                targetPosition = _capturePoints.GetNearestCapturePoint(_tankTransform.position);
            }

            // Move toward target
            if (targetPosition != null)
            {
                Vector3 direction = (targetPosition.position - _tankTransform.position).normalized;
                Vector2 moveVector = new Vector2(direction.x, direction.z).normalized;
                
                _tankController.HandleMoveBody(moveVector);
                
                OnExit();
                return BehaviorNode.State.Running; // Running = still moving
            }

            Debug.LogWarning("[AI] MoveToNode: No target or capture point found!");
            OnExit();
            return BehaviorNode.State.Failure;
        }

        /// <summary>
        /// Set a new target for this move node
        /// </summary>
        public void SetTarget(Transform newTarget)
        {
            _target = newTarget;
        }
    }
}

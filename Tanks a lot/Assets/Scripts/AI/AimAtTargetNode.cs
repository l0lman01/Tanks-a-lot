using UnityEngine;

namespace Tanks.AIBehaviorTree
{
    /// <summary>
    /// Leaf node that aims tank turrets at a target
    /// </summary>
    public class AimAtTargetNode : LeafBehaviorNode
    {
        private TankController _tankController;
        private Transform _target;

        /// <summary>
        /// Initialize this node with tank controller and target
        /// </summary>
        public void Initialize(TankController tankController, Transform target = null)
        {
            _tankController = tankController;
            _target = target;
        }

        /// <summary>
        /// Set the target to aim at
        /// </summary>
        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public override BehaviorNode.State Execute()
        {
            OnEnter();

            if (_tankController == null || _target == null)
            {
                Debug.LogWarning("[AI] AimAtTargetNode: Tank controller or target not set!");
                return BehaviorNode.State.Failure;
            }

            // Get target world position
            Vector3 targetPosition = _target.position;

            // Call turret movement with target position
            _tankController.HandleTurretMovement((Vector2)targetPosition);

            OnExit();
            return BehaviorNode.State.Success; // Aiming is immediate success
        }
    }
}

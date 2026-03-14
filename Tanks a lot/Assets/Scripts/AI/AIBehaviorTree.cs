using UnityEngine;

namespace Tanks.AIBehaviorTree
{
    /// <summary>
    /// Factory for creating behavior trees for AI tanks
    /// </summary>
    public static class AIBehaviorTreeFactory
    {
        /// <summary>
        /// Create a complete behavior tree for an AI tank
        /// Tree structure:
        /// - ROOT: Selector
        ///   - Branch 1: Sequence (Attack if target detected)
        ///     - Detect targets
        ///     - Aim at target
        ///     - Shoot
        ///   - Branch 2: Move to capture point
        /// </summary>
        public static BehaviorNode CreateAIBehaviorTree(
            Transform tankTransform,
            TankController tankController,
            Transform[] enemyTransforms,
            CapturePointManager capturePointManager = null,
            float detectionRadius = 15f,
            float viewAngle = 90f,
            float moveSpeed = 5f,
            float shootCooldown = 0.5f)
        {
            // Create leaf nodes
            var detectionNode = new IsTargetDetectedNode();
            detectionNode.Initialize(tankTransform, enemyTransforms);

            var aimNode = new AimAtTargetNode();
            aimNode.Initialize(tankController, null); // Target will be set dynamically

            var shootNode = new ShootNode();
            shootNode.Initialize(tankController);

            var moveNode = new MoveToNode();
            moveNode.Initialize(tankTransform, tankController, null, capturePointManager);

            // Create composite nodes - Attack sequence (Detect, Aim, Shoot)
            var attackSequence = new SequenceNode();
            attackSequence.AddChild(detectionNode);
            attackSequence.AddChild(aimNode);
            attackSequence.AddChild(shootNode);

            // Create root selector (Attack OR Move)
            var rootSelector = new SelectorNode();
            rootSelector.AddChild(attackSequence);
            rootSelector.AddChild(moveNode);

            return rootSelector;
        }

        /// <summary>
        /// Create a simple behavior tree that just moves
        /// Useful for debugging
        /// </summary>
        public static BehaviorNode CreateSimpleMovementTree(
            Transform tankTransform,
            TankController tankController,
            CapturePointManager capturePointManager,
            float moveSpeed = 5f)
        {
            var moveNode = new MoveToNode();
            moveNode.Initialize(tankTransform, tankController, null, capturePointManager);
            return moveNode;
        }

        /// <summary>
        /// Create a simple behavior tree that just shoots
        /// Useful for debugging
        /// </summary>
        public static BehaviorNode CreateSimpleShootTree(TankController tankController, float shootCooldown = 0.5f)
        {
            var shootNode = new ShootNode();
            shootNode.Initialize(tankController);
            return shootNode;
        }
    }
}

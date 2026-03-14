using System.Collections.Generic;
using UnityEngine;

namespace Tanks.AIBehaviorTree
{
    /// <summary>
    /// Node that checks if target is detected by the AI tank
    /// </summary>
    public class IsTargetDetectedNode : LeafBehaviorNode
    {
        [SerializeField] protected float detectionRadius = 10f;
        [SerializeField] protected float viewAngle = 45f;

        private Transform _tankTransform;
        private Transform[] _targets;
        private List<AIInfo> _detectedTargets = new List<AIInfo>();

        /// <summary>
        /// Set the tank this node belongs to and its current targets
        /// </summary>
        public void Initialize(Transform tank, Transform[] targetTransforms)
        {
            _tankTransform = tank;

            var targets = new List<Transform>();
            if (targetTransforms != null)
                targets.AddRange(targetTransforms);

            _targets = targets.ToArray();
        }

        private bool IsInFieldOfView(Transform target)
        {
            if (_tankTransform == null || target == null)
                return false;

            float angle = Vector3.SignedAngle(Vector3.up, (target.position - _tankTransform.position).normalized, Vector3.right);
            return Mathf.Abs(angle) <= viewAngle / 2;
        }

        public override BehaviorNode.State Execute()
        {
            OnEnter();

            // Clear detected targets each frame
            _detectedTargets.Clear();

            // Check all targets for detection
            if (_tankTransform != null && _targets != null)
            {
                foreach (var target in _targets)
                {
                    if (target == null) continue;
                    
                    float distance = Vector3.Distance(_tankTransform.position, target.position);
                    if (distance < detectionRadius && IsInFieldOfView(target))
                    {
                        _detectedTargets.Add(new AIInfo(target, _tankTransform));
                    }
                }
            }

            var result = _detectedTargets.Count > 0
                ? BehaviorNode.State.Success
                : BehaviorNode.State.Failure;

            if (_detectedTargets.Count > 0)
                Debug.Log($"[AI] Tank detected {_detectedTargets.Count} target(s).");
                
            OnExit();
            return result;
        }

        /// <summary>
        /// Get detected targets from the latest Execute() call
        /// </summary>
        public List<AIInfo> GetDetectedTargets() => new List<AIInfo>(_detectedTargets);
    }

    /// <summary>
    /// Target information shared between nodes
    /// </summary>
    public class AIInfo
    {
        public Transform Tank { get; }
        private Transform _aiTank;

        public float DistanceToAI => _aiTank != null ? Vector3.Distance(_aiTank.position, Tank.position) : float.MaxValue;

        public AIInfo(Transform tank, Transform aiTank = null)
        {
            Tank = tank;
            _aiTank = aiTank;
        }

        public void SetAITankReference(Transform aiTank)
        {
            _aiTank = aiTank;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Tanks.AIBehaviorTree
{
    /// <summary>
    /// Shared data dictionary for AI behavior tree nodes to communicate state
    /// </summary>
    public class Blackboard : ScriptableObject
    {
        private Dictionary<string, object> _data = new Dictionary<string, object>();
        private List<BlackboardDataCallback> _onValueChanged = new List<BlackboardDataCallback>();

        public void SetValue(string key, object value)
        {
            if (_data.ContainsKey(key))
                _data[key] = value;
            else
                _data.Add(key, value);

            foreach (var callback in _onValueChanged)
            {
                callback?.Invoke(key, value);
            }
        }

        public T Get<T>(string key) where T : class
        {
            if (_data.TryGetValue(key, out var value))
            {
                return value as T;
            }
            return null;
        }

        public object this[string key] => _data.ContainsKey(key) ? _data[key] : null;
        public void Set(string key, object value) => SetValue(key, value);

        public delegate void BlackboardDataCallback(string key, object value);

        public event BlackboardDataCallback OnValueChanged;

        public void AddValueChangedListener(BlackboardDataCallback callback)
        {
            if (!_onValueChanged.Contains(callback))
                _onValueChanged.Add(callback);
        }
    }
}

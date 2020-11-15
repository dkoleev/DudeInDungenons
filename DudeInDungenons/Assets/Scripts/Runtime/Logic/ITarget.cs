using UnityEngine;

namespace Runtime.Logic {
    public interface ITarget {
        bool IsReachable { get; }
        Transform Transform { get; }
    }
}

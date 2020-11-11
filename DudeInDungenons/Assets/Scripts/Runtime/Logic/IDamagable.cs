using UnityEngine;

namespace Runtime.Logic {
    public interface IDamagable {
        Transform MainTransform { get; }
        void TakeDamage(int damage);
    }
}
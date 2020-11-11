using UnityEngine;

namespace Runtime.Logic {
    public interface IDamagable {
        Transform Transform { get; }
        void TakeDamage(int damage);
    }
}
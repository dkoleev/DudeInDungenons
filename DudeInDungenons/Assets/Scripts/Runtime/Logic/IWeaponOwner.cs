using UnityEngine;

namespace Runtime.Logic {
    public interface IWeaponOwner {
        Transform RaycastStartPoint { get; }
        Transform RotateTransform { get; }
        Transform MainTransform { get; }
    }
}

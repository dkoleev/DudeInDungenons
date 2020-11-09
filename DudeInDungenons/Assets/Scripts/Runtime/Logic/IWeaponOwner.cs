using UnityEngine;

namespace Runtime.Logic {
    public interface IWeaponOwner {
        Transform RaycastStartPoint { get; }
    }
}

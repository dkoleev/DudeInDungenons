using Input;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    private Vector2 _moveAxis;
    
    private void Awake() {
        _moveAxis = Vector2.zero;
        
        InputManager.Instance.MainControl.Player.Move.performed += MoveOnPerformed;
        InputManager.Instance.MainControl.Player.Move.canceled += MoveOnCanceled; 
        InputManager.Instance.MainControl.Player.Enable();
    }

    private void Update() {
        if (_moveAxis.magnitude > 0) {
            transform.position += new Vector3(
                _moveAxis.x * Time.deltaTime * 5,
                0,
                _moveAxis.y * Time.deltaTime * 5);
        }
    }

    private void MoveOnCanceled(InputAction.CallbackContext obj) {
        _moveAxis = Vector2.zero;
    }

    private void MoveOnPerformed(InputAction.CallbackContext context) {
        _moveAxis = context.ReadValue<Vector2>();
    }

}
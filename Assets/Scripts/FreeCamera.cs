using UnityEngine;
using UnityEngine.InputSystem;

public class FreeCameraController : MonoBehaviour
{
    public float moveSpeed = 100f;
    public float lookSpeed = 5f;

    private Vector2 lookInput;
    private Vector3 moveInput;

    void Update()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;

        if (keyboard == null || mouse == null) return;

        // Look (Mouse delta)
        lookInput = mouse.delta.ReadValue() * lookSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, lookInput.x, Space.World);
        transform.Rotate(Vector3.left, lookInput.y, Space.Self);

        // Move
        moveInput = Vector3.zero;
        if (keyboard.wKey.isPressed) moveInput += transform.forward;
        if (keyboard.sKey.isPressed) moveInput -= transform.forward;
        if (keyboard.aKey.isPressed) moveInput -= transform.right;
        if (keyboard.dKey.isPressed) moveInput += transform.right;
        if (keyboard.eKey.isPressed) moveInput += transform.up;
        if (keyboard.qKey.isPressed) moveInput -= transform.up;

        transform.position += moveInput * moveSpeed * Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    public bool freeCam = true;
    public float cameraSpeed = 1f;

    [SerializeField]
    private PlayerInputActions inputActions;
    private InputAction moveAction;

    public void SelectFollowTarget()
    {
        var vcam = GetComponent<CinemachineVirtualCamera>();
    }

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        moveAction = inputActions.Player.Move;
        moveAction.Enable();
    }

    private Vector2 MovementInput()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();

        //normalize in case of diagonal movement direction
        if (input.magnitude > 1)
            input = input.normalized;

        return input;
    }

    void FixedUpdate()
    {
        if (!freeCam)
            return;

        transform.Translate(MovementInput() * Time.deltaTime * cameraSpeed);
    }
}

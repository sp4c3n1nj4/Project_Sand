using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//uaing input system for unity input actions
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction move;
    [SerializeField]
    private bool pawnSelected;

    [SerializeField]
    private PlayerInputActions inputActions;
    [SerializeField]
    private GameObject vCam;
    [SerializeField]
    private float speed, cameraSpeed;
    [SerializeField]
    private GameObject pawn;

    public void SelectPawn(GameObject newPawn)
    {
        if (newPawn == null)  
            return;
        if (pawn != null)
            UnselectPawn();

        pawn = newPawn;
        pawnSelected = true;
        pawn.GetComponent<Soldier>().controlled = true;

        vCam.GetComponent<CinemachineVirtualCamera>().Follow = pawn.transform;
    }

    public void UnselectPawn()
    {
        pawn.GetComponent<Soldier>().controlled = false;
        pawn = null;
        pawnSelected = false;

        vCam.GetComponent<CinemachineVirtualCamera>().Follow = null;
    }

    //set up input actions at the start
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = inputActions.Player.Move;
        move.Enable();
    }

    //update physics of player
    private void FixedUpdate()
    {
        if (pawnSelected)
        {
            MovePlayer();
        }
        else
        {
            MoveCamera();
        }
    }

    //move player in fixed update
    private void MovePlayer()
    {
        pawn.transform.Translate(MovementInput() * (Time.fixedDeltaTime * speed));
    }

    private void MoveCamera()
    {
        vCam.transform.Translate(MovementInput() * (Time.fixedDeltaTime * cameraSpeed));
    }

    //convert player input to vector3
    private Vector2 MovementInput()
    {
        Vector2 input = move.ReadValue<Vector2>();

        //normalize in case of diagonal movement direction
        if (input.magnitude > 1)
            input = input.normalized;

        return input;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class TruckBehaviour : MonoBehaviour
{
    [SerializeField] public Rigidbody2D car;
    [SerializeField] private Rigidbody2D wheelL;
    [SerializeField] private Rigidbody2D wheelR;
    [SerializeField] private float speed = 1f;
    [SerializeField] private WheelBehaviour wheelLBehaviour;
    [SerializeField] private WheelBehaviour wheelRBehaviour;

    private bool movement;
    private bool breaking;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public static TruckBehaviour Instance;

    private void Awake()
    {
        Instance = this;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        //car.centerOfMass = new Vector2(0, 0f);
    }

    private void Update()
    {
        if(movement) MovementAction();
        if(breaking) BreakingAction();
        else UnBreakingAction();
    }

    public void Move(MovementState movementState)
    {
        switch (movementState)
        {
            case MovementState.MOVEMENTSTART:
                movement = true;
                break;
            case MovementState.MOVEMENTEND:
                movement = false;
                break;
            case MovementState.BREAKINGSTART:
                breaking = true;
                break;
            case MovementState.BREAKINGEND:
                breaking = false;
                break;
        }
    }

    public void EndMovement()
    {
        movement = false;
        breaking = false;
    }

    private void UnBreakingAction()
    {
        car.drag = 0;
        wheelL.angularDrag = 2;
        wheelR.angularDrag = 2;
    }

    private void MovementAction()
    {
        car.AddRelativeForce(Vector3.right * speed * Time.fixedDeltaTime);
    }

    private void BreakingAction()
    {
        if (wheelLBehaviour.isMakingContact && wheelRBehaviour.isMakingContact)
        {
            car.drag = 1000;
        }
        wheelL.angularDrag = 1000;
        wheelR.angularDrag = 1000;
    }

    public void ResetPosition()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        car.velocity = Vector2.zero;
        car.rotation = 0;
        car.angularVelocity = 0;
        wheelL.velocity = Vector2.zero;
        wheelL.rotation = 0;
        wheelL.angularVelocity = 0;
        wheelR.velocity = Vector2.zero;
        wheelR.rotation = 0;
        wheelR.angularVelocity = 0;
    }
}
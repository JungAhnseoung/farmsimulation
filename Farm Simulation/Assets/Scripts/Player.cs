using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float xInput;
    private float yInput;
    private bool isIdle;
    private bool isWalking;
    private bool isRunning;
    private bool isHolding;
    private bool idleRight;
    private bool idleLeft;
    private bool idleUp;
    private bool idleDown;
    private bool isToolRight;
    private bool isToolLeft;
    private bool isToolUp;
    private bool isToolDown;
    private bool isPullRight;
    private bool isPullLeft;
    private bool isPullUp;
    private bool isPullDown;
    private bool isWaterRight;
    private bool isWaterLeft;
    private bool isWaterUp;
    private bool isWaterDown;
    private bool isReapRight;
    private bool isReapLeft;
    private bool isReapUp;
    private bool isReapDown;

    private Rigidbody2D rigidBody2D;
    private float movementSpeed;
    private Direction direction;

    public const float walkingSpeed = 15.0f;
    public const float runningSpeed = 25.0f;

    public enum Direction
    {
        up,
        down,
        left,
        right,
        none
    }

    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isToolRight = false;
        isToolLeft = false;
        isToolUp = false;
        isToolDown = false;
        isPullRight = false;
        isPullLeft = false;
        isPullUp = false;
        isPullDown = false;
        isWaterRight = false;
        isWaterLeft = false;
        isWaterUp = false;
        isWaterDown = false;
        isReapRight = false;
        isReapLeft = false;
        isReapUp = false;
        isReapDown = false;
        
        MovementInput();
        //transform.Translate(xInput * movementSpeed * Time.deltaTime, yInput * movementSpeed * Time.deltaTime, 0);
        rigidBody2D.MovePosition(transform.position + new Vector3(xInput * movementSpeed * Time.deltaTime, yInput * movementSpeed * Time.deltaTime, 0));
        EventHandler.CallActionEvent(xInput, yInput, isIdle, isWalking, isRunning, isHolding,
            idleRight, idleLeft, idleUp, idleDown,
            isToolRight, isToolLeft, isToolUp, isToolDown,
            isPullRight, isPullLeft, isPullUp, isPullDown,
            isWaterRight, isWaterLeft, isWaterUp, isWaterDown,
            isReapRight, isReapLeft, isReapUp, isReapDown);

      }


    private void MovementInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        

        if(xInput !=0 || yInput != 0)
        {
            isWalking = true;
            isRunning = false;
            isIdle = false;
            movementSpeed = walkingSpeed;

            if (xInput > 0) direction = Direction.right;
            else if (xInput < 0) direction = Direction.left;
            else if (yInput > 0) direction = Direction.up;
            else if (yInput < 0) direction = Direction.down;
        }
        else if(xInput == 0 && yInput ==0)
        {
            isRunning = false;
            isWalking = false;
            isIdle = true;
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = runningSpeed;
        }
        else
        {
            isWalking = true;
            isRunning = false;
            isIdle = false;
            movementSpeed = walkingSpeed;
        }
    }

}

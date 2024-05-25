using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 7f;
    [SerializeField] private GameInput _gameInput;

    private bool isWalking;
    private void Update()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = Time.deltaTime * moveSpeed;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //cant move towards moveDir
            
            //attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0); 
            canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius, moveDirX, moveDistance);

            if (canMove) //can move only on the x
                moveDir = moveDirX;
            else //cant move only on the x
            {
                //attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z); 
                canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius, moveDirZ, moveDistance);
                
                if (canMove) //can move only on the Z
                    moveDir = moveDirZ;
                else
                {
                    //cant move in any dir
                }
            }
        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
            
        }

        isWalking = moveDir != Vector3.zero;
        
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
